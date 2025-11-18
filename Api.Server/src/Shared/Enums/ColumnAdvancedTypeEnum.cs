

namespace Fast.Shared;

/// <summary>
/// <see cref="ColumnAdvancedTypeEnum"/> 列高级选项类型枚举
/// </summary>
[Flags]
[FastEnum("列高级选项类型枚举")]
public enum ColumnAdvancedTypeEnum : byte
{
    /// <summary>
    /// 字符串
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("字符串")]
    String = 1,

    /// <summary>
    /// 数字
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("数字")]
    Number = 2,

    /// <summary>
    /// Boolean
    /// </summary>
    [TagType(TagTypeEnum.Success)] [Description("Boolean")]
    Boolean = 4,

    /// <summary>
    /// 方法
    /// </summary>
    [TagType(TagTypeEnum.Warning)] [Description("方法")]
    Function = 8
}