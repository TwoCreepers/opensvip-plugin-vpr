using NUnit.Framework;
using OpenSvip.Framework;
using OpenSvip.Stream;
using Plugin.Vpr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSvip.Tests.PluginVprTest
{
    [TestFixture]
    public class LoadTest
    {
        [Test]
        public void Test01()
        {
            if (File.Exists(@"C:\game\worker\yqzhishen\opensvip-data\还是你的笑容最可爱.json"))
            {
                File.Delete(@"C:\game\worker\yqzhishen\opensvip-data\还是你的笑容最可爱.json");
            }
            var project = new VprConverter().Load(@"C:\game\worker\yqzhishen\opensvip-data\还是你的笑容最可爱.vpr", new ConverterOptions(new Dictionary<string, string>()));
            new JsonSvipConverter().Save(@"C:\game\worker\yqzhishen\opensvip-data\还是你的笑容最可爱.json", project, new ConverterOptions(new Dictionary<string, string>()));
        }
    }
}
