using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.MasterTrack
{
    public class LoopInfo
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; } = false;

        [JsonProperty("begin")]
        public int Begin { get; set; } = 0;

        [JsonProperty("end")]
        public int End { get; set; } = 7680;
    }
}
