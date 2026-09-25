// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 客户端用户类型枚举
/// </summary>
[Flags]
[FastEnum("客户端用户类型枚举")]
public enum ClientUserTypeEnum
{
    /// <summary>
    /// 手机
    /// </summary>
    [Description("手机")]
    Mobile = 1 << 0,

    /// <summary>
    /// 苹果
    /// </summary>
    [Description("苹果")]
    Apple = 1 << 1,

    /// <summary>
    /// 小程序
    /// </summary>
    [Description("小程序")]
    MiniProgram = 1 << 2,

    /// <summary>
    /// 公众号
    /// </summary>
    [Description("公众号")]
    OfficialAccount = 1 << 3,

    /// <summary>
    /// 服务号
    /// </summary>
    [Description("服务号")]
    ServiceAccount = 1 << 4,

    /// <summary>
    /// 开放平台
    /// </summary>
    [Description("开放平台")]
    OpenPlatform = 1 << 5,

    /// <summary>
    /// 企业微信
    /// </summary>
    [Description("企业微信")]
    WorkWeChat = 1 << 6
}
