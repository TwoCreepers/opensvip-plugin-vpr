using Newtonsoft.Json;
using Plugin.Vpr.Core.Model.MasterTrack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track.Part
{
    public class TrackBase
    {
        [JsonProperty("type")]
        public TrackType Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("color")]
        public int Color { get; set; } = 0;

        [JsonProperty("busNo")]
        public int BusNo { get; set; } = 0;

        [JsonProperty("isFolded")]
        public bool IsFolded { get; set; } = false;

        [JsonProperty("height")]
        public double Height { get; set; } = 0.0;

        [JsonProperty("volume")]
        public VolumeInfo Volume { get; set; } = new VolumeInfo
        {
            IsFolded = true,
            Height = 40.0
        };

        [JsonProperty("panpot")]
        public VolumeInfo Panpot { get; set; } = new VolumeInfo
        {
            IsFolded = true,
            Height = 40.0
        };

        [JsonProperty("isMuted")]
        public bool IsMuted { get; set; } = false;

        [JsonProperty("isSoloMode")]
        public bool IsSoloMode { get; set; } = false;

        public TrackBase(TrackType trackType)
        {
            Type = trackType;
        }
    }

}
