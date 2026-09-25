// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Center.Domain;

namespace Fast.Center.Service.Complaint.Dto;

/// <summary>
/// 添加投诉输入
/// </summary>
public class AddComplaintInput
{
    /// <summary>
    /// 投诉类型
    /// </summary>
    [EnumRequired(ErrorMessage = "投诉类型不能为空")]
    public ComplaintTypeEnum ComplaintType { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [StringRequired(ErrorMessage = "联系电话不能为空")]
    [RegularExpression(RegexConst.Mobile, ErrorMessage = "联系电话格式不正确")]
    public string ContactPhone { get; set; }

    /// <summary>
    /// 联系邮箱
    /// </summary>
    [RegularExpression(RegexConst.EmailAddress, ErrorMessage = "联系邮箱格式不正确")]
    public string ContactEmail { get; set; }

    /// <summary>
    /// 投诉描述
    /// </summary>
    [StringRequired(ErrorMessage = "投诉描述不能为空")]
    public string Description { get; set; }

    /// <summary>
    /// 附件图片
    /// </summary>
    [SugarColumn(IsJson = true)]
    public List<string> AttachmentImages { get; set; }
}
