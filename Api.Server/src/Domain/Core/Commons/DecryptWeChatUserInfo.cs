using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace Fast.Core;

/// <summary>
/// <see cref="DecryptWeChatUserInfo"/> 解密微信用户信息
/// </summary>
/// <remarks>解密数据返回</remarks>
public class DecryptWeChatUserInfo
{
    /// <summary>
    /// 微信昵称
    /// </summary>
    [JsonPropertyName("nickName")]
    [JsonProperty("nickName")]
    public string NickName { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    [JsonPropertyName("gender")]
    [JsonProperty("gender")]
    public GenderEnum Gender { get; set; }

    /// <summary>
    /// 国家
    /// </summary>
    [JsonPropertyName("country")]
    [JsonProperty("country")]

    public string Country { get; set; }

    /// <summary>
    /// 省份
    /// </summary>
    [JsonPropertyName("province")]
    [JsonProperty("province")]
    public string Province { get; set; }

    /// <summary>
    /// 城市
    /// </summary>
    [JsonPropertyName("city")]
    [JsonProperty("city")]
    public string City { get; set; }

    /// <summary>
    /// 语言
    /// </summary>
    [JsonPropertyName("language")]
    [JsonProperty("language")]
    public string Language { get; set; }
}