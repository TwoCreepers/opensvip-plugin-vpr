using NUnit.Framework;
using Plugin.Vpr.Core.phoneme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSvip.Tests.PluginVprTest
{
    [TestFixture]
    public class PhonemeTest
    {
        [Test]
        public void TestPhonemeParse()
        {
            Enum.GetValues(typeof(PhonemeLang)).Cast<PhonemeLang>().ToList().ForEach(lang =>
            {
                var phonemeConverter = PhonemeParse.CreateConverter(lang);
                Assert.That(phonemeConverter.PhonemeLang, Is.EqualTo(lang), $"字典名 {Enum.GetName(typeof(PhonemeLang), lang)}.json 与其语言 {Enum.GetName(typeof(PhonemeLang), phonemeConverter.PhonemeLang)} 不匹配");
            });
        }
        [Test]
        public void TestPhonemeZHConverter()
        {
            var phonemeConverter = PhonemeParse.CreateConverter(PhonemeLang.zh_ch);
            Assert.That(phonemeConverter.PhonemeLang, Is.EqualTo(PhonemeLang.zh_ch), "字典名 zh_ch.json 与其语言 zh_ch 不匹配");
            var rightPinYinArray = new List<string>
            {
                "ai", "an", "ang", "ao", "ba", "bai", "ban", "bang", "bao",
                "ni", "hao", "luo", "tian", "yi",
            };
            var errorPinYinArray = new List<string>
            {
                "skdjk", "wgb", "shakjhvk", "zpgf", "is", "jn", "png",
            };
            var extendedPinYinArray = new List<string>
            {
                "yo", "lo",
            };
            var NumArray = new List<string>
            {
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
            };
            rightPinYinArray
                .ForEach(it => Assert.That(phonemeConverter.TryConvert(it, out var value), Is.True, $"正确音标 {it} 应该转换成功"));
            errorPinYinArray
                .ForEach(it => Assert.That(phonemeConverter.TryConvert(it, out var value), Is.False, $"错误音标 {it} 应该转换失败"));
            extendedPinYinArray
                .ForEach(it => Warn.If(phonemeConverter.TryConvert(it, out var value) == false, $"扩展音标 {it} 被转换为默认音素 a"));
            NumArray
                .ForEach(it => Warn.If(phonemeConverter.TryConvert(it, out var value) == false, $"数字 {it} 被转换为默认音素 a")); 
        }
    }
}
