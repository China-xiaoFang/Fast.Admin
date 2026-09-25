// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 区域层级枚举
/// </summary>
[Flags]
[FastEnum("区域层级枚举")]
public enum RegionLevelEnum : byte
{
    /// <summary>
    /// 省/直辖市
    /// </summary>
    [Description("省/直辖市")]
    Province = 1,

    /// <summary>
    /// 地级市
    /// </summary>
    [Description("地级市")]
    City = 2,

    /// <summary>
    /// 区/县/自治县
    /// </summary>
    [Description("区/县/自治县")]
    District = 4,

    /// <summary>
    /// 街道/镇/乡
    /// </summary>
    [Description("街道/镇/乡")]
    Street = 8,

    /// <summary>
    /// 村/社区
    /// </summary>
    [Description("村/社区")]
    Village = 16
}
