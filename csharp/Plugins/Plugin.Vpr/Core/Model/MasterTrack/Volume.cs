using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.MasterTrack
{
    public class VolumeEvent
    {
        [JsonProperty("pos")]
        public int Position { get; set; } = 0;

        [JsonProperty("value")]
        public int Value { get; set; } = 0;
    }

    public class VolumeInfo
    {
        [JsonProperty("isFolded")]
        public bool IsFolded { get; set; } = false;

        [JsonProperty("height")]
        public double Height { get; set; } = 0.0;

        [JsonProperty("events")]
        public List<VolumeEvent> Events { get; set; } = new List<VolumeEvent>
        {
            new VolumeEvent()
        };
    }
}
