// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 正则表达式常量
/// </summary>
[SuppressSniffer]
public static class RegexConst
{
    /// <summary>
    /// 账号
    /// </summary>
    /// <remarks>中文、英文、数字包括下划线（6 ~ 20位）</remarks>
    public const string Account = "^[\u4E00-\u9FA5A-Za-z0-9_]{6,20}$";

    /// <summary>
    /// 中文
    /// </summary>
    public const string Chinese = "^[\u4e00-\u9fa5]{0,}$";

    /// <summary>
    /// HTTP 地址判断
    /// </summary>
    public const string HttpUrl = "^(http):\\/\\/([\\w.]+\\/?)\\S*$";

    /// <summary>
    /// Https地址判断
    /// </summary>
    public const string HttpsUrl = "^(https):\\/\\/([\\w.]+\\/?)\\S*$";

    /// <summary>
    /// HTTP 或者Https地址判断
    /// </summary>
    public const string HttpOrHttpsUrl = "^(http|https):\\/\\/([\\w.]+\\/?)\\S*$";

    /// <summary>
    /// 邮箱地址判断
    /// </summary>
    public const string EmailAddress = @"^[A-Za-z0-9._%+-]+@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}$";

    /// <summary>
    /// 手机号码判断
    /// </summary>
    public const string Mobile = @"^1[3-9]\d{9}$";

    /// <summary>
    /// 弱密码（6~18位，仅包含字母和数字）
    /// </summary>
    public const string Password = @"^[A-Z0-9]{6,18}$";

    /// <summary>
    /// 中密码（8~20位，必须包含大小写字母、数字）
    /// </summary>
    public const string MediumPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{8,20}$";

    /// <summary>
    /// 强密码（8~20位，必须包含大小写字母、数字及特殊字符）
    /// </summary>
    public const string StrongPassword = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s])\S{8,20}$";

    /// <summary>
    /// 验证码6位
    /// </summary>
    public const string VerificationCode = "^[0-9]{6}$";

    /// <summary>
    /// 图片验证码4位
    /// </summary>
    public const string ImageCaptchaCode = "^[A-Za-z0-9]{4}$";
}
