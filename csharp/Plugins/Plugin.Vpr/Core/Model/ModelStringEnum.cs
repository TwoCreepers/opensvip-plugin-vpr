using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ControllerName
    {
        dynamics,
        pitchBendSens,
        pitchBend,
        breathiness,
        brightness,
        clearness,
        portamento,
        growl,
        exciter,
        air,
        character
    }

    public enum TrackType
    {
        Singing = 0,
        Audio = 1
    }

    public enum LanguageId
    {
        /// <summary>
        /// 中文
        /// </summary>
        zh_ch = 4
    }
}
