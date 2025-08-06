using System.IO;

namespace Plugin.Vpr.Core
{
    public static class ProjectFileConstantDocument
    {
        /// <summary>
        /// 工程文件根目录名
        /// </summary>
        public const string ProjectFileRootDirectoryName = "Project";
        /// <summary>
        /// 工程文件序列Json名
        /// </summary>
        public const string ProjectFileSequenceJsonName = "sequence.json";
        /// <summary>
        /// 工程文件序列Json目录
        /// </summary>
        public static string ProjectFileSequenceJsonPath = Path.Combine(ProjectFileRootDirectoryName, ProjectFileSequenceJsonName);
        /// <summary>
        /// 工程文件音频目录名
        /// </summary>
        public const string ProjectFileAudioDirectoryName = "Audio/";
        /// <summary>
        /// 工程文件音频目录名
        /// </summary>
        public static string ProjectFileAudioDirectoryPath = Path.Combine(ProjectFileRootDirectoryName, ProjectFileAudioDirectoryName);
    }
}
