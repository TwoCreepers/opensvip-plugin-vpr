using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track.Part
{
    public class ControllerEvent
    {
        [JsonProperty("pos")]
        public int Position { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }

    public class ControllerInfo
    {
        [JsonProperty("name")]
        public ControllerName Name { get; set; }

        [JsonProperty("events")]
        public List<ControllerEvent> Events { get; set; }
    }
}
