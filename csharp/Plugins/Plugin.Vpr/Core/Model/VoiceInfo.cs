using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.MasterTrack
{
    public class VoiceInfo
    {
        [JsonProperty("compID")]
        public string CompId { get; set; } = "BP8CDDH5M7XN2PED";

        [JsonProperty("name")]
        public string Name { get; set; } = "洛天依·萌";
    }
}
