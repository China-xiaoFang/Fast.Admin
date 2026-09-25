// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.SqlSugar;

namespace Fast.Core;

/// <summary>
/// SqlSugar 实体服务
/// </summary>
public interface ISqlSugarEntityService
{
    /// <summary>
    /// 根据类型获取连接字符串
    /// </summary>
    /// <returns>指定数据库类型的连接配置</returns>
    Task<ConnectionSettingsOptions> GetConnectionSetting(long tenantId, string tenantNo, DatabaseTypeEnum databaseType);

    /// <summary>
    /// 删除缓存
    /// </summary>
    Task DeleteCache(string tenantNo, DatabaseTypeEnum databaseType);

    /// <summary>
    /// 删除所有缓存
    /// </summary>
    Task DeleteAllCache(string tenantNo);
}
