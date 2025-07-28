// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Fast.Tool.ConsoleApp;

/// <summary>
/// Swagger 文档 JSON 信息
/// </summary>
internal class SwaggerDocInfo
{
    /// <summary>
    /// 接口地址
    /// </summary>
    [JsonPropertyName("paths")]
    public Dictionary<string, SwaggerDocMethod> Paths { get; set; }

    /// <summary>
    /// 组件
    /// </summary>
    [JsonPropertyName("components")]
    public SwaggerDocComponents Components { get; set; }

    /// <summary>
    /// 模块
    /// </summary>
    [JsonPropertyName("tags")]
    public List<SwaggerDocTag> Tags { get; set; }
}

/// <summary>
/// 组件
/// </summary>
internal class SwaggerDocComponents
{
    /// <summary>
    /// 声明
    /// </summary>
    [JsonPropertyName("schemas")]
    public Dictionary<string, SwaggerDocComponentsSchemas> Schemas { get; set; }
}

internal class SwaggerDocComponentsSchemas
{
    /// <summary>
    /// 枚举
    /// </summary>
    [JsonPropertyName("enum")]
    public List<long> Enum { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// 属性
    /// </summary>
    [JsonPropertyName("properties")]
    public Dictionary<string, SwaggerDocComponentsSchemasProperties> Properties { get; set; }
}

/// <summary>
/// 属性
/// </summary>
internal class SwaggerDocComponentsSchemasProperties
{
    /// <summary>
    /// 类型
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// 格式化
    /// </summary>
    [JsonPropertyName("format")]
    public string Format { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// 可为空
    /// </summary>
    [JsonPropertyName("nullable")]
    public bool Nullable { get; set; }

    /// <summary>
    /// 只读
    /// </summary>
    [JsonPropertyName("readOnly")]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// 引用
    /// </summary>
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }

    /// <summary>
    /// 数组项
    /// </summary>
    [JsonPropertyName("items")]
    public SwaggerDocComponentsSchemasProperties Items { get; set; }
}

/// <summary>
/// 接口方法
/// </summary>
internal class SwaggerDocMethod
{
    /// <summary>
    /// Get 请求
    /// </summary>
    [JsonPropertyName("get")]
    public SwaggerDocMethodInfo Get { get; set; }

    /// <summary>
    /// Post 请求
    /// </summary>
    [JsonPropertyName("post")]
    public SwaggerDocMethodInfo Post { get; set; }
}

/// <summary>
/// 请求信息
/// </summary>
internal class SwaggerDocMethodInfo
{
    /// <summary>
    /// 模块
    /// <remarks>这里默认获取第一个</remarks>
    /// </summary>
    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; }

    /// <summary>
    /// 接口描述
    /// </summary>
    [JsonPropertyName("summary")]
    public string Summary { get; set; }

    /// <summary>
    /// Url参数
    /// </summary>
    [JsonPropertyName("parameters")]
    public List<SwaggerDocMethodParameters> Parameters { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    [JsonPropertyName("requestBody")]
    public SwaggerDocMethodRequestBody RequestBody { get; set; }

    /// <summary>
    /// 响应参数
    /// </summary>
    [JsonPropertyName("responses")]
    public SwaggerDocMethodResponses Responses { get; set; }
}

/// <summary>
/// 请求Url参数
/// </summary>
internal class SwaggerDocMethodParameters
{
    /// <summary>
    /// 参数名称
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// 参数声明
    /// </summary>
    [JsonPropertyName("schema")]
    public SwaggerDocMethodParametersSchema Schema { get; set; }
}

/// <summary>
/// 声明
/// </summary>
internal class SwaggerDocMethodParametersSchema
{
    /// <summary>
    /// 参数类型
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// 引用来源
    /// </summary>
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}

/// <summary>
/// 请求参数
/// </summary>
internal class SwaggerDocMethodRequestBody
{
    /// <summary>
    /// 请求参数内容
    /// </summary>
    [JsonPropertyName("content")]
    public SwaggerDocMethodRequestBodyContent Content { get; set; }
}

/// <summary>
/// 请求参数内容
/// </summary>
internal class SwaggerDocMethodRequestBodyContent
{
    [JsonPropertyName("application/json")]
    public SwaggerDocMethodContentJson Json { get; set; }

    [JsonPropertyName("multipart/form-data")]
    public SwaggerDocMethodContentJson FormData { get; set; }
}

/// <summary>
/// 响应
/// </summary>
internal class SwaggerDocMethodResponses
{
    /// <summary>
    /// 200
    /// </summary>
    [JsonPropertyName("200")]
    public SwaggerDocResponsesCode Code200 { get; set; }
}

/// <summary>
/// 响应内容
/// </summary>
internal class SwaggerDocResponsesCode
{
    /// <summary>
    /// 内容
    /// </summary>
    [JsonPropertyName("content")]
    public SwaggerDocResponsesCodeContent Content { get; set; }
}

internal class SwaggerDocResponsesCodeContent
{
    /// <summary>
    /// Json格式
    /// </summary>
    [JsonPropertyName("application/json")]
    public SwaggerDocMethodContentJson Json { get; set; }
}

/// <summary>
/// 内容Json
/// </summary>
internal class SwaggerDocMethodContentJson
{
    /// <summary>
    /// 声明
    /// </summary>
    [JsonPropertyName("schema")]
    public SwaggerDocMethodContentJsonRef Schema { get; set; }
}

internal class SwaggerDocMethodContentJsonRef
{
    /// <summary>
    /// 参数类型
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; }

    /// <summary>
    /// 声明
    /// </summary>
    [JsonPropertyName("items")]
    public SwaggerDocMethodContentJsonRef Items { get; set; }

    /// <summary>
    /// 引用来源
    /// </summary>
    [JsonPropertyName("$ref")]
    public string Ref { get; set; }
}

/// <summary>
/// Tag
/// </summary>
internal class SwaggerDocTag
{
    /// <summary>
    /// 名称
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }
}