using System.Net;
using System.Net.Sockets;



namespace Fast.Core;

/// <summary>
/// <see cref="MetadataContext"/> 元数据上下文
/// </summary>
[SuppressSniffer]
public class MetadataContext
{
    private static ServerMetadataInfo _metadataInfo;

    /// <summary>
    /// 元数据信息
    /// </summary>
    public static ServerMetadataInfo MetadataInfo
    {
        get
        {
            _metadataInfo ??= GetServerMetadata()
                .Result;

            return _metadataInfo;
        }
    }

    /// <summary>
    /// 获取服务器Ip地址
    /// </summary>
    /// <remarks>从Metadata Service中获取</remarks>
    /// <returns></returns>
    private static async Task<ServerMetadataInfo> GetServerMetadata()
    {
        // 阿里云
        try
        {
            var (instanceName, _) =
                await RemoteRequestUtil.GetAsync("http://100.100.100.200/latest/meta-data/instance-id", timeout: 1);
            var (region, _) =
                await RemoteRequestUtil.GetAsync("http://100.100.100.200/latest/meta-data/region-id", timeout: 1);
            var (zone, _) =
                await RemoteRequestUtil.GetAsync("http://100.100.100.200/latest/meta-data/zone-id", timeout: 1);
            var (innerIp, _) =
                await RemoteRequestUtil.GetAsync("http://100.100.100.200/latest/meta-data/private-ipv4", timeout: 1);
            var (publicIp, _) =
                await RemoteRequestUtil.GetAsync("http://100.100.100.200/latest/meta-data/eipv4", timeout: 1);
            return new ServerMetadataInfo
            {
                Provider = "AliYun",
                InstanceName = instanceName,
                Region = region,
                Zone = zone,
                InnerIp = innerIp,
                PublicIp = publicIp
            };
        }
        catch
        {
            // ignored
        }

        // 腾讯云
        try
        {
            var (instanceName, _) =
                await RemoteRequestUtil.GetAsync("http://metadata.tencentyun.com/latest/meta-data/instance-id",
                    timeout: 1);
            var (region, _) = await RemoteRequestUtil.GetAsync(
                "http://metadata.tencentyun.com/latest/meta-data/placement/region",
                timeout: 1);
            var (zone, _) =
                await RemoteRequestUtil.GetAsync("http://metadata.tencentyun.com/latest/meta-data/placement/zone",
                    timeout: 1);
            var (innerIp, _) =
                await RemoteRequestUtil.GetAsync("http://metadata.tencentyun.com/latest/meta-data/local-ipv4",
                    timeout: 1);
            var (publicIp, _) =
                await RemoteRequestUtil.GetAsync("http://metadata.tencentyun.com/latest/meta-data/public-ipv4",
                    timeout: 1);
            return new ServerMetadataInfo
            {
                Provider = "TencentCloud",
                InstanceName = instanceName,
                Region = region,
                Zone = zone,
                InnerIp = innerIp,
                PublicIp = publicIp
            };
        }
        catch
        {
            // ignored
        }

        // 华为云
        try
        {
            var (instanceName, _) =
                await RemoteRequestUtil.GetAsync("http://169.254.169.254/latest/meta-data/instance-id", timeout: 1);
            var (region, _) =
                await RemoteRequestUtil.GetAsync("http://169.254.169.254/latest/meta-data/region", timeout: 1);
            var (zone, _) =
                await RemoteRequestUtil.GetAsync("http://169.254.169.254/latest/meta-data/availability-zone",
                    timeout: 1);
            var (innerIp, _) =
                await RemoteRequestUtil.GetAsync("http://169.254.169.254/latest/meta-data/local-ipv4", timeout: 1);
            var (publicIp, _) =
                await RemoteRequestUtil.GetAsync("http://169.254.169.254/latest/meta-data/public-ipv4s", timeout: 1);

            return new ServerMetadataInfo
            {
                Provider = "HuaweiCloud",
                InstanceName = instanceName,
                Region = region,
                Zone = zone,
                InnerIp = innerIp,
                PublicIp = publicIp
            };
        }
        catch
        {
            // ignored
        }

        return new ServerMetadataInfo
        {
            Provider = "Local",
            InstanceName = Environment.MachineName,
            Region = "Local",
            Zone = "Local",
            InnerIp = (await Dns.GetHostEntryAsync(Dns.GetHostName())).AddressList
                      .FirstOrDefault(f => f.AddressFamily == AddressFamily.InterNetwork)
                      ?.ToString()
                      ?? "127.0.0.1"
        };
    }
}