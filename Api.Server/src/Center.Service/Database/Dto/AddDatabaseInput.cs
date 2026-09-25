// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.Database.Dto;

/// <summary>
/// 添加数据库输入
/// </summary>
public class AddDatabaseInput
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    [EnumRequired(ErrorMessage = "数据库类型不能为空")]
    public DatabaseTypeEnum DatabaseType { get; set; }

    /// <summary>
    /// 数据库类型，用于区分使用的是那个类型的数据库
    /// </summary>
    [EnumRequired(ErrorMessage = "数据库类型不能为空")]
    public SugarDbType DbType { get; set; }

    /// <summary>
    /// 公网Ip地址
    /// </summary>
    [StringRequired(ErrorMessage = "公网Ip地址不能为空")]
    public string PublicIp { get; set; }

    /// <summary>
    /// 内网Ip地址
    /// </summary>
    [StringRequired(ErrorMessage = "内网Ip地址不能为空")]
    public string IntranetIp { get; set; }

    /// <summary>
    /// 端口号
    /// </summary>
    [IntRequired(ErrorMessage = "端口号不能为空")]
    public int Port { get; set; }

    /// <summary>
    /// 数据库名称
    /// </summary>
    [StringRequired(ErrorMessage = "数据库名称不能为空")]
    public string DbName { get; set; }

    /// <summary>
    /// 数据库用户
    /// </summary>
    [StringRequired(ErrorMessage = "数据库用户不能为空")]
    public string DbUser { get; set; }

    /// <summary>
    /// 数据库密码
    /// </summary>
    [StringRequired(ErrorMessage = "数据库密码不能为空")]
    public string DbPwd { get; set; }

    /// <summary>
    /// 自定义连接字符串
    /// </summary>
    public string CustomConnectionStr { get; set; }

    /// <summary>
    /// 超时时间，单位秒
    /// </summary>
    [IntRequired(ErrorMessage = "超时时间不能为空")]
    public int CommandTimeOut { get; set; }

    /// <summary>
    /// SqlSugar SQL 执行最大秒数，如果超过记录警告日志
    /// </summary>
    [IntRequired(ErrorMessage = "SqlSugar Sql执行最大秒数不能为空")]
    public int SugarSqlExecMaxSeconds { get; set; }

    /// <summary>
    /// 差异日志
    /// </summary>
    [Required(ErrorMessage = "差异日志不能为空")]
    public bool DiffLog { get; set; }

    /// <summary>
    /// 禁用 SqlSugar 的 AOP
    /// </summary>
    [Required(ErrorMessage = "禁用 SqlSugar 的 Aop不能为空")]
    public bool DisableAop { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [LongRequired(ErrorMessage = "租户Id不能为空")]
    public long TenantId { get; set; }

    /// <summary>
    /// 是否创建数据库
    /// </summary>
    [Required(ErrorMessage = "是否创建数据库不能为空")]
    public bool IsCreateDatabase { get; set; }
}
