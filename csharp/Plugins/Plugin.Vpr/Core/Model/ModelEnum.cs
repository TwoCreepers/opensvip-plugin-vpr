using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Plugin.Vpr.Core.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ControllerName
    {
        /// <summary>
        /// 动态
        /// </summary>
        dynamics,
        /// <summary>
        /// 音高偏移灵敏度
        /// </summary>
        pitchBendSens,
        /// <summary>
        /// 音高偏移
        /// </summary>
        pitchBend,
        /// <summary>
        /// 气声
        /// </summary>
        breathiness,
        /// <summary>
        /// 明亮度
        /// </summary>
        brightness,
        /// <summary>
        /// 清晰度
        /// </summary>
        clearness,
        /// <summary>
        /// 滑音时间
        /// </summary>
        portamento,
        /// <summary>
        /// 怒音
        /// </summary>
        growl,
        /// <summary>
        /// 激励
        /// </summary>
        exciter,
        /// <summary>
        /// 空气感
        /// </summary>
        air,
        /// <summary>
        /// 个性值
        /// </summary>
        character
    }

    public enum TrackType
    {
        Singing = 0,
        Audio = 1
    }

    public enum LanguageId
    {
        /// <summary>
        /// 中文
        /// </summary>
        zh_ch = 4
    }
}
