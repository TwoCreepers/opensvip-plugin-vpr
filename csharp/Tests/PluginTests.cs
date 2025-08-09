using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using OpenSvip.Framework;
using OpenSvip.Stream;
using Plugin.Vpr;

namespace OpenSvip.Tests
{
    [TestFixture]
    public class PluginTests
    {
        [Test]
        public void TestVprSave01()
        {
            if (File.Exists(@"C:\game\worker\yqzhishen\opensvip-data\还是你的笑容最可爱.vpr"))
            {
                File.Delete(@"C:\game\worker\yqzhishen\opensvip-data\还是你的笑容最可爱.vpr");
            }
            var model = new JsonSvipConverter().Load(@"C:\game\worker\yqzhishen\opensvip-data\还是你的笑容最可爱.json", new ConverterOptions(new Dictionary<string, string>()));
            new VprConverter().Save(@"C:\game\worker\yqzhishen\opensvip-data\还是你的笑容最可爱.vpr", model, new ConverterOptions(new Dictionary<string, string>()));
        }
    }
}