// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// ElementPlus ElSelect 通用输出
/// </summary>
[SuppressSniffer]
public class ElSelectorOutput<T>
{
    /// <summary>
    /// 值
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// 显示
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 禁用
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 附加数据
    /// </summary>
    public object Data { get; set; }

    /// <summary>
    /// 子节点
    /// </summary>
    public List<ElSelectorOutput<T>> Children { get; set; }
}
