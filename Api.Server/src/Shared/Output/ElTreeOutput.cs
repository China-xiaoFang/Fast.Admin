// ReSharper disable once CheckNamespace

namespace Fast.Shared;

/// <summary>
/// <see cref="ElTreeOutput{T}"/> ElementPlus ElTree 通用输出
/// </summary>
[SuppressSniffer]
public class ElTreeOutput<T>
{
    /// <summary>
    /// 值
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 是否禁用
    /// </summary>
    public bool? Disabled { get; set; }

    /// <summary>
    /// 是否显示数量
    /// </summary>
    public bool? ShowQuantity { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// 附加数据
    /// </summary>
    public object Data { get; set; }

    /// <summary>
    /// 子节点
    /// </summary>
    public List<ElTreeOutput<T>> Children { get; set; }
}