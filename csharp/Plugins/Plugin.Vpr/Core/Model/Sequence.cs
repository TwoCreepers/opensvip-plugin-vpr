using Newtonsoft.Json;
using Plugin.Vpr.Core.Model.MasterTrack;
using Plugin.Vpr.Core.Model.Track;
using Plugin.Vpr.Core.Model.Track.Part;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model
{
    /// <summary>
    /// 主序列结构
    /// </summary>
    public class Sequence
    {
        [JsonProperty("version")]
        public VersionInfo Version { get; set; } = new VersionInfo();

        [JsonProperty("vender")] // 这可能是 VOCALOID 开发者的拼写错误，但总之我们得遵守
        public string Vendor { get; set; } = "Yamaha Corporation"; // 这种东西为什么要写进工程文件

        [JsonProperty("title")]
        public string Title { get; set; } = "Converted by OpenSvip Plugin Vpr";

        [JsonProperty("masterTrack")]
        public MasterTrack.MasterTrack MasterTrack { get; set; } = new MasterTrack.MasterTrack();

        [JsonProperty("voices")]
        public List<VoiceInfo> Voices { get; set; } = new List<VoiceInfo>();

        [JsonProperty("tracks")]
        [JsonConverter(typeof(TrackConverter))]
        public List<TrackBase> Tracks { get; set; } = new List<TrackBase>();
    }
}
