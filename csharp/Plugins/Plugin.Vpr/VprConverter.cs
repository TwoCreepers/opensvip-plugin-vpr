using OpenSvip.Framework;
using OpenSvip.Model;
using Plugin.Vpr.Core.Model;

namespace Plugin.Vpr
{
    public class VprConverter : IProjectConverter
    {
        public Project Load(string path, ConverterOptions options)
        {
            var model = VprModel.Read(path);
            return new VprDecoder(options).Decode(model);
        }

        public void Save(string path, Project project, ConverterOptions options)
        {
            var model = new VprEncoder(options).Encode(project);
            model.Write(path);
        }
    }
}
