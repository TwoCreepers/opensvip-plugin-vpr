using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.MasterTrack
{
    public class TempoGlobal
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; } = false;
        /// <summary>
        /// BPM * 100
        /// </summary>
        [JsonProperty("value")]
        public int Value { get; set; } = 12000;
    }

    public class TempoAra
    {
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; } = true;
    }

    public class TempoEvent
    {
        [JsonProperty("pos")]
        public int Position { get; set; } = 0;
        /// <summary>
        /// BPM * 100
        /// </summary>
        [JsonProperty("value")]
        public int Value { get; set; } = 12000;
    }

    public class TempoInfo
    {
        [JsonProperty("isFolded")]
        public bool IsFolded { get; set; } = false;

        [JsonProperty("height")]
        public double Height { get; set; } = 0.0;

        [JsonProperty("global")]
        public TempoGlobal Global { get; set; } = new TempoGlobal();

        [JsonProperty("ara")]
        public TempoAra Ara { get; set; } = new TempoAra();

        [JsonProperty("events")]
        public List<TempoEvent> Events { get; set; } = new List<TempoEvent>
            {
                new TempoEvent()
            };
    }
}
