// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.Config.Dto;

/// <summary>
/// 获取配置分页列表输出
/// </summary>
public class QueryConfigPagedOutput : PagedOutput
{
    /// <summary>
    /// 配置Id
    /// </summary>
    public long ConfigId { get; set; }

    /// <summary>
    /// 配置编码
    /// </summary>
    [SugarSearchValue]
    public string ConfigCode { get; set; }

    /// <summary>
    /// 配置名称
    /// </summary>
    [SugarSearchValue]
    public string ConfigName { get; set; }

    /// <summary>
    /// 配置值
    /// </summary>
    public string ConfigValue { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}
