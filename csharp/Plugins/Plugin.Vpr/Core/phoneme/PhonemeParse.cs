using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.phoneme
{
    /// <summary>
    /// 解析和加载音素表
    /// </summary>
    public class PhonemeParse
    {
        public const string DefaultBaseDictPath = "phoneme_dicts";
        public static PhonemeConverter CreateConverter(PhonemeLang lang, string BaseDictPath = DefaultBaseDictPath)
        {
            var DictPath = Path.Combine(BaseDictPath, Enum.GetName(typeof(PhonemeLang), lang) + ".json");
            var PhonemeModel = phoneme.PhonemeModel.LoadFromFile(DictPath);
            var PhonemeConverter = new PhonemeConverter(PhonemeModel);
            return PhonemeConverter;
        }
        public static PhonemeConverter CreateConverter(string DictPath)
        {
            var PhonemeModel = phoneme.PhonemeModel.LoadFromFile(DictPath);
            var PhonemeConverter = new PhonemeConverter(PhonemeModel);
            return PhonemeConverter;
        }
    }
}
