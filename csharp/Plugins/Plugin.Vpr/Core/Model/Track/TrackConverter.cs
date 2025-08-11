using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            var jArray = JArray.Load(reader);
            var tracks = new List<TrackBase>();

            foreach (var item in jArray)
            {
                if (item is JObject jObject)
                {
                    var type = jObject["type"].ToObject<TrackType>();
                    switch (type)
                    {
                        case TrackType.Singing:
                            tracks.Add(jObject.ToObject<SingingTrack>(serializer));
                            break;
                        case TrackType.Audio:
                            tracks.Add(jObject.ToObject<AudioTrack>(serializer));
                            break;
                        default:
                            throw new JsonSerializationException($"Unknown track type: {type}");
                    }
                }
            }

            return tracks;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
