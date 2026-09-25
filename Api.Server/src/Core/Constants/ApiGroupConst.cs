// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Core;

/// <summary>
/// 接口分组常量
/// </summary>
/// <remarks>这里需要和配置文件中的“SwaggerSettings”节点对应</remarks>
[SuppressSniffer]
public static class ApiGroupConst
{
    /// <summary>
    /// 鉴权
    /// </summary>
    public const string Auth = "Auth";

    /// <summary>
    /// 文件
    /// </summary>
    public const string File = "File";

    /// <summary>
    /// 管理后台
    /// </summary>
    public const string Center = "Center";

    /// <summary>
    /// 业务后台
    /// </summary>
    public const string Admin = "Admin";

    /// <summary>
    /// 调度作业
    /// </summary>
    public const string Scheduler = "Scheduler";
}
