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
/// <see cref="MainDatabaseModel"/> 主数据库表Model类
/// </summary>
[SugarTable("MainDatabase", "主数据库表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class MainDatabaseModel : BaseTEntity
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库类型")]
    public DatabaseTypeEnum DatabaseType { get; set; }

    /// <summary>
    /// 数据库类型，用于区分使用的是那个类型的数据库
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库类型")]
    public DbType DbType { get; set; }

    /// <summary>
    /// 公网Ip地址
    /// </summary>
    [SugarColumn(ColumnDescription = "公网Ip地址", ColumnDataType = "varchar(15)", IsNullable = true)]
    public string PublicIp { get; set; }

    /// <summary>
    /// 内网Ip地址
    /// </summary>
    [SugarColumn(ColumnDescription = "内网Ip地址", ColumnDataType = "varchar(15)", IsNullable = true)]
    public string IntranetIp { get; set; }

    /// <summary>
    /// 端口号
    /// </summary>
    [SugarColumn(ColumnDescription = "端口号")]
    public int? Port { get; set; }

    /// <summary>
    /// 数据库名称
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库名称", Length = 50, IsNullable = false)]
    public string DbName { get; set; }

    /// <summary>
    /// 数据库用户
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库用户", Length = 10, IsNullable = true)]
    public string DbUser { get; set; }

    /// <summary>
    /// 数据库密码
    /// </summary>
    [SugarColumn(ColumnDescription = "数据库密码", Length = 20, IsNullable = true)]
    public string DbPwd { get; set; }

    /// <summary>
    /// 自定义连接字符串
    /// </summary>
    [SugarColumn(ColumnDescription = "自定义连接字符串", Length = 100, IsNullable = true)]
    public string CustomConnectionStr { get; set; }

    /// <summary>
    /// 超时时间，单位秒
    /// </summary>
    [SugarColumn(ColumnDescription = "超时时间，单位秒")]
    public int CommandTimeOut { get; set; }

    /// <summary>
    /// SqlSugar Sql执行最大秒数，如果超过记录警告日志
    /// </summary>
    [SugarColumn(ColumnDescription = "SqlSugar Sql执行最大秒数，如果超过记录警告日志")]
    public double SugarSqlExecMaxSeconds { get; set; }

    /// <summary>
    /// 差异日志
    /// </summary>
    [SugarColumn(ColumnDescription = "差异日志")]
    public bool DiffLog { get; set; }

    /// <summary>
    /// 禁用 SqlSugar 的 Aop
    /// </summary>
    /// <remarks>如果是通过 <see cref="ISqlSugarEntityHandler"/> 进行保存日志到数据库中，必须要将相关 AOP 中涉及到的日志表，单独进行分库设置，并且禁用 AOP，不然会导致死循环的问题。</remarks>
    [SugarColumn(ColumnDescription = "差异日志")]
    public bool DisableAop { get; set; }

    /// <summary>
    /// 从库信息
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(SlaveDatabaseModel.MainId), nameof(Id))]
    public List<SlaveDatabaseModel> SlaveDatabaseList { get; set; }
}