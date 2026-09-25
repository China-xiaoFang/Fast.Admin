// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Scheduler;

/// <summary>
/// URL 调度作业日志信息
/// </summary>
[SuppressSniffer]
internal sealed class SchedulerJobUrlLogInfo : SchedulerJobLogInfo
{
    /// <summary>
    /// 请求 URL
    /// </summary>
    public string RequestUrl { get; set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    public string RequestMethod { get; set; }

    /// <summary>
    /// 请求超时时间，单位秒（默认不超时）
    /// </summary>
    public int? RequestTimeout { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    public string RequestParams { get; set; }

    /// <summary>
    /// 请求头部
    /// </summary>
    public string RequestHeader { get; set; }

    /// <summary>
    /// 响应头部
    /// </summary>
    public IDictionary<string, string> ResponseHeader { get; set; }
}
