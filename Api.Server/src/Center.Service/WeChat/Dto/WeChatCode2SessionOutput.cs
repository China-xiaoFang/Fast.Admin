// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.WeChat.Dto;

/// <summary>
/// 换取微信用户身份信息输出
/// </summary>
public class WeChatCode2SessionOutput
{
    /// <summary>
    /// 唯一用户标识
    /// </summary>
    public string OpenId { get; set; }

    /// <summary>
    /// 统一用户标识
    /// </summary>
    public string UnionId { get; set; }

    /// <summary>
    /// 小程序登录凭证
    /// </summary>
    public string SessionKey { get; set; }

    /// <summary>
    /// 微信昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public GenderEnum Sex { get; set; }

    /// <summary>
    /// 国家
    /// </summary>

    public string Country { get; set; }

    /// <summary>
    /// 省份
    /// </summary>

    public string Province { get; set; }

    /// <summary>
    /// 城市
    /// </summary>

    public string City { get; set; }

    /// <summary>
    /// 语言
    /// </summary>

    public string Language { get; set; }
}
