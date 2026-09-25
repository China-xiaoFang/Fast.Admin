// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 消息发送记录表Model类
/// </summary>
[SugarTable("MessageSendRecord", "消息发送记录表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(CreatedTime)}", nameof(CreatedTime), OrderByType.Desc)]
public class MessageSendRecordModel : BaseRecordEntity
{
    /// <summary>
    /// 记录Id
    /// </summary>
    [SugarColumn(ColumnDescription = "记录Id", IsPrimaryKey = true, IsIdentity = true)]
    public long RecordId { get; set; }

    /// <summary>
    /// 渠道
    /// </summary>
    [SugarColumn(ColumnDescription = "渠道")]
    public MessageSendChannelEnum Channel { get; set; }

    /// <summary>
    /// 收件人
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "收件人", Length = 50)]
    public string Receiver { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [SugarColumn(ColumnDescription = "标题", Length = 50)]
    public string Title { get; set; }

    /// <summary>
    /// 记录值
    /// </summary>
    [SugarColumn(ColumnDescription = "记录值", ColumnDataType = StaticConfig.CodeFirst_BigString)]
    public string RecordValue { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    [SugarColumn(ColumnDescription = "是否成功")]
    public bool IsSuccess { get; set; }
}
