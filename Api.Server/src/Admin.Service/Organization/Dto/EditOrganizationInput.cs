// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Service.Organization.Dto;

/// <summary>
/// 编辑机构输入
/// </summary>
public class EditOrganizationInput : UpdateVersionInput
{
    /// <summary>
    /// 机构Id
    /// </summary>
    [LongRequired(ErrorMessage = "机构Id不能为空")]
    public long OrgId { get; set; }

    /// <summary>
    /// 父级Id
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 机构名称
    /// </summary>
    [StringRequired(ErrorMessage = "机构名称不能为空")]
    public string OrgName { get; set; }

    /// <summary>
    /// 机构编码
    /// </summary>
    [StringRequired(ErrorMessage = "机构编码不能为空")]
    public string OrgCode { get; set; }

    /// <summary>
    /// 联系人
    /// </summary>
    public string Contacts { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [IntRequired(ErrorMessage = "排序不能为空")]
    public int Sort { get; set; }

    /// <summary>
    /// 数据公开
    /// </summary>
    [Required(ErrorMessage = "数据公开为空")]
    public bool DataPublic { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}
