using OpenSvip.Framework;
using OpenSvip.Model;
using Plugin.Vpr.Core.Model;
using Plugin.Vpr.Core.Model.Track.Part;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Plugin.Vpr
{
    public class VprDecoder
    {
        private readonly ConverterOptions _converterOptions;
        public VprDecoder(ConverterOptions options)
        {
            _converterOptions = options;
        }
        public Project Decode(VprModel model, string path)
        {
            // 构造 Project 对象
            var project = new Project
            {
                // 设置速度信息
                SongTempoList = model.Sequence.MasterTrack.Tempo.Events.Select(it => new SongTempo
                {
                    Position = it.Position,
                    BPM = it.Value / 100.0f
                }).ToList(),

                // 设置拍号信息
                TimeSignatureList = model.Sequence.MasterTrack.TimeSignature.Events.Select(it => new TimeSignature
                {
                    BarIndex = it.Bar,
                    Numerator = it.Numerator,
                    Denominator = it.Denominator
                }).ToList(),

                TrackList = model.Sequence.Tracks.Select<TrackBase, Track>(item =>
                {
                    switch (item)
                    {
                        case Plugin.Vpr.Core.Model.Track.SingingTrack singingTrack:
                            // 获取我也不知道是什么东西的偏移量，但别人都这么写。
                            var firstBarLength = 1920 * (model.Sequence.MasterTrack.TimeSignature.Events.First().Numerator / model.Sequence.MasterTrack.TimeSignature.Events.First().Denominator);

                            // 创建音轨
                            var singingTrackResult = new SingingTrack
                            {
                                Title = singingTrack.Name,
                                Mute = singingTrack.IsMuted,
                                Solo = singingTrack.IsSoloMode,
                                NoteList = singingTrack.Parts   // 转换音符
                                    .Select(part => part.Notes.Select(note => new Note
                                    {
                                        StartPos = note.Position + part.Position,
                                        Length = note.Duration,
                                        KeyNumber = note.Number,
                                        Lyric = note.Lyric,
                                        Pronunciation = note.Lyric,
                                    }))
                                    .SelectMany(notes => notes)
                                    .ToList()
                            };

                            // 转换参数
                            var volumeBuffer = new List<Tuple<int, int>>();
                            var breathBuffer = new List<Tuple<int, int>>();
                            var pitchBuffer = new List<Tuple<int, int>>();
                            foreach (var item1 in singingTrack.Parts)
                            {
                                if (item1 == null || item1.Controllers.Count < 1)
                                {
                                    continue;
                                }
                                foreach (var item2 in item1.Controllers)
                                {
                                    if (item2 == null || item2.Events.Count < 1)
                                    {
                                        continue;
                                    }
                                    if (item2.Name == ControllerName.dynamics)
                                    {
                                        foreach (var item3 in item2.Events)
                                        {
                                            volumeBuffer.Add(Tuple.Create(
                                                item3.Position + item1.Position + firstBarLength, // 计算实际于音轨位置: 块起始位置偏移 + OpenSvip谜之规定的第一小节偏移
                                                item3.Value * 2000 / 127 - 1000)); // 将 VPR 的音量范围 [0, 127] 映射到 OpenSvip 的 [-1000, 1000]
                                        }
                                    }
                                    if (item2.Name == ControllerName.breathiness)
                                    {
                                        foreach (var item3 in item2.Events)
                                        {
                                            breathBuffer.Add(Tuple.Create(
                                                item3.Position + item1.Position + firstBarLength, // 计算实际于音轨位置: 块起始位置偏移 + OpenSvip谜之规定的第一小节偏移
                                                item3.Value * 2000 / 127 - 1000)); // 将 VPR 的气声范围 [0, 127] 映射到 OpenSvip 的 [-1000, 1000]
                                        }
                                    }
                                    if (item2.Name == ControllerName.pitchBend)
                                    {
                                        var noteOffset = 0;
                                        var notes = item1.Notes.OrderBy(note => note.Position).ThenBy(note => note.Duration).ToList();
                                        if (item1.Controllers.Any(c => c.Name == ControllerName.pitchBendSens))
                                        {
                                            var pitchBendSens = item1.Controllers.First(c => c.Name == ControllerName.pitchBendSens).Events.OrderBy(it => it.Position).ToList();
                                            var pitchBendSensOffset = 0;
                                            foreach (var item3 in item2.Events)
                                            {
                                                for (global::System.Int32 i = noteOffset; i < notes.Count; i++)
                                                {
                                                    if (item3.Position < notes[i].Position + notes[i].Duration)
                                                    {
                                                        // 计算实际于音轨位置: 块起始位置偏移 + OpenSvip谜之规定的第一小节偏移
                                                        var position = item3.Position + item1.Position + firstBarLength;
                                                        for (global::System.Int32 j = pitchBendSensOffset; j < pitchBendSens.Count; j++)
                                                        {
                                                            if (item3.Position < pitchBendSens[j].Position)
                                                            {
                                                                var PBS = pitchBendSens[j < 1 ? 0 : j - 1].Value;
                                                                pitchBuffer.Add(Tuple.Create(
                                                                    position,
                                                                    notes[i].Number * 100 + item3.Value * 100 / (int)(8192.0 / PBS)));
                                                                pitchBendSensOffset = j;
                                                                break;
                                                            }
                                                        }
                                                        noteOffset = i;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            foreach (var item3 in item2.Events)
                                            {
                                                for (global::System.Int32 i = noteOffset; i < notes.Count; i++)
                                                {
                                                    if (item3.Position < notes[i].Position + notes[i].Duration)
                                                    {
                                                        pitchBuffer.Add(Tuple.Create(
                                                            item3.Position + item1.Position + firstBarLength, // 计算实际于音轨位置: 块起始位置偏移 + OpenSvip谜之规定的第一小节偏移
                                                            notes[i].Number * 100 + item3.Value * 100 / 4096));
                                                        noteOffset = i;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (volumeBuffer.Count > 0)
                            {
                                // OpenSvip 标准规定两端点横坐标分别固定为 -192000 和 1073741823（意义呢？意义在哪呢？取其糟粕，去其精华）
                                singingTrackResult.EditedParams.Volume.PointList.Add(Tuple.Create(-192000, 0));
                                singingTrackResult.EditedParams.Volume.PointList.AddRange(volumeBuffer);
                                singingTrackResult.EditedParams.Volume.PointList.Add(Tuple.Create(1073741823, 0));
                            }
                            if (breathBuffer.Count > 0)
                            {
                                singingTrackResult.EditedParams.Breath.PointList.Add(Tuple.Create(-192000, 0));
                                singingTrackResult.EditedParams.Breath.PointList.AddRange(breathBuffer);
                                singingTrackResult.EditedParams.Breath.PointList.Add(Tuple.Create(1073741823, 0));
                            }
                            if (pitchBuffer.Count > 0)
                            {
                                singingTrackResult.EditedParams.Pitch.PointList.Add(Tuple.Create(-192000, -100));   // 音高中断值为 -100
                                singingTrackResult.EditedParams.Pitch.PointList.AddRange(pitchBuffer);
                                singingTrackResult.EditedParams.Pitch.PointList.Add(Tuple.Create(1073741823, -100));
                            }

                            return singingTrackResult;
                        case Plugin.Vpr.Core.Model.Track.AudioTrack audioTrack:
                            var instrumentTrack = new InstrumentalTrack
                            {
                                Title = audioTrack.Name,
                                Mute = audioTrack.IsMuted,
                                Solo = audioTrack.IsSoloMode,
                            };
                            if (audioTrack.Parts.Count > 0)
                            {
                                instrumentTrack.Offset = audioTrack.Parts.First().Position; // InstrumentalTrack 可没有块的概念，而且一般也没有人会在一个音轨上放多个音频文件，所以直接取第一个块。
                                instrumentTrack.AudioFilePath = Path.Combine(Path.GetDirectoryName(path), audioTrack.Parts.First().Wav.OriginalName);
                            }
                            return instrumentTrack;
                        default:
                            Warnings.AddWarning($"无法识别的 Vpr 音轨类型: {item.GetType().Name}", type: WarningTypes.Others);
                            return new SingingTrack
                            {
                                Title = "该音轨可能已损坏或类型信息丢失，无法转换",
                                Mute = true,
                                Solo = false,
                            };
                    }
                }).ToList()
            };

            return project;
        }
    }
}
