// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Core;

/// <summary>
/// 验证码服务
/// </summary>
public interface ICaptchaService
{
    /// <summary>
    /// 获取图片验证码
    /// </summary>
    /// <remarks>验证码Key</remarks>
    Task<(string captchaKey, string captchaImage)> GetImageCaptcha();

    /// <summary>
    /// 验证并一次性消费图片验证码
    /// </summary>
    /// <param name="captchaKey">验证码Key</param>
    /// <param name="verificationCode">验证码</param>
    Task VerifyImageCaptcha(string captchaKey, string verificationCode);
}
