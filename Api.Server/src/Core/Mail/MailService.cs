// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

using System.Net;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Fast.Core;

/// <summary>
/// <see cref="IMailService"/> 默认实现
/// </summary>
public class MailService : IMailService, ISingletonDependency
{
    /// <summary>
    /// 缓存
    /// </summary>
    private readonly ICache<CenterCCL> _centerCache;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// 初始化邮件服务
    /// </summary>
    public MailService(ICache<CenterCCL> centerCache, ILogger<IMailService> logger)
    {
        _centerCache = centerCache;
        _logger = logger;
    }

    /// <inheritdoc />
    public string GetEmailTemplate(string title, string msg, string type = null)
    {
        var (accentColor, badgeBackgroundColor, badgeText) = type?.Trim()
                .ToLowerInvariant() switch
            {
                "warn" => ("#d97706", "#fff7ed", "重要提醒"),
                "error" => ("#dc2626", "#fef2f2", "异常通知"),
                _ => ("#2563eb", "#eff6ff", "系统通知")
            };
        var displayName = ConfigContext.GetConfigSync(ConfigConst.MailDisplayName);
        var encodedTitle = WebUtility.HtmlEncode(title);
        var encodedDisplayName = WebUtility.HtmlEncode(displayName);
        var sendTime = DateTime.Now;

        return $$"""
                 <!doctype html>
                 <html lang="zh-CN">
                 <head>
                 	<meta charset="utf-8" />
                 	<meta name="viewport" content="width=device-width, initial-scale=1" />
                 	<meta name="color-scheme" content="light only" />
                 	<title>{{encodedTitle}}</title>
                 	<style>
                 		body, table, td, p { margin: 0; padding: 0; }
                 		table { border-collapse: collapse; }
                 		.mail-content p { margin: 0 0 14px; }
                 		.mail-content p:last-child { margin-bottom: 0; }
                 		.mail-content pre {
                 			margin: 16px 0 0;
                 			padding: 16px;
                 			overflow-wrap: anywhere;
                 			white-space: pre-wrap;
                 			word-break: break-word;
                 			border: 1px solid #e2e8f0;
                 			border-radius: 8px;
                 			color: #475569;
                 			background-color: #f8fafc;
                 			font:
                 				12px/1.6 Consolas,
                 				Monaco,
                 				monospace;
                 		}
                 		.mail-content .warn { color: #d97706 !important; font-weight: 700 !important; }
                 		.mail-content .error { color: #dc2626 !important; font-weight: 700 !important; }
                 		@media only screen and (max-width: 680px) {
                 			.mail-shell { padding: 24px 12px !important; }
                 			.mail-card-content { padding: 28px 22px !important; }
                 		}
                 	</style>
                 </head>
                 <body style="width: 100%; margin: 0; padding: 0; color: #334155; background-color: #f1f5f9; font-family: -apple-system, BlinkMacSystemFont, &quot;Segoe UI&quot;, Arial, sans-serif;">
                 	<div style="display: none; max-height: 0; overflow: hidden; opacity: 0; color: transparent">
                 		{{encodedTitle}} · {{encodedDisplayName}}
                 	</div>
                 	<table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="width: 100%; margin: 0 auto; background-color: #f1f5f9">
                 		<tr>
                 			<td class="mail-shell" align="center" style="padding: 40px 16px">
                 				<table role="presentation" align="center" width="640" cellpadding="0" cellspacing="0" style="width: 100%; max-width: 640px; margin: 0 auto">
                 					<tr>
                 						<td align="center" style="padding: 0 4px 20px">
                 							<table role="presentation" align="center" cellpadding="0" cellspacing="0" style="margin: 0 auto">
                 								<tr>
                 									<td width="58" height="58" align="center" style="width: 58px; height: 58px">
                 										<img src="https://cdn.fastdotnet.com/logo/fast/logo.png" alt="{{encodedDisplayName}}" width="58" height="58" style="display: block; width: 58px; height: 58px; border: 0; outline: none; text-decoration: none" />
                 									</td>
                 									<td style="padding-left: 12px; color: #0f172a; font-size: 24px; font-weight: 700; letter-spacing: -0.2px">
                 										{{encodedDisplayName}}
                 									</td>
                 								</tr>
                 							</table>
                 						</td>
                 					</tr>
                 					<tr>
                 						<td style="border: 1px solid #e2e8f0; border-radius: 16px;background-color: #ffffff; box-shadow: 0 10px 30px rgba(15, 23, 42, 0.06);overflow: hidden;">
                 							<table role="presentation" width="100%" cellpadding="0" cellspacing="0">
                 								<tr>
                 									<td height="5" style="height: 5px; background-color:{{accentColor}}; font-size:0; line-height:0;">
                 										&nbsp;
                 									</td>
                 								</tr>
                 								<tr>
                 									<td class="mail-card-content" style="padding: 36px 42px 32px">
                 										<table role="presentation" width="100%" cellpadding="0" cellspacing="0">
                 											<tr>
                 												<td>
                 													<span style="display: inline-block; padding: 5px 10px; border-radius: 999px; color: {{accentColor}}; background-color: {{badgeBackgroundColor}}; font-size: 12px; font-weight: 700; line-height: 18px;">
                 														{{badgeText}}
                 													</span>
                 												</td>
                 											</tr>
                 											<tr>
                 												<td style="padding: 16px 0 22px; color: #0f172a; font-size: 24px; font-weight: 750; line-height: 1.4;">
                 													{{encodedTitle}}
                 												</td>
                 											</tr>
                 											<tr>
                 												<td class="mail-content" style="padding-top: 22px; border-top: 1px solid #e2e8f0; color: #475569; font-size: 15px; line-height: 1.75;">
                 													{{msg}}
                 												</td>
                 											</tr>
                 										</table>
                 									</td>
                 								</tr>
                 								<tr>
                 									<td style="padding: 18px 42px; border-top: 1px solid #e2e8f0; color: #94a3b8; background-color: #f8fafc; font-size: 12px; line-height: 1.7; ">
                 										If you have any questions, please contact the administrator.<br />
                 										This email was sent automatically. Please do not reply.<br />
                 										发送时间：{{sendTime:yyyy-MM-dd HH:mm:ss}}
                 									</td>
                 								</tr>
                 							</table>
                 						</td>
                 					</tr>
                 					<tr>
                 						<td align="center" style="padding: 22px 16px 0; color: #94a3b8; font-size: 12px; line-height: 1.7">
                 							<a href="https://fastdotnet.com" target="_blank" style="color: #64748b; text-decoration: none">
                 								{{encodedDisplayName}}
                 							</a>
                 							<br />
                 							Copyright © 2018 ~ {{sendTime:yyyy}} FastDotNet. All rights reserved.
                 						</td>
                 					</tr>
                 				</table>
                 			</td>
                 		</tr>
                 	</table>
                 </body>
                 </html>

                 """;
    }

    /// <summary>
    /// 验证码缓存Dto
    /// </summary>
    private class VerificationCodeCacheDto
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 错误次数
        /// </summary>
        /// <remarks>超过5次自动失效</remarks>
        public int ErrorCount { get; set; }
    }

    /// <inheritdoc />
    public async Task SendVerificationCode(MailTypeEnum mailType, string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, RegexConst.EmailAddress))
        {
            throw new UserFriendlyException("邮箱地址不正确！");
        }

        email = email.Trim()
            .ToLowerInvariant();

        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.Mail, mailType.ToString(), email);
        var dto = await _centerCache.GetAsync<VerificationCodeCacheDto>(cacheKey);
        if (dto != null && dto.SendTime.AddSeconds(60) > DateTime.Now)
        {
            throw new UserFriendlyException("验证码发送过于频繁，请稍后重试！");
        }

        // 生成验证码
        dto ??= new VerificationCodeCacheDto();
        dto.VerificationCode = VerificationUtil.GenNumVerCode();
        dto.SendTime = DateTime.Now;
        dto.ErrorCount = 0;

        var mailTypeDescription = mailType.GetDescription();
        var title = $"【{mailTypeDescription}】邮箱验证码";
        var content = $$"""
                        <p>您好：</p>
                        <p>您正在进行<strong>{{mailTypeDescription}}</strong>操作，请使用以下验证码完成身份校验。</p>
                        <table role="presentation" width="100%" cellpadding="0" cellspacing="0" style="margin: 24px 0;">
                          <tr>
                            <td align="center" style="padding: 22px 16px; border: 1px solid #bfdbfe; border-radius: 10px; color: #1d4ed8; background-color: #eff6ff; font-size: 32px; font-weight: 700; letter-spacing: 10px; line-height: 1;">
                              {{WebUtility.HtmlEncode(dto.VerificationCode)}}
                            </td>
                          </tr>
                        </table>
                        <p style="color: #64748b;">验证码 5 分钟内有效，请勿向任何人泄露。如非本人操作，请忽略此邮件。</p>
                        """;

        await SendEmail(title, GetEmailTemplate(title, content), email);
        await _centerCache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));
    }

    /// <inheritdoc />
    public async Task SendVerificationCode(MailTypeEnum mailType, string email, string verificationCode)
    {
        if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, RegexConst.EmailAddress))
        {
            throw new UserFriendlyException("邮箱地址不正确！");
        }

        email = email.Trim()
            .ToLowerInvariant();

        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.Mail, mailType.ToString(), email);
        var dto = await _centerCache.GetAsync<VerificationCodeCacheDto>(cacheKey);
        if (dto is not {ErrorCount: < 5})
        {
            throw new UserFriendlyException("验证码无效或已过期！");
        }

        if (!string.Equals(dto.VerificationCode, verificationCode, StringComparison.Ordinal))
        {
            dto.ErrorCount++;
            await _centerCache.SetAsync(cacheKey, dto);
            throw new UserFriendlyException("验证码无效或已过期！");
        }

        await _centerCache.DelAsync(cacheKey);
    }

    /// <inheritdoc />
    public async Task SendEmail(string title, string content, string receiveEmails)
    {
        await SendEmail(title, new BodyBuilder {HtmlBody = content}, [receiveEmails]);
    }

    /// <inheritdoc />
    public async Task SendEmail(string title, string content)
    {
        var mailReceiveEmails = await ConfigContext.GetConfig(ConfigConst.MailReceiveEmails);
        var receiveEmails = mailReceiveEmails.ToObject<List<string>>();
        if (receiveEmails is not {Count: > 0})
            return;

        await SendEmail(title, new BodyBuilder {HtmlBody = content}, receiveEmails);
    }

    /// <inheritdoc />
    public async Task SendEmail(string title, BodyBuilder content, string receiveEmails)
    {
        await SendEmail(title, content, [receiveEmails]);
    }

    /// <inheritdoc />
    public async Task SendEmail(string title, BodyBuilder content)
    {
        var mailReceiveEmails = await ConfigContext.GetConfig(ConfigConst.MailReceiveEmails);
        var receiveEmails = mailReceiveEmails.ToObject<List<string>>();
        if (receiveEmails is not {Count: > 0})
            return;

        await SendEmail(title, content, receiveEmails);
    }

    /// <inheritdoc />
    public async Task SendEmail(string title, string content, List<string> receiveEmails)
    {
        await SendEmail(title, new BodyBuilder {HtmlBody = content}, receiveEmails);
    }

    /// <inheritdoc />
    public async Task SendEmail(string title, BodyBuilder content, List<string> receiveEmails)
    {
        try
        {
            var smtp = await ConfigContext.GetConfig(ConfigConst.MailSmtp);
            var portValue = await ConfigContext.GetConfig(ConfigConst.MailPort);
            var email = await ConfigContext.GetConfig(ConfigConst.MailEmail);
            var authCode = await ConfigContext.GetConfig(ConfigConst.MailAuthCode);
            var displayName = await ConfigContext.GetConfig(ConfigConst.MailDisplayName);

            if (string.IsNullOrWhiteSpace(smtp))
                throw new UserFriendlyException("发件服务器地址为空！");

            if (!int.TryParse(portValue, out var port) || port <= 0)
                throw new ArgumentException("发件服务器端口不正确！");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("发件邮箱为空！");

            if (string.IsNullOrWhiteSpace(authCode))
                throw new ArgumentException("发件邮箱授权码为空！");

            // 创建邮件内容
            var message = new MimeMessage();

            // 发件人
            message.From.Add(new MailboxAddress(displayName, email));

            // 收件人
            foreach (var receiveEmail in receiveEmails)
                message.To.Add(new MailboxAddress(null, receiveEmail));

            // 标题
            message.Subject = title;

            // 正文
            message.Body = content.ToMessageBody();

            // 配置 Smtp 客户端
            using var smtpClient = new SmtpClient();
            // 连接发件邮箱服务器
            await smtpClient.ConnectAsync(smtp, port, SecureSocketOptions.SslOnConnect);
            // 登录邮箱
            await smtpClient.AuthenticateAsync(email, authCode);
            // 发送邮件
            await smtpClient.SendAsync(message);
            // 关闭连接
            await smtpClient.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                $"邮件发送失败。\r\nReceiveEmails：{string.Join(",", receiveEmails)}\r\nTitle：{title}\r\nContent：{content}");
            throw;
        }
    }
}