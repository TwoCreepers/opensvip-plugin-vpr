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

namespace OpenSvip.Tests.PluginVprTest.ConverterTest
{
    [TestFixture]
    public class LoadTest
    {
        [Test]
        public void BatchConvertTest()
        {
            var dataDirectoryPath = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "PluginVprTest",
                "ConverterTest"
            );
            var surceDataDirectoryPath = Path.Combine(dataDirectoryPath, "Source", "Vpr");
            var targetDataDirectoryPath = Path.Combine(dataDirectoryPath, "Output", "OpenSvip");
            var vprFiles = Directory.GetFiles(surceDataDirectoryPath, "*.vpr", SearchOption.AllDirectories);

            var opensvipConverter = new JsonSvipConverter();
            var vprConverter = new VprConverter();
            var options = new ConverterOptions(new Dictionary<string, string>());
            foreach (var item in vprFiles)
            {
                var targetFilePath = Path.Combine(targetDataDirectoryPath, Path.ChangeExtension(Path.GetFileName(item), ".json"));
                if (!Directory.Exists(Path.GetDirectoryName(targetFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));
                }
                if (File.Exists(targetFilePath))
                {
                    File.Delete(targetFilePath);
                }
                opensvipConverter.Save(
                    targetFilePath,
                    vprConverter.Load(item, options),
                    options);
            }
        }
    }
}
