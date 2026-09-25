// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Core;

/// <summary>
/// API 限流配置选项
/// </summary>
public class ApiRateLimitSettingsOptions : IPostConfigure
{
    /// <summary>
    /// 登录限流，单个Ip与设备Id组合在统计窗口内允许的请求数
    /// </summary>
    public int? LoginPermitLimit { get; set; }

    /// <summary>
    /// 登录限流，单个Ip在统计窗口内允许的请求数
    /// </summary>
    public int? LoginIpPermitLimit { get; set; }

    /// <summary>
    /// 已登录限流，单个会话在统计窗口内允许的请求数；未登录时按单个Ip与设备Id组合统计
    /// </summary>
    public int? PermitLimit { get; set; }

    /// <summary>
    /// 单个Ip在统计窗口内允许的请求数
    /// </summary>
    public int? IpPermitLimit { get; set; }

    /// <summary>
    /// 统计窗口秒数
    /// </summary>
    public int? WindowSeconds { get; set; }

    /// <inheritdoc />
    public void PostConfigure()
    {
        LoginPermitLimit ??= 10;
        LoginIpPermitLimit ??= 30;
        PermitLimit ??= 120;
        IpPermitLimit ??= 60;
        WindowSeconds ??= 60;
    }
}
