using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.phoneme
{
    /// <summary>
    /// 音素语言
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PhonemeLang
    {
        /// <summary>
        /// 英文(美国)
        /// </summary>
        en_us,
        /// <summary>
        /// 中文(中国)
        /// </summary>
        zh_ch,
    }
    /// <summary>
    /// 音素表模型
    /// </summary>
    public class PhonemeModel
    {
        public PhonemeLang Lang { get; set; }
        public string DefaultValue { get; set; }
        public Dictionary<string, string> Dictionary { get; set; }
        
        public static PhonemeModel LoadFromJson(string json)
        {
            return JsonConvert.DeserializeObject<PhonemeModel>(json);
        }
        public static PhonemeModel LoadFromFile(string path)
        {
            return JsonConvert.DeserializeObject<PhonemeModel>(File.ReadAllText(path));
        }
    }
}
