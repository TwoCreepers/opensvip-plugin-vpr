using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track.Part.Note
{
    public class WavInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("originalName")]
        public string OriginalName { get; set; }
    }

    public class RegionInfo
    {
        [JsonProperty("begin")]
        public double Begin { get; set; } = 0;

        [JsonProperty("end")]
        public double End { get; set; }
    }

    public class AudioPart : PartBase
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("wav")]
        public WavInfo Wav { get; set; }

        [JsonProperty("region")]
        public RegionInfo Region { get; set; }
    }
}
