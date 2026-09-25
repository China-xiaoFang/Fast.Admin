// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.SysSerial.Dto;

/// <summary>
/// 编辑系统序号规则输入
/// </summary>
public class EditSysSerialRuleInput : UpdateVersionInput
{
    /// <summary>
    /// 序号规则Id
    /// </summary>
    [LongRequired(ErrorMessage = "序号规则Id不能为空")]
    public long SerialRuleId { get; set; }

    /// <summary>
    /// 前缀
    /// </summary>
    [MaxLength(5, ErrorMessage = "前缀最长为5个字符")]
    public string Prefix { get; set; }

    /// <summary>
    /// 时间类型
    /// </summary>
    [EnumRequired(ErrorMessage = "时间类型不能为空")]
    public SerialDateTypeEnum DateType { get; set; }

    /// <summary>
    /// 分隔符
    /// </summary>
    [EnumRequired(ErrorMessage = "分隔符不能为空", AllowZero = true)]
    public SerialSpacerEnum Spacer { get; set; }

    /// <summary>
    /// 长度
    /// </summary>
    [IntRequired(ErrorMessage = "长度不能为空")]
    public int Length { get; set; }
}
