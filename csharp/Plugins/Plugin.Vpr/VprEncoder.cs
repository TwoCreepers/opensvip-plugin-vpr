using NAudio.Wave;
using OpenSvip.Framework;
using OpenSvip.Library;
using OpenSvip.Model;
using Plugin.Vpr.Core.Model;
using Plugin.Vpr.Core.Model.MasterTrack;
using Plugin.Vpr.Core.Model.Track.Part;
using Plugin.Vpr.Core.phoneme;
using Plugin.Vpr.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;

namespace Plugin.Vpr
{
    public class VprEncoder
    {
        private readonly ConverterOptions _converterOptions;
        public VprEncoder(ConverterOptions options)
        {
            _converterOptions = options;
        }
        public VprModel Encode(Project project)
        {
            // 构造 VprModel 对象
            var model = new VprModel
            {
                Sequence = new Sequence()
            };

            // 设置速度信息
            model.Sequence.MasterTrack.Tempo.Events = project.SongTempoList.Select(it => new TempoEvent
            {
                Position = it.Position,
                Value = (int)it.BPM * 100,
            }).ToList();

            // 设置拍号信息
            model.Sequence.MasterTrack.TimeSignature.Events = project.TimeSignatureList.Select(it => new TimeSigEvent
            {
                Bar = it.BarIndex,
                Numerator = it.Numerator,
                Denominator = it.Denominator,
            }).ToList();

            // 加载音素转换器
            var phonemeConverter = PhonemeParse.CreateConverter(PhonemeLang.zh_ch);

            // 获取我也不知道是什么东西的偏移量，但别人都这么写。
            var firstBarLength = 1920 * (project.TimeSignatureList.First().Numerator / project.TimeSignatureList.First().Denominator);

            // 设置音轨信息
            model.Sequence.Tracks = project.TrackList.Select<Track, TrackBase>(item =>
            {
                switch (item)
                {
                    // 歌唱音轨
                    case SingingTrack singingTrack:
                        var vprSingingTrack = new Plugin.Vpr.Core.Model.Track.SingingTrack
                        {
                            Name = singingTrack.Title,
                            IsMuted = singingTrack.Mute,
                            IsSoloMode = singingTrack.Solo,
                        };
                        // 设置音轨音量与声像
                        vprSingingTrack.Volume.Events[0].Value = (int)singingTrack.Volume * 100; // 不用在意数组越界，VolumeInfo 默认有一个Event元素
                        vprSingingTrack.Panpot.Events[0].Value = (int)singingTrack.Pan * 100;

                        // 转换歌词
                        var originalLyricsList = singingTrack.NoteList
                            .Select(note => note.Lyric);
                        var convertedLyricsList = PinyinUtils.GetPinyinSeries(originalLyricsList);

                        // 构造块
                        var part = new SingingPart();   // OpenSvip 没有 Part 的概念，直接创建一个包含该音轨所有音符的块
                        var partDuration = 0;
                        part.Notes = singingTrack.NoteList.Zip(convertedLyricsList, (note, lyrics) =>
                        {
                            // 尝试获取最靠后的音符位置，加上音符长度作为音轨的持续时间
                            if (note.StartPos + note.Length > partDuration)
                            {
                                partDuration = note.StartPos + note.Length;
                            }
                            // 创建音符对象
                            var vprNote = new Plugin.Vpr.Core.Model.Track.Part.Note.Note
                            {
                                Position = note.StartPos,
                                Duration = note.Length,
                                Number = note.KeyNumber,
                                Lyric = string.IsNullOrEmpty(note.Pronunciation) ? lyrics : note.Pronunciation,
                                Phoneme = phonemeConverter.Convert(string.IsNullOrEmpty(note.Pronunciation) ? lyrics : note.Pronunciation),
                            };
                            return vprNote;
                        })
                        .OrderBy(note => note.Position)
                        .ThenBy(note => note.Duration)
                        .ToList();
                        part.Duration = partDuration + 480 * 4; // 余量

                        // 如果音轨有编辑参数，则添加到块中
                        var editedParams = singingTrack.EditedParams;
                        if (editedParams.Pitch.TotalPointsCount > 0)
                        {
                            var controllerInfos = new List<ControllerInfo>();

                            // 添加音高控制器
                            var noteOffset = 0;
                            var pitchController = new ControllerInfo
                            {
                                Name = ControllerName.pitchBend,
                                Events = editedParams.Pitch.PointList
                                .Where(p => p.Item1 >= firstBarLength)  // 明明就没人用啊，为什么要保留这个破玩意
                                .Select(p => (p.Item1 - firstBarLength, p.Item2))   // 计算实际音高控制器位置
                                .Where(p => p.Item1 >= part.Position && p.Item1 < part.Duration)
                                .Select(p =>
                                {
                                    if (p.Item2 == -100)
                                    {
                                        return new ControllerEvent
                                        {
                                            Position = p.Item1,
                                            Value = 0
                                        };
                                    }
                                    var pitchValue = 0;
                                    for (global::System.Int32 i = noteOffset; i < part.Notes.Count; i++)
                                    {
                                        if (part.Notes[i].Position + part.Notes[i].Duration > p.Item1)
                                        {
                                            // 因为 OpenSvip 格式的音高控制器是绝对音高，所以需要将其转换为相对音高
                                            pitchValue = (p.Item2 - (part.Notes[i].Number * 100)) * 512 / 100;
                                            // 若启用则 去掉 VOCALOID 对该位置最终音高的补偿，使音高参数更贴合最终音高
                                            if (_converterOptions.GetValueAsBoolean("IsEnablesReversePitchCompensation", true))
                                                pitchValue -= VprPitchUtils.PitchPointsCompensation(part.Notes, i, p.Item1);
                                            noteOffset = i;
                                            break;
                                        }
                                    }
                                    if (pitchValue > 8191) Warnings.AddWarning("音高控制器超出限制(>8191)，已忽略", $"在 {p.Item1} 处，值 {pitchValue}，可能的最近音符歌词 {part.Notes[noteOffset].Lyric}", WarningTypes.Params);
                                    if (pitchValue < -8192) Warnings.AddWarning("音高控制器超出限制(<-8192)，已忽略", $"在 {p.Item1} 处，值 {pitchValue}，可能的最近音符歌词 {part.Notes[noteOffset].Lyric}", WarningTypes.Params);
                                    return new ControllerEvent
                                    {
                                        Position = p.Item1,
                                        Value = pitchValue
                                    };
                                })
                                .Where(p => p.Value <= 8191 && p.Value >= -8192)
                                .ToList()
                            };
                            // 添加一个值为16的音高灵敏度控制器，因为前面音高控制器的值是相对于音符音高的，并且假设一个半音高为512，8192 / 512 = 16
                            // 同时这也意味这音高控制器范围只有 ±16 个半音
                            controllerInfos.Add(new ControllerInfo
                            {
                                Name = ControllerName.pitchBendSens,
                                Events = new List<ControllerEvent>
                                {
                                    new ControllerEvent
                                    {
                                        Position = part.Position,
                                        Value = 16
                                    }
                                }
                            });
                            // 添加音高控制器到块中
                            controllerInfos.Add(pitchController);

                            part.Controllers = controllerInfos;
                        }

                        vprSingingTrack.Parts.Add(part);
                        return vprSingingTrack;
                    // 音频音轨
                    case InstrumentalTrack instrumentTrack:
                        var vprInstrumentTrack = new Plugin.Vpr.Core.Model.Track.AudioTrack
                        {
                            Name = instrumentTrack.Title,
                            IsMuted = instrumentTrack.Mute,
                            IsSoloMode = instrumentTrack.Solo,
                        };
                        // 设置音轨音量与声像
                        vprInstrumentTrack.Volume.Events[0].Value = (int)instrumentTrack.Volume * 100; // 不用在意数组越界，VolumeInfo 默认有一个Event元素
                        vprInstrumentTrack.Panpot.Events[0].Value = (int)instrumentTrack.Pan * 100;

                        if (!File.Exists(instrumentTrack.AudioFilePath) && !File.Exists(Path.GetFileName(instrumentTrack.AudioFilePath)))
                        {
                            Warnings.AddWarning($"音频文件不存在，已忽略该音轨: {instrumentTrack.AudioFilePath}", type: WarningTypes.Others);
                            return vprInstrumentTrack;
                        }

                        var audioFile = new AudioFile(instrumentTrack.AudioFilePath);
                        model.AudioFiles.Add(audioFile);

                        // 获取 Wav 文件长度
                        double WavLength;
                        using (var audioFileReader = new AudioFileReader(instrumentTrack.AudioFilePath))
                        {
                            WavLength = audioFileReader.TotalTime.TotalSeconds * (60 / project.SongTempoList.First().BPM);
                        }

                        // 设置音轨音频文件路径
                        vprInstrumentTrack.Parts.Add(new AudioPart
                        {
                            Region = new RegionInfo
                            {
                                Begin = instrumentTrack.Offset,
                                End = instrumentTrack.Offset + WavLength
                            },
                            Wav = new WavInfo
                            {
                                Name = audioFile.Name,
                                OriginalName = audioFile.OriginalName,
                            },
                            Position = instrumentTrack.Offset,
                        });
                        return vprInstrumentTrack;
                    default:
                        Warnings.AddWarning($"无法识别的 OpenSvip 音轨类型: {item.Type}", type: WarningTypes.Others);
                        return new Plugin.Vpr.Core.Model.Track.SingingTrack()
                        {
                            Name = "该音轨可能已损坏或类型信息丢失，无法转换",
                            IsMuted = true,
                        };
                }
            }).ToList();

            return model;
        }
    }
}
