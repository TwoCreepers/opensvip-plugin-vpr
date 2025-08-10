using OpenSvip.Framework;
using OpenSvip.Model;
using Plugin.Vpr.Core.Model;

namespace Plugin.Vpr
{
    public class VprConverter : IProjectConverter
    {
        public Project Load(string path, ConverterOptions options)
        {
            var model = VprModel.Read(path, options);
            return new VprDecoder(options).Decode(model, path);
        }

        public void Save(string path, Project project, ConverterOptions options)
        {
            var model = new VprEncoder(options).Encode(project, path);
            model.Write(path, options);
        }
    }
}
