// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Serialization;
using Newtonsoft.Json;

namespace Fast.Center.Service.Database.Dto;

/// <summary>
/// 获取数据库分页列表输出
/// </summary>
public class QueryDatabasePagedOutput : PagedOutput
{
    /// <summary>
    /// 主库Id
    /// </summary>
    public long MainId { get; set; }

    /// <summary>
    /// 数据库类型
    /// </summary>  
    public DatabaseTypeEnum DatabaseType { get; set; }

    /// <summary>
    /// 数据库类型，用于区分使用的是那个类型的数据库
    /// </summary>
    public SugarDbType DbType { get; set; }

    /// <summary>
    /// 公网Ip地址
    /// </summary>
    [JsonConverter(typeof(DataMaskingConverter), DataMaskingTypeEnum.Ip)]
    [SugarSearchValue]
    public string PublicIp { get; set; }

    /// <summary>
    /// 内网Ip地址
    /// </summary>
    public string IntranetIp { get; set; }

    /// <summary>
    /// 端口号
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 数据库名称
    /// </summary>
    [SugarSearchValue]
    public string DbName { get; set; }

    /// <summary>
    /// 数据库用户
    /// </summary>
    public string DbUser { get; set; }

    /// <summary>
    /// 超时时间，单位秒
    /// </summary>
    public int CommandTimeOut { get; set; }

    /// <summary>
    /// SqlSugar SQL 执行最大秒数，如果超过记录警告日志
    /// </summary>
    public int SugarSqlExecMaxSeconds { get; set; }

    /// <summary>
    /// 差异日志
    /// </summary>
    public bool DiffLog { get; set; }

    /// <summary>
    /// 禁用 SqlSugar 的 AOP
    /// </summary>
    public bool DisableAop { get; set; }

    /// <summary>
    /// 是否初始化
    /// </summary>
    public bool IsInitialized { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public long TenantId { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }
}
