

namespace Fast.Shared;

/// <summary>
/// <see cref="DatabaseTypeEnum"/> 数据库类型枚举
/// </summary>
[Flags]
[FastEnum("数据库类型枚举")]
public enum DatabaseTypeEnum
{
    /// <summary>
    /// 系统核心库
    /// </summary>
    [Description("系统核心库")] Center = 1,

    /// <summary>
    /// 系统核心日志库
    /// </summary>
    [Description("系统核心日志库")] CenterLog = 2,

    /// <summary>
    /// 系统业务库
    /// </summary>
    [Description("系统业务库")] Admin = 4,

    /// <summary>
    /// 系统业务日志库
    /// </summary>
    [Description("系统业务日志库")] AdminLog = 8,

    /// <summary>
    /// 网关系统库
    /// </summary>
    [Description("网关系统库")] Gateway = 16,

    /// <summary>
    /// 部署系统库
    /// </summary>
    [Description("部署系统库")] Deploy = 32
}