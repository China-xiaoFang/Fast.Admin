// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.Menu.Dto;

/// <summary>
/// 编辑菜单按钮输入
/// </summary>
public class EditMenuButtonInput
{
    /// <summary>
    /// 按钮Id
    /// </summary>
    public long? ButtonId { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    [EnumRequired(ErrorMessage = "版本不能为空", AllowZero = true)]
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 按钮编码
    /// </summary>
    [StringRequired(ErrorMessage = "按钮编码不能为空")]
    public string ButtonCode { get; set; }

    /// <summary>
    /// 按钮名称
    /// </summary>
    [StringRequired(ErrorMessage = "按钮名称不能为空")]
    public string ButtonName { get; set; }

    /// <summary>
    /// 角色类型
    /// </summary>
    [EnumRequired(ErrorMessage = "角色类型不能为空", FlagEnum = true, AllowZero = true)]
    public RoleTypeEnum RoleType { get; set; }

    /// <summary>
    /// 是否桌面端
    /// </summary>
    [Required(ErrorMessage = "是否桌面端不能为空")]
    public bool HasDesktop { get; set; }

    /// <summary>
    /// 是否Web端
    /// </summary>
    [Required(ErrorMessage = "是否Web端不能为空")]
    public bool HasWeb { get; set; }

    /// <summary>
    /// 是否移动端
    /// </summary>
    [Required(ErrorMessage = "是否移动端不能为空")]
    public bool HasMobile { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [IntRequired(ErrorMessage = "排序不能为空")]
    public int Sort { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [EnumRequired(ErrorMessage = "状态不能为空")]
    public CommonStatusEnum Status { get; set; }
}
