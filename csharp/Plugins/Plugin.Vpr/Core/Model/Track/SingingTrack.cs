using Newtonsoft.Json;
using Plugin.Vpr.Core.Model.Track.Part;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track
{
    public class SingingTrack : TrackBase
    {
        [JsonProperty("lastScrollPositionNoteNumber")]
        public int LastScrollPositionNoteNumber { get; set; } = 68;

        [JsonProperty("parts")]
        public List<SingingPart> Parts { get; set; } = new List<SingingPart>();
    }
}
