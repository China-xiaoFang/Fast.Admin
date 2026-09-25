// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 调度作业类型枚举
/// </summary>
[Flags]
[FastEnum("调度作业类型枚举")]
public enum SchedulerJobTypeEnum : byte
{
    /// <summary>
    /// 无
    /// </summary>
    [Description("无")]
    None = 0,

    /// <summary>
    /// 本地
    /// </summary>
    [Description("本地")]
    Local = 1,

    /// <summary>
    /// 内网 URL
    /// </summary>
    /// <remarks>自动处理 AccessToken</remarks>
    [Description("内网Url")]
    IntranetUrl = 2,

    /// <summary>
    /// 外网 URL
    /// </summary>
    [Description("外网Url")]
    OuterNetUrl = 4
}
