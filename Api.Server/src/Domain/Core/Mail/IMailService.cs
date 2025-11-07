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