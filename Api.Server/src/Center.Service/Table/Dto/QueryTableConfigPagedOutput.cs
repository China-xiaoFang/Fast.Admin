// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// 获取表格配置分页列表输出
/// </summary>
public class QueryTableConfigPagedOutput : PagedOutput
{
    /// <summary>
    /// 表格Id
    /// </summary>
    public long TableId { get; set; }

    /// <summary>
    /// 表格Key
    /// </summary>
    [SugarSearchValue]
    public string TableKey { get; set; }

    /// <summary>
    /// 表格名称
    /// </summary>
    [SugarSearchValue]
    public string TableName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}
