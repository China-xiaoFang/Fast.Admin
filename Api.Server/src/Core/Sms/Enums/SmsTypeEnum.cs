// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using System.ComponentModel;

namespace Fast.Core;

/// <summary>
/// 短信类型枚举
/// </summary>
[FastEnum("短信类型枚举")]
public enum SmsTypeEnum
{
    /// <summary>
    /// 注册
    /// </summary>
    [Description("注册")]
    Register = 1,

    /// <summary>
    /// 登录
    /// </summary>
    [Description("登录")]
    Login = 2,

    /// <summary>
    /// 校验
    /// </summary>
    [Description("校验")]
    Validity = 4,

    /// <summary>
    /// 修改密码
    /// </summary>
    [Description("修改密码")]
    ChangePassword = 8
}
