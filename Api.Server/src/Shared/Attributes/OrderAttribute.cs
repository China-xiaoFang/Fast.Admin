// ReSharper disable once CheckNamespace

namespace Fast.Shared;

/// <summary>
/// <see cref="OrderAttribute"/> 顺序特性
/// </summary>
[SuppressSniffer]
public class OrderAttribute : Attribute
{
    /// <summary>
    /// 顺序
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// <see cref="OrderAttribute"/> 顺序特性
    /// </summary>
    /// <param name="order"><see cref="int"/> 顺序</param>
    public OrderAttribute(int order)
    {
        Order = order;
    }
}