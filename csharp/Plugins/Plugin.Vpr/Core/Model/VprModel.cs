using NAudio.Wave;
using Newtonsoft.Json;
using OpenSvip.Framework;
using Plugin.Vpr.Core.Model.Track;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Plugin.Vpr.Core.Model
{
    public class AudioFile
    {
        public string FilePath { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public AudioFile(string path)
        {
            FilePath = path;
            OriginalName = Path.GetFileName(path);
            RandomName();
        }
        public AudioFile(string path, string name)
        {
            FilePath = path;
            OriginalName = name;
            RandomName();
        }
        public void RandomName()
        {
            Name = Guid.NewGuid().ToString() + ".wav";
        }
    }
    public class VprModel
    {
        public Sequence Sequence { get; set; }
        public List<AudioFile> AudioFiles { get; set; } = new List<AudioFile>();

        /// <summary>
        /// 读取 Vpr 序列文件
        /// 不会读取音频文件
        /// </summary>
        /// <param name="path">Vpr 序列文件路径</param>
        /// <param name="options">转换选项</param>
        /// <returns>Vpr 序列模型</returns>
        public static VprModel Read(string path, ConverterOptions options)
        {
            var model = new VprModel();
            using (FileStream zipToOpen = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    var sequenceFileEntry = archive.GetEntry(ProjectFileConstantDocument.ProjectFileSequenceJsonPath);
                    using (StreamReader reader = new StreamReader(sequenceFileEntry.Open()))
                    {
                        var json = reader.ReadToEnd();
                        model.Sequence = JsonConvert.DeserializeObject<Sequence>(json);
                    }
                    if (!options.GetValueAsBoolean("IsCopyAudioFileFromVprFileToVprFileDirectory", true))
                    {
                        return model;
                    }
                    try
                    {
                        var audioDirectoryEntries = archive.Entries;
                        var audioFileNames = model.Sequence.Tracks
                            .OfType<AudioTrack>()
                            .Select(t => t.Parts)
                            .SelectMany(p => p)
                            .ToDictionary(p => p.Wav.Name, p => p.Wav.OriginalName);
                        foreach (var entry in audioDirectoryEntries)
                        {
                            if (entry.FullName.StartsWith(ProjectFileConstantDocument.ProjectFileAudioDirectoryPath) && !string.IsNullOrEmpty(entry.Name))
                            {
                                if (!audioFileNames.TryGetValue(entry.Name, out var originalName))
                                {
                                    continue;
                                }
                                using (var entryStream = entry.Open())
                                {
                                    using (var fileStream = new FileStream(Path.Combine(Path.GetDirectoryName(path), originalName), FileMode.Create, FileAccess.Write, FileShare.None))
                                    {
                                        entryStream.CopyTo(fileStream);
                                        model.AudioFiles.Add(new AudioFile(fileStream.Name, originalName) { Name = entry.Name });
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Warnings.AddWarning($"在读取音频文件时出现异常: {e.Message}", "读取音频文件", WarningTypes.Others);
                    }
                }
            }
            return model;
        }
        /// <summary>
        /// 写入 Vpr 序列文件
        /// </summary>
        /// <param name="path">Vpr 序列文件路径</param>
        /// <param name="options">转换选项</param>
        public void Write(string path, ConverterOptions options)
        {
            using (var archive = ZipFile.Open(path, ZipArchiveMode.Create))
            {
                var sequenceFileEntry = archive.CreateEntry(ProjectFileConstantDocument.ProjectFileSequenceJsonPath);
                using (StreamWriter writer = new StreamWriter(sequenceFileEntry.Open()))
                {
                    writer.Write(JsonConvert.SerializeObject(Sequence, Formatting.None));
                }
                if (!options.GetValueAsBoolean("IsCopyAudioFileToVprFile", true))
                {
                    return;
                }
                foreach (var item in AudioFiles)
                {
                    try
                    {
                        var audioFileEntry = archive.CreateEntry(Path.Combine(ProjectFileConstantDocument.ProjectFileAudioDirectoryPath, item.Name));
                        using (var audioStream = new AudioFileReader(item.FilePath))
                        {
                            // 创建目标格式: 16位, 44.1kHz, 立体声
                            var targetFormat = new WaveFormat(44100, 16, audioStream.WaveFormat.Channels);
                            using (var conversionStream = new MediaFoundationResampler(audioStream, targetFormat))
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    WaveFileWriter.WriteWavFileToStream(memoryStream, conversionStream);
                                    using (var stream = audioFileEntry.Open())
                                    {
                                        memoryStream.Seek(0, SeekOrigin.Begin);
                                        memoryStream.CopyTo(stream);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Warnings.AddWarning($"在复制音频到序列文件时出现异常: {e.Message}", $"在复制 {item.FilePath} 时", WarningTypes.Others);
                    }
                }
            }
        }
    }
}