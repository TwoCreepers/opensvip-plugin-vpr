using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.MasterTrack
{
    public class TimeSigEvent
    {
        [JsonProperty("bar")]
        public int Bar { get; set; } = 0;

        [JsonProperty("numer")]
        public int Numerator { get; set; } = 4;

        [JsonProperty("denom")]
        public int Denominator { get; set; } = 4;
    }

    public class TimeSigInfo
    {
        [JsonProperty("isFolded")]
        public bool IsFolded { get; set; } = false;

        [JsonProperty("events")]
        public List<TimeSigEvent> Events { get; set; }
    }
}
