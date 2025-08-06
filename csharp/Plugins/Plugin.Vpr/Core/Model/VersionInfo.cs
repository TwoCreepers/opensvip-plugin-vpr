using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model
{
    public class VersionInfo
    {
        [JsonProperty("major")]
        public int Major { get; set; } = 6;

        [JsonProperty("minor")]
        public int Minor { get; set; } = 1;

        [JsonProperty("revision")]
        public int Revision { get; set; } = 0;
    }
}
