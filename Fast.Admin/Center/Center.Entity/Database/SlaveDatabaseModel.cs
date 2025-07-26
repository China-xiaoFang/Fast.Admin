﻿// ------------------------------------------------------------------------
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

// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="SlaveDatabaseModel"/> 从数据库表Model类
/// </summary>
[SugarTable("SlaveDatabase", "从数据库表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class SlaveDatabaseModel : BaseTEntity, IUpdateVersion
{
    /// <summary>
    /// 主库Id
    /// </summary>
    [SugarColumn(ColumnDescription = "主库Id")]
    public long MainId { get; set; }

    /// <summary>
    /// 数据库类型，用于区分使用的是那个类型的数据库
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库类型")]
    public DbType DbType { get; set; }

    /// <summary>
    /// 公网Ip地址
    /// </summary>
    [SugarColumn(ColumnDescription = "公网Ip地址", ColumnDataType = "varchar(15)")]
    public string PublicIp { get; set; }

    /// <summary>
    /// 内网Ip地址
    /// </summary>
    [SugarColumn(ColumnDescription = "内网Ip地址", ColumnDataType = "varchar(15)")]
    public string IntranetIp { get; set; }

    /// <summary>
    /// 端口号
    /// </summary>
    [SugarColumn(ColumnDescription = "端口号")]
    public int? Port { get; set; }

    /// <summary>
    /// 数据库名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "数据库名称", Length = 50)]
    public string DbName { get; set; }

    /// <summary>
    /// 数据库用户
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库用户", Length = 10)]
    public string DbUser { get; set; }

    /// <summary>
    /// 数据库密码
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库密码", Length = 20)]
    public string DbPwd { get; set; }

    /// <summary>
    /// 自定义连接字符串
    /// </summary>
    [SugarColumn(ColumnDescription = "自定义连接字符串", Length = 100)]
    public string CustomConnectionStr { get; set; }

    /// <summary>
    /// 从库命中率
    /// </summary>
    /// <remarks>
    /// <para>为 0 则不命中</para>
    /// <para>建议相加不超过10</para>
    /// </remarks>
    [SugarColumn(ColumnDescription = "从库命中率")]
    public int HitRate { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    [SugarColumn(ColumnDescription = "更新版本控制字段", IsEnableUpdateVersionValidation = true, CreateTableFieldSort = 998)]
    public virtual long RowVersion { get; set; }
}