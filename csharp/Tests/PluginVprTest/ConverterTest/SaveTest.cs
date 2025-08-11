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
    public class SaveTest
    {
        [Test]
        public void BatchConvertTest()
        {
            var dataDirectoryPath = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "PluginVprTest",
                "ConverterTest"
            );
            var surceDataDirectoryPath = Path.Combine(dataDirectoryPath, "Source", "OpenSvip");
            var targetDataDirectoryPath = Path.Combine(dataDirectoryPath, "Output", "Vpr");
            var opensvipFiles = Directory.GetFiles(surceDataDirectoryPath, "*.json", SearchOption.AllDirectories);

            var opensvipConverter = new JsonSvipConverter();
            var vprConverter = new VprConverter();
            var options = new ConverterOptions(new Dictionary<string, string>());
            foreach (var item in opensvipFiles)
            {
                var targetFilePath = Path.Combine(targetDataDirectoryPath, Path.ChangeExtension(Path.GetFileName(item), ".vpr"));
                if (!Directory.Exists(Path.GetDirectoryName(targetFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));
                }
                if (File.Exists(targetFilePath))
                {
                    File.Delete(targetFilePath);
                }
                vprConverter.Save(
                    targetFilePath,
                    opensvipConverter.Load(item, options),
                    options);
            }
        }
    }
}
