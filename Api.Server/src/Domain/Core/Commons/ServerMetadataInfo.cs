

namespace Fast.Core;

/// <summary>
/// <see cref="ServerMetadataInfo"/> 服务器元数据信息
/// </summary>
public class ServerMetadataInfo
{
    /// <summary>
    /// 供应商
    /// </summary>
    public string Provider { get; set; }

    /// <summary>
    /// 实例名称
    /// </summary>
    public string InstanceName { get; set; }

    /// <summary>
    /// 地域
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    /// 可用区
    /// </summary>
    public string Zone { get; set; }

    /// <summary>
    /// 内网Ip
    /// </summary>
    public string InnerIp { get; set; }

    /// <summary>
    /// 公网Ip
    /// </summary>
    public string PublicIp { get; set; }
}