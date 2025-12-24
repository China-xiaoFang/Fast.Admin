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

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Fast.Core;

/// <summary>
/// <see cref="MailService"/> 邮件服务
/// </summary>
public class MailService : IMailService, ISingletonDependency
{
    /// <summary>
    /// <see cref="MailSettingsOptions"/> 邮件配置
    /// </summary>
    private readonly MailSettingsOptions _mailSettingsOptions;

    /// <summary>
    /// <see cref="ILogger"/> 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"><see cref="MailSettingsOptions"/> 邮件配置</param>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public MailService(IOptions<MailSettingsOptions> options, ILogger<IMailService> logger)
    {
        _mailSettingsOptions = options.Value;
        _logger = logger;
    }

    /// <summary>
    /// 获取公用邮件模板
    /// </summary>
    /// <param name="title"></param>
    /// <param name="msg"></param>
    /// <param name="type">
    /// <para>info</para>
    /// <para>warn</para>
    /// <para>error</para>
    /// </param>
    /// <returns></returns>
    public string GetEmailTemplate(string title, string msg, string type = null)
    {
        return """
               <style>
                 p {
                   padding: 0;
                   margin: 0;
                 }
                 pre {
                   padding: 20px 30px;
                   margin: 20px 10px;
                   white-space: pre-wrap;
                   word-wrap: break-word;
                   color: #909399;
                   background-color: #f2f3f5;
                 }
                 .email_main {
                   display: flex;
                   flex-direction: column;
                   justify-content: start;
                   align-items: center;
                   height: 100%;
                   width: 100%;
                   font-family: "Helvetica", "Arial", sans-serif;
                   font-weight: normal;
                   line-height: 24px;
                   font-size: 14px;
                   margin: 0;
                   padding: 0;
                   color: #303133;
                   background-color: #f2f3f5;
                 }
                 .email_main_warp {
                   width: 100%;
                   max-width: 780px;
                   min-width: 280px;
                   padding: 20px;
                   text-align: center;
                   box-sizing: border-box;
                 }
                 .header {
                   font-size: 32px;
                   font-weight: bold;
                   color: #303133;
                   display: flex;
                   align-items: center;
                   justify-content: center;
                   gap: 10px;
                 }
                 .content {
                   margin: 20px 0;
                   background-color: #ffffff;
                   border: 1px solid #dcdfe6;
                 }
                 .footer a {
                   color: #909399;
                   text-decoration: none !important;
                   letter-spacing: 0.5px;
                 }
                 .logo {
                   height: 80px !important;
                 }
                 .content_header {
                   /* color: #ffffff; */
                   font-size: 18px;
                   font-weight: bold;
                   padding: 20px 0;
                 }
                 .content_header.info {
                   background-color: #303133;
                 }
                 .content_header.warn {
                   background-color: #e6a23c;
                 }
                 .content_header.error {
                   background-color: #f56c6c;
                 }
                 .content_warp {
                   padding: 20px 0;
                   margin: 0 50px;
                   text-align: left;
                   border-bottom: 1px solid #dcdfe6;
                 }
                 .content_warp.warn,
                 .content_warp .warn {
                   color: #e6a23c !important;
                   font-weight: bold !important;
                 }
                 .content_warp.error,
                 .content_warp .error {
                   color: #f56c6c !important;
                   font-weight: bold !important;
                 }
                 .content_footer {
                   padding: 20px 0;
                   color: #909399;
                   font-size: 12px;
                 }
               </style>
               <div class="email_main">
                 <div class="email_main_warp">
                   <div class="header">
                     <img class="logo" src="https://cdn.fastdotnet.com/logo/fast/logo.png" />
                     <span>FastDotNet</span>
                   </div>
                   <div class="content">
                     <div class="content_header {{type}}">{{title}}</div>
                     <div class="content_warp {{type}}">{{msg}}</div>
                     <div class="content_footer">
                       If you have any questions, please contact the administrator. Send time
                       {{DateTime.Now:yyyy-MM-dd HH:mm:ss}}.
                     </div>
                   </div>
                   <div class="footer">
                     <a href="http://fastdotnet.com" target="_blank">
                       Copyright © {{DateTime.Now:yyyy}} FastDotNet All rights reserved.
                     </a>
                   </div>
                 </div>
               </div>

               """;
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="string"/> 正文内容</param>
    /// <param name="receiveEmails"><see cref="string"/> 收件人邮箱</param>
    /// <returns></returns>
    public async Task SendEmail(string title, string content, string receiveEmails)
    {
        await SendEmail(title, new BodyBuilder {HtmlBody = content}, [receiveEmails]);
    }

    /// <summary>
    /// 发送邮件 - 默认收件人
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="string"/> 正文内容</param>
    /// <returns></returns>
    public async Task SendEmail(string title, string content)
    {
        if (_mailSettingsOptions.ReceiveEmails?.Count == 0)
            return;

        await SendEmail(title, new BodyBuilder {HtmlBody = content}, _mailSettingsOptions.ReceiveEmails);
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="BodyBuilder"/> 正文内容</param>
    /// <param name="receiveEmails"><see cref="string"/> 收件人邮箱</param>
    /// <returns></returns>
    public async Task SendEmail(string title, BodyBuilder content, string receiveEmails)
    {
        await SendEmail(title, content, [receiveEmails]);
    }

    /// <summary>
    /// 发送邮件 - 默认收件人
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="BodyBuilder"/> 正文内容</param>
    /// <returns></returns>
    public async Task SendEmail(string title, BodyBuilder content)
    {
        if (_mailSettingsOptions.ReceiveEmails?.Count == 0)
            return;

        await SendEmail(title, content, _mailSettingsOptions.ReceiveEmails);
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="string"/> 正文内容</param>
    /// <param name="receiveEmails"><see cref="List{T}"/> 收件人邮箱</param>
    /// <returns></returns>
    public async Task SendEmail(string title, string content, List<string> receiveEmails)
    {
        await SendEmail(title, new BodyBuilder {HtmlBody = content}, receiveEmails);
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="BodyBuilder"/> 正文内容</param>
    /// <param name="receiveEmails"><see cref="List{T}"/> 收件人邮箱</param>
    /// <returns></returns>
    public async Task SendEmail(string title, BodyBuilder content, List<string> receiveEmails)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_mailSettingsOptions.Smtp))
                throw new UserFriendlyException("发件服务器地址为空！");

            if (_mailSettingsOptions.Port == null || _mailSettingsOptions.Port <= 0)
                throw new ArgumentException("发件服务器端口为空！");

            if (string.IsNullOrWhiteSpace(_mailSettingsOptions.Email))
                throw new ArgumentException("发件邮箱为空！");

            if (string.IsNullOrWhiteSpace(_mailSettingsOptions.AuthCode))
                throw new ArgumentException("发件邮箱授权码为空！");

            // 显示名称
            var displayName = string.IsNullOrWhiteSpace(_mailSettingsOptions.DisplayName) ? "掘慧科技" : _mailSettingsOptions.DisplayName;

            // 创建邮件内容
            var message = new MimeMessage();

            // 发件人
            message.From.Add(new MailboxAddress(displayName, _mailSettingsOptions.Email));

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
            await smtpClient.ConnectAsync(_mailSettingsOptions.Smtp, _mailSettingsOptions.Port.Value,
                SecureSocketOptions.SslOnConnect);
            // 登录邮箱
            await smtpClient.AuthenticateAsync(_mailSettingsOptions.Email, _mailSettingsOptions.AuthCode);
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