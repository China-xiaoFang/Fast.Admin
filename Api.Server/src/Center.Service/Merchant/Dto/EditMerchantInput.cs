// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.Merchant.Dto;

/// <summary>
/// 编辑商户号输入
/// </summary>
public class EditMerchantInput : UpdateVersionInput
{
    /// <summary>
    /// 商户号Id
    /// </summary>
    [LongRequired(ErrorMessage = "商户号Id不能为空")]
    public long MerchantId { get; set; }

    /// <summary>
    /// 商户号类型
    /// </summary>
    [EnumRequired(ErrorMessage = "商户号类型不能为空")]
    public PaymentChannelEnum MerchantType { get; set; }

    /// <summary>
    /// 商户名称
    /// </summary>
    [StringRequired(ErrorMessage = "商户名称不能为空")]
    public string MerchantName { get; set; }

    /// <summary>
    /// 商户号
    /// </summary>
    [StringRequired(ErrorMessage = "商户号不能为空")]
    public string MerchantNo { get; set; }

    /// <summary>
    /// 商户密钥
    /// </summary>
    public string MerchantSecret { get; set; }

    /// <summary>
    /// 公钥序号
    /// </summary>
    public string PublicSerialNum { get; set; }

    /// <summary>
    /// 公钥
    /// </summary>
    public string PublicKey { get; set; }

    /// <summary>
    /// 证书序号
    /// </summary>
    public string CertSerialNum { get; set; }

    /// <summary>
    /// 证书
    /// </summary>
    public string Cert { get; set; }

    /// <summary>
    /// 证书私钥
    /// </summary>
    public string CertPrivateKey { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}
