// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 响应加密特性
/// </summary>
[SuppressSniffer]
[AttributeUsage(AttributeTargets.Method)]
public sealed class ResponseEncipherAttribute : Attribute
{
    /// <summary>
    /// 启用
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// 响应加密特性
    /// </summary>
    public ResponseEncipherAttribute()
    {
        Enable = true;
    }

    /// <summary>
    /// 响应加密特性
    /// </summary>
    public ResponseEncipherAttribute(bool enable)
    {
        Enable = enable;
    }
}
