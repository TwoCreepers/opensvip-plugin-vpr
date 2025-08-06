using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track.Part.Note
{
    public class Note
    {
        [JsonProperty("lyric")]
        public string Lyric { get; set; } = null;

        [JsonProperty("phoneme")]
        public string Phoneme { get; set; } = null;

        [JsonProperty("langID")]
        public LanguageId LangId { get; set; } = LanguageId.zh_ch;

        [JsonProperty("isProtected")]
        public bool IsProtected { get; set; } = false;

        [JsonProperty("pos")]
        public int Position { get; set; } = 0;

        [JsonProperty("duration")]
        public int Duration { get; set; } = 120;

        [JsonProperty("number")]
        public int Number { get; set; } = 60;
        
        /// <summary>
        /// 力度
        /// </summary>
        [JsonProperty("velocity")]
        public int Velocity { get; set; } = 64;

        [JsonProperty("exp")]
        public ExpInfo Exp { get; set; } = new ExpInfo();
    }
}
