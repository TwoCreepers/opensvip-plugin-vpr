using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FlutyDeer.Svip3Plugin.Stream;

namespace Plugin.Vpr
{
    public static class Test
    {
        public static void Main()
        {
            var project = new Svip3Converter().Load(@"C:\game\worker\音乐\还是你的笑容最可爱-音阙诗听_泠鸢yousa_王梓钰\还是你的笑容最可爱.svip3", new OpenSvip.Framework.ConverterOptions(new Dictionary<string, string>()));
            new VprConverter().Save(@"C:\game\worker\yqzhishen\opensvip-data\还是你的笑容最可爱.vpr", project, new OpenSvip.Framework.ConverterOptions(new Dictionary<string, string>()));
        }
    }
}
