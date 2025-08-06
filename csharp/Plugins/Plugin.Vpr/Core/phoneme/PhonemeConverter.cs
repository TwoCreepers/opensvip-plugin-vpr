using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.phoneme
{
    /// <summary>
    /// 主音素转换类
    /// </summary>
    public class PhonemeConverter
    {
        private IDictionary<string, string> _phonemeDictionaries;
        private string _defaultValue;
        
        /// <summary>
        /// 转换器语言
        /// </summary>
        public PhonemeLang PhonemeLang { get; }

        public PhonemeConverter(IDictionary<string, string> phonemeDictionaries, string defaultValue, PhonemeLang phonemeLang)
        {
            _phonemeDictionaries = phonemeDictionaries;
            _defaultValue = defaultValue;
            PhonemeLang = phonemeLang;
        }
        public PhonemeConverter(PhonemeModel model)
        {
            _phonemeDictionaries = model.Dictionary;
            _defaultValue = model.DefaultValue;
            PhonemeLang = model.Lang;
        }
        /// <summary>
        /// 主转换函数，若不在字典中则返回默认音素
        /// </summary>
        /// <param name="key">音标</param>
        /// <returns>音素</returns>
        public string Convert(string key)
        {
            if (string.IsNullOrEmpty(key)) return _defaultValue;
            if (_phonemeDictionaries.TryGetValue(key, out var value)) return value;
            else return _defaultValue;
        }
    }
}
