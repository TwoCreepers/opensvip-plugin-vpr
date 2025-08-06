using Newtonsoft.Json;
using Plugin.Vpr.Core.Model.Track.Part;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Vpr.Core.Model.Track
{
    /// <summary>
    /// 音轨类型鉴别器
    /// </summary>
    public class TrackConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TrackBase);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = Newtonsoft.Json.Linq.JObject.Load(reader);
            var type = jObject["type"].ToObject<TrackType>();

            switch (type)
            {
                case TrackType.Singing:
                    return jObject.ToObject<SingingTrack>(serializer);
                case TrackType.Audio:
                    return jObject.ToObject<AudioTrack>(serializer);
                default:
                    throw new JsonSerializationException($"Unknown track type: {type}");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
