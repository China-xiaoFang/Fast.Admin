// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using System.ComponentModel;

namespace Fast.Center.Service.Login.Dto;

/// <summary>
/// 登录状态枚举
/// </summary>
[FastEnum("登录状态枚举")]
public enum LoginStatusEnum
{
    /// <summary>
    /// 登录成功
    /// </summary>
    [Description("登录成功")]
    Success = 1,

    /// <summary>
    /// 选择租户
    /// </summary>
    [Description("选择租户")]
    SelectTenant = 2,

    /// <summary>
    /// 授权过期
    /// </summary>
    [Description("授权过期")]
    AuthExpired = 4,

    /// <summary>
    /// 无账号
    /// </summary>
    [Description("无账号")]
    NotAccount = 8
}
