// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Center.Domain;

namespace Fast.Scheduler;

/// <summary>
/// 调度作业标识输入
/// </summary>
public class SchedulerJobKeyInput
{
    /// <summary>
    /// 作业名称
    /// </summary>
    [StringRequired(ErrorMessage = "作业名称不能为空")]
    public string JobName { get; set; }

    /// <summary>
    /// 作业分组
    /// </summary>
    [EnumRequired(ErrorMessage = "作业分组不能为空")]
    public SchedulerJobGroupEnum JobGroup { get; set; }
}
