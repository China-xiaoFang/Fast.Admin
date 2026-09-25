// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 密码类型枚举
/// </summary>
[Flags]
[FastEnum("密码类型枚举")]
public enum PasswordTypeEnum : byte
{
    /// <summary>
    /// MD5
    /// </summary>
    [Description("MD5")]
    MD5 = 1,

    /// <summary>
    /// SHA1
    /// </summary>
    [Description("SHA1")]
    SHA1 = 2,

    /// <summary>
    /// PBKDF2-SHA256
    /// </summary>
    [Description("PBKDF2-SHA256")]
    PBKDF2_SHA256 = 4
}
