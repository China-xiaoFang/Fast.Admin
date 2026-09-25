// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Center.Domain;

namespace Fast.Core;

/// <summary>
/// 缓存上下文
/// </summary>
/// <remarks>存放一些启动时可加载的静态变量</remarks>
[SuppressSniffer]
public static class CacheContext
{
    /// <summary>
    /// 接口信息缓存
    /// </summary>
    public static List<ApiInfoModel> ApiInfoList { get; set; }
}
