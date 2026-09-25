// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Core;

/// <summary>
/// 服务器元数据信息
/// </summary>
public class ServerMetadataInfo
{
    /// <summary>
    /// 供应商
    /// </summary>
    public string Provider { get; set; }

    /// <summary>
    /// 实例名称
    /// </summary>
    public string InstanceName { get; set; }

    /// <summary>
    /// 地域
    /// </summary>
    public string Region { get; set; }

    /// <summary>
    /// 可用区
    /// </summary>
    public string Zone { get; set; }

    /// <summary>
    /// 内网Ip
    /// </summary>
    public string InnerIp { get; set; }

    /// <summary>
    /// 公网Ip
    /// </summary>
    public string PublicIp { get; set; }
}
