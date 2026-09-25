// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Core;

/// <summary>
/// Excel正则验证特性
/// </summary>
[SuppressSniffer]
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class ExcelRegexAttribute : Attribute
{
    /// <summary>
    /// Excel正则验证特性
    /// </summary>
    public ExcelRegexAttribute(string pattern)
    {
        Pattern = pattern;
    }

    /// <summary>
    /// Excel正则验证特性
    /// </summary>
    public ExcelRegexAttribute(string pattern, string errorMessage)
    {
        Pattern = pattern;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// 正则表达式
    /// </summary>
    public string Pattern { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    /// <remarks>为空时使用默认消息 "{列名} 格式不正确"</remarks>
    public string ErrorMessage { get; set; }
}
