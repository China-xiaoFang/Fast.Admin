// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.WeChat.Dto;

/// <summary>
/// 换取微信用户身份信息输入
/// </summary>
public class WeChatCode2SessionInput
{
    /// <summary>
    /// 微信Code
    /// </summary>
    [StringRequired(ErrorMessage = "微信Code不能为空")]
    public string Code { get; set; }

    /// <summary>
    /// 加密算法的初始向量
    /// </summary>
    public string IV { get; set; }

    /// <summary>
    /// 包括敏感数据在内的完整用户信息的加密数据
    /// </summary>
    public string EncryptedData { get; set; }
}
