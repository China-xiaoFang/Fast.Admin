// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 邮件消息枚举
/// </summary>
[Flags]
[FastEnum("邮件消息枚举")]
public enum MailMessageEnum : byte
{
    /// <summary>
    /// 无
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("无")]
    None = 0,

    /// <summary>
    /// 信息
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("信息")]
    Info = 1,

    /// <summary>
    /// 警告
    /// </summary>
    /// <remarks>只有警告，错误日志才发送邮件</remarks>
    [TagType(TagTypeEnum.Warning)]
    [Description("警告")]
    Warn = 2,

    /// <summary>
    /// 错误
    /// </summary>
    /// <remarks>只有错误日志才发送邮件</remarks>
    [TagType(TagTypeEnum.Danger)]
    [Description("错误")]
    Error = 4
}
