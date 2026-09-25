// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Center.Domain;

namespace Fast.Scheduler;

/// <summary>
/// 本地调度作业日志信息
/// </summary>
[SuppressSniffer]
public class SchedulerJobLocalLogInfo
{
    /// <summary>
    /// 作业名称
    /// </summary>
    public string JobName { get; set; }

    /// <summary>
    /// 信息日志
    /// </summary>
    public Func<string, string, Task> InfoLog { get; set; }

    /// <summary>
    /// 警告日志
    /// </summary>
    public Func<string, string, Task> WarnLog { get; set; }

    /// <summary>
    /// 错误日志
    /// </summary>
    public Func<string, Exception, string, Task> ErrorLog { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 租户编号
    /// </summary>
    public string TenantNo { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    public string TenantCode { get; set; }

    /// <summary>
    /// 机器人信息
    /// </summary>
    public TenantUserModel RobotInfo { get; set; }
}
