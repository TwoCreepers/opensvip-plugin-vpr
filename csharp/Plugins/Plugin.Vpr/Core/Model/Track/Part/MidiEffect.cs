using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track.Part
{
    public class Parameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }
    }

    public class MidiEffect
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("isBypassed")]
        public bool IsBypassed { get; set; }

        [JsonProperty("isFolded")]
        public bool IsFolded { get; set; }

        [JsonProperty("parameters")]
        public List<Parameter> Parameters { get; set; }
    }
}
