namespace Fast.Deploy.Shared.Enums;

/// <summary>应用类型</summary>
public enum AppType
{
    DotNet = 0,
    Vue = 1
}

/// <summary>部署策略</summary>
public enum DeployStrategy
{
    /// <summary>单机部署</summary>
    Single = 0,

    /// <summary>滚动发布</summary>
    Rolling = 1,

    /// <summary>蓝绿发布（基于 Nginx）</summary>
    BlueGreen = 2
}

/// <summary>部署状态</summary>
public enum DeployStatus
{
    Pending = 0,
    Running = 1,
    Success = 2,
    Failed = 3,
    RollingBack = 4,
    RolledBack = 5
}

/// <summary>节点状态</summary>
public enum NodeStatus
{
    Online = 0,
    Offline = 1
}

/// <summary>操作系统类型</summary>
public enum OsType
{
    Windows = 0,
    Linux = 1
}
