using Newtonsoft.Json;
using OpenSvip.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

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
            Name = Guid.NewGuid().ToString();
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
                foreach (var item in AudioFiles)
                {
                    try
                    {
                        var audioFileEntry = archive.CreateEntry(Path.Combine(ProjectFileConstantDocument.ProjectFileAudioDirectoryPath, item.Name));
                        using (FileStream audioFileStream = new FileStream(item.FilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            using (Stream stream = audioFileEntry.Open())
                            {
                                audioFileStream.CopyTo(stream);
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