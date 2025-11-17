// ReSharper disable once CheckNamespace

namespace Fast.Shared;

/// <summary>
/// <see cref="EditionEnum"/> 版本枚举
/// </summary>
[Flags]
[FastEnum("版本枚举")]
public enum EditionEnum : byte
{
    /// <summary>
    /// 无
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("无")]
    None = 0,

    /// <summary>
    /// 试用版
    /// </summary>
    [TagType(TagTypeEnum.Warning)] [Description("试用版")]
    Trial = 1,

    /// <summary>
    /// 基础版
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("基础版")]
    Basic = 2,

    /// <summary>
    /// 标准版
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("标准版")]
    Standard = 4,

    /// <summary>
    /// 专业版
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("专业版")]
    Professional = 8,

    /// <summary>
    /// 企业版
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("企业版")]
    Enterprise = 16,

    /// <summary>
    /// 旗舰版
    /// </summary>
    [TagType(TagTypeEnum.Success)] [Description("旗舰版")]
    Flagship = 32,

    /// <summary>
    /// 定制版
    /// </summary>
    [TagType(TagTypeEnum.Success)] [Description("定制版")]
    Custom = 64,

    /// <summary>
    /// 内部版  
    /// </summary>
    /// <remarks>不对外出售</remarks>
    [TagType(TagTypeEnum.Warning)] [Description("内部版")]
    Internal = 128
}