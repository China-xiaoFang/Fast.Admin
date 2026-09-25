// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 调度作业分组枚举
/// </summary>
[Flags]
[FastEnum("调度作业分组枚举")]
public enum SchedulerJobGroupEnum : byte
{
    /// <summary>
    /// 系统管理
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("系统管理")]
    System = 1,

    /// <summary>
    /// 业务处理
    /// </summary>
    [TagType(TagTypeEnum.Warning)]
    [Description("业务处理")]
    Business = 2,

    /// <summary>
    /// 第三方集成
    /// </summary>
    [TagType(TagTypeEnum.Danger)]
    [Description("第三方集成")]
    ThirdParty = 4,

    /// <summary>
    /// 自定义
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("自定义")]
    Custom = 8
}
