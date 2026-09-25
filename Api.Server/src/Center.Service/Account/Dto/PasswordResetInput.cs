// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.Account.Dto;

/// <summary>
/// 密码重置输入
/// </summary>
public class PasswordResetInput
{
    /// <summary>
    /// 验证Key
    /// </summary>
    [StringRequired(ErrorMessage = "验证Key不能为空")]
    public string VerificationKey { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    [StringRequired(ErrorMessage = "验证码不能为空")]
    [RegularExpression(RegexConst.VerificationCode, ErrorMessage = "验证码必须为6位数字")]
    public string VerificationCode { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    [StringRequired(ErrorMessage = "新密码不能为空")]
    [RegularExpression(RegexConst.MediumPassword, ErrorMessage = "新密码长度必须为8~20位，且必须包含大小写字母、数字")]
    public string NewPassword { get; set; }

    /// <summary>
    /// 确认密码
    /// </summary>
    [StringRequired(ErrorMessage = "确认密码不能为空")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "确认密码长度必须为 8~20 位字符")]
    [Compare(nameof(NewPassword), ErrorMessage = "新密码和确认密码不一致")]
    public string ConfirmPassword { get; set; }
}
