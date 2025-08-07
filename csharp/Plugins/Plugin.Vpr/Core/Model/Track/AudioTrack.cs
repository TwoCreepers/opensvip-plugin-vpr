using Newtonsoft.Json;
using Plugin.Vpr.Core.Model.Track.Part;
using Plugin.Vpr.Core.Model.Track.Part.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track
{
    public class AudioTrack : TrackBase
    {
        public AudioTrack() : base(TrackType.Audio)
        {
        }

        [JsonProperty("parts")]
        public List<AudioPart> Parts { get; set; } = new List<AudioPart>();
    }
}
