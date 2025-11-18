

namespace Fast.Shared;

/// <summary>
/// <see cref="FaTableEnumColumnCtx"/> FastElementPlus FaTable 枚举列上下文
/// </summary>
public class FaTableEnumColumnCtx
{
    /// <summary>
    /// 选项框显示的文字
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 选项框值
    /// </summary>
    /// <remarks>
    /// <para>string</para>
    /// <para>number</para>
    /// <para>boolean</para>
    /// </remarks>
    public object Value { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Show { get; set; }

    /// <summary>
    /// 是否禁用此选项
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// 提示
    /// </summary>
    public string Tips { get; set; }

    /// <summary>
    /// Tag 类型
    /// </summary>
    /// <remarks>
    /// <para>primary</para>
    /// <para>success</para>
    /// <para>info</para>
    /// <para>warning</para>
    /// <para>danger</para>
    /// </remarks>
    public string Type { get; set; }

    /// <summary>
    /// 为树形选择是，可以通过 children 属性指定子选项
    /// </summary>
    public List<FaTableEnumColumnCtx> Children { get; set; }
}