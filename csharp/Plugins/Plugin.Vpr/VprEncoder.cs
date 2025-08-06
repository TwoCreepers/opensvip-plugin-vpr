using OpenSvip.Framework;
using OpenSvip.Model;
using Plugin.Vpr.Core.Model;
using System;

namespace Plugin.Vpr
{
    public class VprEncoder
    {
        private readonly ConverterOptions _converterOptions;
        public VprEncoder(ConverterOptions options)
        {
            _converterOptions = options;
        }
        public VprModel Encode(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
