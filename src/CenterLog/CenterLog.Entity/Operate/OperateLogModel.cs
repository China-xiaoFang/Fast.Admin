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

namespace Fast.CenterLog.Entity;

/// <summary>
/// <see cref="OperateLogModel"/> 操作日志Model类
/// </summary>
[SugarTable("OperateLog{year}{month}{day}", "操作日志表")]
[SplitTable(SplitType.Month)]
[SugarDbType(DatabaseTypeEnum.CenterLog)]
public class OperateLogModel : BaseSnowflakeRecordEntity, IBaseTEntity
{
    /// <summary>
    /// 应用Id
    /// </summary>
    [SugarColumn(ColumnDescription = "应用Id")]
    public long AppId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [SugarColumn(ColumnDescription = "应用名称", Length = 20, IsNullable = false)]
    public string AppName { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    [SugarColumn(ColumnDescription = "手机", ColumnDataType = "varchar(11)", IsNullable = false)]
    public string Mobile { get; set; }

    /// <summary>
    /// 是否执行成功
    /// </summary>
    [SugarColumn(ColumnDescription = "是否执行成功")]
    public YesOrNotEnum Success { get; set; }

    /// <summary>
    /// 操作行为
    /// </summary>
    [SugarColumn(ColumnDescription = "操作行为")]
    public HttpRequestActionEnum OperationAction { get; set; }

    /// <summary>
    /// 操作名称
    /// </summary>
    [SugarColumn(ColumnDescription = "操作名称", Length = 100, IsNullable = true)]
    public string OperationName { get; set; }

    /// <summary>
    /// 类名
    /// </summary>
    [SugarColumn(ColumnDescription = "类名", Length = 200, IsNullable = true)]
    public string ClassName { get; set; }

    /// <summary>
    /// 方法名
    /// </summary>
    [SugarColumn(ColumnDescription = "方法名", Length = 200, IsNullable = true)]
    public string MethodName { get; set; }

    /// <summary>
    /// 请求地址
    /// </summary>
    [SugarColumn(ColumnDescription = "请求地址", Length = 500, IsNullable = true)]
    public string Url { get; set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    [SugarColumn(ColumnDescription = "请求方式")]
    public HttpRequestMethodEnum ReqMethod { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    [SugarColumn(ColumnDescription = "请求参数", ColumnDataType = StaticConfig.CodeFirst_BigString, IsNullable = true)]
    public string Param { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    [SugarColumn(ColumnDescription = "返回结果", ColumnDataType = StaticConfig.CodeFirst_BigString, IsNullable = true)]
    public string Result { get; set; }

    /// <summary>
    /// 地址
    /// </summary>
    [SugarColumn(ColumnDescription = "地址", ColumnDataType = StaticConfig.CodeFirst_BigString, IsNullable = true)]
    public string Location { get; set; }

    /// <summary>
    /// 耗时（毫秒）
    /// </summary>
    [SugarColumn(ColumnDescription = "耗时（毫秒）")]
    public long? ElapsedTime { get; set; }

    /// <summary>
    /// 操作时间
    /// </summary>
    [SplitField]
    [SugarColumn(ColumnDescription = "操作时间")]
    public DateTime OperateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", CreateTableFieldSort = 997)]
    public long TenantId { get; set; }
}