using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.MasterTrack
{
    public class MasterTrack
    {
        [JsonProperty("samplingRate")]
        public int SamplingRate { get; set; } = 44100;

        [JsonProperty("loop")]
        public LoopInfo Loop { get; set; } = new LoopInfo();

        [JsonProperty("tempo")]
        public TempoInfo Tempo { get; set; } = new TempoInfo();

        [JsonProperty("timeSig")]
        public TimeSigInfo TimeSignature { get; set; } = new TimeSigInfo();

        [JsonProperty("volume")]
        public VolumeInfo Volume { get; set; } = new VolumeInfo();
    }
}
