// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Microsoft.Extensions.Options;
using Quartz.Spi;
using Quartz.Xml;


// ReSharper disable once CheckNamespace
namespace Quartz;

/// <summary>
/// 使用 <see cref="XMLSchedulingDataProcessor"/> 复用配置项的添加与移除逻辑
/// </summary>
internal sealed class ContainerConfigurationProcessor : XMLSchedulingDataProcessor
{
    private readonly IOptions<QuartzOptions> options;

    public ContainerConfigurationProcessor(ITypeLoadHelper typeLoadHelper, IOptions<QuartzOptions> options) : base(typeLoadHelper)
    {
        this.options = options;
    }

    /// <inheritdoc />
    public override bool OverWriteExistingData => options.Value.Scheduling.OverWriteExistingData;

    /// <inheritdoc />
    public override bool IgnoreDuplicates => options.Value.Scheduling.IgnoreDuplicates;

    /// <inheritdoc />
    public override bool ScheduleTriggerRelativeToReplacedTrigger =>
        options.Value.Scheduling.ScheduleTriggerRelativeToReplacedTrigger;

    /// <inheritdoc />
    protected override IReadOnlyList<IJobDetail> LoadedJobs => options.Value.JobDetails;

    /// <inheritdoc />
    protected override IReadOnlyList<ITrigger> LoadedTriggers => options.Value.Triggers;
}
