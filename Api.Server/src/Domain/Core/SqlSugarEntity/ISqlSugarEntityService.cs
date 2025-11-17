using Fast.SqlSugar;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="ISqlSugarEntityService"/> SqlSugar实体服务
/// </summary>
public interface ISqlSugarEntityService
{
    /// <summary>
    /// 根据类型获取连接字符串
    /// </summary>
    /// <param name="tenantId"><see cref="long"/> 租户Id</param>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <param name="databaseType"><see cref="DatabaseTypeEnum"/> 数据库类型</param>
    /// <returns></returns>
    Task<ConnectionSettingsOptions> GetConnectionSetting(long tenantId, string tenantNo, DatabaseTypeEnum databaseType);

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <param name="databaseType"><see cref="DatabaseTypeEnum"/> 数据库类型</param>
    /// <returns></returns>
    Task DeleteCache(string tenantNo, DatabaseTypeEnum databaseType);

    /// <summary>
    /// 删除所有缓存
    /// </summary>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <returns></returns>
    Task DeleteAllCache(string tenantNo);
}