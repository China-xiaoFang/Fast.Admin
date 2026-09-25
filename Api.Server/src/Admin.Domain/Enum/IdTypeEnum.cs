// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Domain;

/// <summary>
/// 证件类型枚举
/// </summary>
[Flags]
[FastEnum("证件类型枚举")]
public enum IdTypeEnum : byte
{
    /// <summary>
    /// 身份证
    /// </summary>
    [Description("身份证")]
    IdCard = 1,

    /// <summary>
    /// 护照
    /// </summary>
    [Description("护照")]
    Passport = 2,

    /// <summary>
    /// 军官证
    /// </summary>
    [Description("军官证")]
    MilitaryId = 4,

    /// <summary>
    /// 驾驶证
    /// </summary>
    [Description("驾驶证")]
    DriverLicense = 8,

    /// <summary>
    /// 社保卡
    /// </summary>
    [Description("社保卡")]
    SocialSecurityCard = 16,

    /// <summary>
    /// 港澳居民来往内地通行证
    /// </summary>
    [Description("港澳居民来往内地通行证")]
    MainlandTravelPermitForHKAndMacao = 32,

    /// <summary>
    /// 台湾居民来往大陆通行证
    /// </summary>
    [Description("台湾居民来往大陆通行证")]
    MainlandTravelPermitForTaiwan = 64,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")]
    Other = 128
}
