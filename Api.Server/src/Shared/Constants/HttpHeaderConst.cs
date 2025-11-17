// ReSharper disable once CheckNamespace

namespace Fast.Shared;

/// <summary>
/// <see cref="HttpHeaderConst"/> Http Header 常量
/// </summary>
[SuppressSniffer]
public class HttpHeaderConst
{
    /// <summary>
    /// 来源
    /// </summary>
    public const string Origin = $"{nameof(Fast)}-Origin";

    /// <summary>
    /// 设备类型
    /// </summary>
    public const string DeviceType = $"{nameof(Fast)}-Device-Type";

    /// <summary>
    /// 设备Id
    /// </summary>
    public const string DeviceId = $"{nameof(Fast)}-Device-Id";
}