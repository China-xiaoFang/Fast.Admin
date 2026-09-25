// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Center.Domain;

namespace Fast.Scheduler;

/// <summary>
/// 本地调度作业信息
/// </summary>
[SuppressSniffer]
public class SchedulerLocalJobInfo
{
    /// <summary>
    /// 是否全部租户作业
    /// </summary>
    /// <remarks>如果为 True 则所有调度器都会创建此作业</remarks>
    public bool IsAllTenant { get; set; } = false;

    /// <summary>
    /// 作业名称
    /// </summary>
    public string JobName { get; set; }

    /// <summary>
    /// 作业分组
    /// </summary>
    public SchedulerJobGroupEnum JobGroup { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime BeginTime { get; set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 触发器类型
    /// </summary>
    public TriggerTypeEnum TriggerType { get; set; }

    /// <summary>
    /// Cron表达式
    /// </summary>
    public string Cron { get; set; }

    /// <summary>
    /// 星期（Flag）
    /// </summary>
    public WeekEnum? Week { get; set; }

    /// <summary>
    /// 每天开始时间
    /// </summary>
    public TimeSpan? DailyStartTime { get; set; }

    /// <summary>
    /// 每天结束时间
    /// </summary>
    public TimeSpan? DailyEndTime { get; set; }

    /// <summary>
    /// 执行间隔时间，单位秒（如果有Cron，则IntervalSecond失效）
    /// </summary>
    public int? IntervalSecond { get; set; }

    /// <summary>
    /// 执行次数（默认无限循环）
    /// </summary>
    public int? RunTimes { get; set; }

    /// <summary>
    /// 警告秒数
    /// </summary>
    /// <remarks>优先级最高</remarks>
    public int? WarnTime { get; set; }

    /// <summary>
    /// 重试次数
    /// </summary>
    public int? RetryTimes { get; set; }

    /// <summary>
    /// 重试间隔，单位毫秒
    /// </summary>
    public int? RetryMillisecond { get; set; }

    /// <summary>
    /// 邮件消息
    /// </summary>
    public MailMessageEnum MailMessage { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
}
