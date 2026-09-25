// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Core;

/// <summary>
/// Excel枚举映射信息
/// </summary>
/// <remarks>
/// 缓存枚举类型的双向映射关系
/// <para>- 导出方向：枚举值 → Description 文本（用于将枚举值转换为可读中文）</para>
/// <para>- 导入方向：文本 → 枚举值（支持按 Description、枚举名称、数值字符串反向解析）</para>
/// </remarks>
internal sealed class ExcelEnumMapping
{
    /// <summary>
    /// 导出映射：枚举值 → 描述文本
    /// </summary>
    public Dictionary<object, string> ValueToDescription { get; set; } = new();

    /// <summary>
    /// 导入映射：文本 → 枚举值（忽略大小写）
    /// </summary>
    /// <remarks>包含 Description 描述、枚举名称、数值字符串三种映射</remarks>
    public Dictionary<string, object> TextToValue { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
