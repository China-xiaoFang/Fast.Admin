// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 公共状态枚举
/// </summary>
[FastEnum("公共状态枚举")]
public enum CommonStatusEnum : byte
{
    /// <summary>
    /// 正常
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("正常")]
    Enable = 1,

    /// <summary>
    /// 禁用
    /// </summary>
    [TagType(TagTypeEnum.Danger)]
    [Description("禁用")]
    Disable = 2
}
