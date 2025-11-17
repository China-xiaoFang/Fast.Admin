using MimeKit;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="IMailService"/> 邮件服务
/// </summary>
public interface IMailService
{
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
    string GetEmailTemplate(string title, string msg, string type = null);

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="string"/> 正文内容</param>
    /// <param name="receiveEmails"><see cref="string"/> 收件人邮箱</param>
    /// <returns></returns>
    Task SendEmail(string title, string content, string receiveEmails);

    /// <summary>
    /// 发送邮件 - 默认收件人
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="string"/> 正文内容</param>
    /// <returns></returns>
    Task SendEmail(string title, string content);

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="BodyBuilder"/> 正文内容</param>
    /// <param name="receiveEmails"><see cref="string"/> 收件人邮箱</param>
    /// <returns></returns>
    Task SendEmail(string title, BodyBuilder content, string receiveEmails);

    /// <summary>
    /// 发送邮件 - 默认收件人
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="BodyBuilder"/> 正文内容</param>
    /// <returns></returns>
    Task SendEmail(string title, BodyBuilder content);

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="string"/> 正文内容</param>
    /// <param name="receiveEmails"><see cref="List{T}"/> 收件人邮箱</param>
    /// <returns></returns>
    Task SendEmail(string title, string content, List<string> receiveEmails);

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="title"><see cref="string"/> 标题</param>
    /// <param name="content"><see cref="BodyBuilder"/> 正文内容</param>
    /// <param name="receiveEmails"><see cref="List{T}"/> 收件人邮箱</param>
    /// <returns></returns>
    Task SendEmail(string title, BodyBuilder content, List<string> receiveEmails);
}