// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.Application.Dto;

/// <summary>
/// 添加应用输入
/// </summary>
public class AddApplicationInput
{
    /// <summary>
    /// 版本
    /// </summary>
    [EnumRequired(ErrorMessage = "版本不能为空", AllowZero = true)]
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [StringRequired(ErrorMessage = "应用名称不能为空")]
    public string AppName { get; set; }

    /// <summary>
    /// Logo URL
    /// </summary>
    [StringRequired(ErrorMessage = "LogoUrl不能为空")]
    public string LogoUrl { get; set; }

    /// <summary>
    /// 主题色
    /// </summary>
    [StringRequired(ErrorMessage = "主题色不能为空")]
    public string ThemeColor { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public long? TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }
}
