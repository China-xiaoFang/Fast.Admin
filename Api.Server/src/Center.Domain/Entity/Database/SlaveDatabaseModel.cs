// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 从数据库表Model类
/// </summary>
[SugarTable("DatabaseSlave", "从数据库表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class SlaveDatabaseModel : BaseTEntity
{
    /// <summary>
    /// 从库Id
    /// </summary>
    [SugarColumn(ColumnDescription = "从库Id", IsPrimaryKey = true)]
    public long SlaveId { get; set; }

    /// <summary>
    /// 主库Id
    /// </summary>
    [SugarColumn(ColumnDescription = "主库Id")]
    public long MainId { get; set; }

    /// <summary>
    /// 公网Ip地址
    /// </summary>
    /// <remarks>为空则使用主库的</remarks>
    [SugarColumn(ColumnDescription = "公网Ip地址", Length = 15)]
    public string PublicIp { get; set; }

    /// <summary>
    /// 内网Ip地址
    /// </summary>
    /// <remarks>为空则使用主库的</remarks>
    [SugarColumn(ColumnDescription = "内网Ip地址", Length = 15)]
    public string IntranetIp { get; set; }

    /// <summary>
    /// 端口号
    /// </summary>
    /// <remarks>为空则使用主库的</remarks>
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
    /// <remarks>为空则使用主库的</remarks>
    [SugarColumn(ColumnDescription = "数据库用户", Length = 20)]
    public string DbUser { get; set; }

    /// <summary>
    /// 数据库密码
    /// </summary>
    /// <remarks>为空则使用主库的</remarks>
    [SugarColumn(ColumnDescription = "数据库密码", Length = 64)]
    public string DbPwd { get; set; }

    /// <summary>
    /// 自定义连接字符串
    /// </summary>
    [SugarColumn(ColumnDescription = "自定义连接字符串", Length = 200)]
    public string CustomConnectionStr { get; set; }

    /// <summary>
    /// 从库命中率
    /// </summary>
    /// <remarks>
    /// <para>为 0 则不命中</para>
    /// <para>建议相加不超过100</para>
    /// </remarks>
    [SugarColumn(ColumnDescription = "从库命中率")]
    public int HitRate { get; set; }
}
