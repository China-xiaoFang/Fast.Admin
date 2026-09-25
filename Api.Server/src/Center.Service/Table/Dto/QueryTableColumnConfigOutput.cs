// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// 获取表格列配置输出
/// </summary>
public class QueryTableColumnConfigOutput
{
    /// <summary>
    /// 表格Key
    /// </summary>
    public string TableKey { get; set; }

    /// <summary>
    /// 原始列
    /// </summary>
    public List<IDictionary<string, object>> Columns { get; set; }

    /// <summary>
    /// 缓存列
    /// </summary>
    public List<IDictionary<string, object>> CacheColumns { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// 是否存在改变
    /// </summary>
    public bool Change { get; set; }

    /// <summary>
    /// 是否存在缓存
    /// </summary>
    public bool Cache { get; set; }
}
