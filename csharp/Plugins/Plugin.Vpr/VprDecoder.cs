using OpenSvip.Framework;
using OpenSvip.Model;
using Plugin.Vpr.Core.Model;
using System;

namespace Plugin.Vpr
{
    public class VprDecoder
    {
        private readonly ConverterOptions _converterOptions;
        public VprDecoder(ConverterOptions options)
        {
            _converterOptions = options;
        }
        public Project Decode(VprModel model)
        {
            throw new NotImplementedException();
        }
    }
}
