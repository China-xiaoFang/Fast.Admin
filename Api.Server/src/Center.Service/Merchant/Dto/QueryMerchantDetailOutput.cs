// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.Merchant.Dto;

/// <summary>
/// 获取商户号详情输出
/// </summary>
public class QueryMerchantDetailOutput : PagedOutput
{
    /// <summary>
    /// 商户号Id
    /// </summary>
    public long MerchantId { get; set; }

    /// <summary>
    /// 商户号类型
    /// </summary>
    public PaymentChannelEnum MerchantType { get; set; }

    /// <summary>
    /// 商户名称
    /// </summary>
    public string MerchantName { get; set; }

    /// <summary>
    /// 商户号
    /// </summary>
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
