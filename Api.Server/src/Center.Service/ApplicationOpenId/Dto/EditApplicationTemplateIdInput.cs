// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Center.Domain;

namespace Fast.Center.Service.ApplicationOpenId.Dto;

/// <summary>
/// 编辑应用模板Id输入
/// </summary>
public class EditApplicationTemplateIdInput
{
    /// <summary>
    /// 记录Id
    /// </summary>
    public long? RecordId { get; set; }

    /// <summary>
    /// 模板类型
    /// </summary>
    [EnumRequired(ErrorMessage = "模板类型不能为空")]
    public ApplicationTemplateTypeEnum TemplateType { get; set; }

    /// <summary>
    /// 模板Id
    /// </summary>
    [StringRequired(ErrorMessage = "模板Id不能为空")]
    public string TemplateId { get; set; }
}
