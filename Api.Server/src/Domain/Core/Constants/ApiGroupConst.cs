

namespace Fast.Core;

/// <summary>
/// <see cref="ApiGroupConst"/> 接口分组常量
/// </summary>
/// <remarks>这里需要和配置文件中的“SwaggerSettings”节点对应</remarks>
[SuppressSniffer]
public class ApiGroupConst
{
    /// <summary>
    /// 鉴权
    /// </summary>
    public const string Auth = "Auth";

    /// <summary>
    /// 文件
    /// </summary>
    public const string File = "File";

    /// <summary>
    /// 管理后台
    /// </summary>
    public const string Center = "Center";

    /// <summary>
    /// 业务后台
    /// </summary>
    public const string Admin = "Admin";

    /// <summary>
    /// 调度作业
    /// </summary>
    public const string Scheduler = "Scheduler";
}