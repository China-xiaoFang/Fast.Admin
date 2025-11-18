

namespace Fast.Shared;

/// <summary>
/// <see cref="ElSelectorOutput{T}"/> ElementPlus ElSelect 通用输出
/// </summary>
[SuppressSniffer]
public class ElSelectorOutput<T>
{
    /// <summary>
    /// 值
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// 显示
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 禁用
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// 附加数据
    /// </summary>
    public object Data { get; set; }

    /// <summary>
    /// 子节点
    /// </summary>
    public List<ElSelectorOutput<T>> Children { get; set; }
}