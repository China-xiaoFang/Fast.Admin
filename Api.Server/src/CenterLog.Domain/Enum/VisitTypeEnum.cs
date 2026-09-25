// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.CenterLog.Domain;

/// <summary>
/// 访问类型枚举
/// </summary>
[Flags]
[FastEnum("访问类型枚举")]
public enum VisitTypeEnum : byte
{
    /// <summary>
    /// 登录
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("登录")]
    Login = 1,

    /// <summary>
    /// 登出
    /// </summary>
    [TagType(TagTypeEnum.Warning)]
    [Description("登出")]
    Logout = 2,

    /// <summary>
    /// 改密
    /// </summary>
    [TagType(TagTypeEnum.Danger)]
    [Description("改密")]
    ChangePassword = 4,

    /// <summary>
    /// 授权登录
    /// </summary>
    [TagType(TagTypeEnum.Success)]
    [Description("授权登录")]
    AuthorizedLogin = 8
}
