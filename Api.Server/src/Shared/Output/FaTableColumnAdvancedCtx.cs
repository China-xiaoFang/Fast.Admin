// ReSharper disable once CheckNamespace

namespace Fast.Shared;

/// <summary>
/// <see cref="FaTableColumnAdvancedCtx"/> FastElementPlus FaTable 列高级选项上下文
/// </summary>
public class FaTableColumnAdvancedCtx
{
    /// <summary>
    /// 字段名称
    /// </summary>
    public string Prop { get; set; }

    /// <summary>
    /// 选项类型
    /// </summary>
    public ColumnAdvancedTypeEnum Type { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }
}