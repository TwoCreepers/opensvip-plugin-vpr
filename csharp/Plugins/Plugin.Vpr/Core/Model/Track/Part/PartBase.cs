using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track.Part
{
    public class PartBase
    {
        [JsonProperty("pos")]
        public int Position { get; set; } = 0;

        [JsonProperty("duration")]
        public int Duration { get; set; }
    }
}
