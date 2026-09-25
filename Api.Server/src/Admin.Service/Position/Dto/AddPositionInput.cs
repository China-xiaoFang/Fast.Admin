// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Service.Position.Dto;

/// <summary>
/// 添加职位输入
/// </summary>
public class AddPositionInput
{
    /// <summary>
    /// 职位名称
    /// </summary>
    [Required(ErrorMessage = "职位名称不能为空")]
    public string PositionName { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [IntRequired(ErrorMessage = "排序不能为空")]
    public int Sort { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}
