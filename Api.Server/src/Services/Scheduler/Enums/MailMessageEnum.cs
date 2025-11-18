

namespace Fast.Scheduler;

/// <summary>
/// <see cref="MailMessageEnum"/> 邮件消息枚举
/// </summary>
[Flags]
[FastEnum("邮件消息枚举")]
public enum MailMessageEnum : byte
{
    /// <summary>
    /// 无
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("无")]
    None = 0,

    /// <summary>
    /// 信息
    /// </summary>
    [TagType(TagTypeEnum.Info)] [Description("信息")]
    Info = 1,

    /// <summary>
    /// 警告
    /// </summary>
    /// <remarks>只有警告，错误日志才发送邮件</remarks>
    [TagType(TagTypeEnum.Warning)] [Description("警告")]
    Warn = 2,

    /// <summary>
    /// 错误
    /// </summary>
    /// <remarks>只有错误日志才发送邮件</remarks>
    [TagType(TagTypeEnum.Danger)] [Description("错误")]
    Error = 4,

    /// <summary>
    /// 警告，错误
    /// </summary>
    WarnAndError = Warn | Error,

    /// <summary>
    /// 全部
    /// </summary>
    All = Info | Warn | Error
}