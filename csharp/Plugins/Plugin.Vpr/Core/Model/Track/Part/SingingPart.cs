using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track.Part
{
    /// <summary>
    /// 演唱轨
    /// </summary>
    public class SingingPart : PartBase
    {
        [JsonProperty("styleName")]
        public string StyleName { get; set; } = "No Effect";

        [JsonProperty("stylePresetID")]
        public string StylePresetId { get; set; } = "0c29827a-4289-495d-94d2-e23602d346c6";

        [JsonProperty("voice")]
        public Voice Voice { get; set; } = new Voice();

        [JsonProperty("midiEffects")]
        public List<MidiEffect> MidiEffects { get; set; } = new List<MidiEffect>();

        [JsonProperty("notes")]
        public List<Note.Note> Notes { get; set; } = new List<Note.Note>();

        [JsonProperty("controllers")]
        public List<ControllerInfo> Controllers { get; set; }
    }
}
