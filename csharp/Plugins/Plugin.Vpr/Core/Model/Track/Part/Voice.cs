using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track.Part
{
    public class Voice
    {
        [JsonProperty("compID")]
        public string CompId { get; set; } = "BP8CDDH5M7XN2PED";

        [JsonProperty("langID")]
        public LanguageId LangId { get; set; } = LanguageId.zh_ch;
    }
}
