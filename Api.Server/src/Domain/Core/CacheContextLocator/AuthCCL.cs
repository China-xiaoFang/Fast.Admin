// ReSharper disable once CheckNamespace

namespace Fast.Core;

/// <summary>
/// <see cref="AuthCCL"/> 授权缓存上下文
/// </summary>
public class AuthCCL : ICacheContextLocator
{
    /// <summary>服务名称</summary>
    public string ServiceName => "Auth";
}