using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track.Part.Note
{
    public class ExpInfo
    {
        [JsonProperty("accent")]
        public int Accent { get; set; } = 50;

        [JsonProperty("decay")]
        public int Decay { get; set; } = 50;

        [JsonProperty("bendDepth")]
        public int BendDepth { get; set; } = 0;

        [JsonProperty("bendLength")]
        public int BendLength { get; set; } = 0;

        /// <summary>
        /// 开口度
        /// </summary>
        [JsonProperty("opening")]
        public int Opening { get; set; } = 127;
    }
}
