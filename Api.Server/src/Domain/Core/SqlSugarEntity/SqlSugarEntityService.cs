using Fast.Center.Entity;
using Fast.SqlSugar;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="SqlSugarEntityService"/> SqlSugar实体服务
/// </summary>
public class SqlSugarEntityService : ISqlSugarEntityService, ISingletonDependency
{
    /// <summary>
    /// 缓存
    /// </summary>
    private readonly ICache<CenterCCL> _centerCache;

    /// <summary>
    /// 运行环境
    /// </summary>
    private readonly IHostEnvironment _hostEnvironment;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// <see cref="SqlSugarEntityService"/> SqlSugar实体服务
    /// </summary>
    /// <param name="centerCache"><see cref="ICache"/> 缓存</param>
    /// <param name="hostEnvironment"><see cref="IHostEnvironment"/> 运行环境</param>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public SqlSugarEntityService(ICache<CenterCCL> centerCache, IHostEnvironment hostEnvironment,
        ILogger<ISqlSugarEntityService> logger)
    {
        _centerCache = centerCache;
        _hostEnvironment = hostEnvironment;
        _logger = logger;
    }

    /// <summary>
    /// 根据类型获取连接字符串
    /// </summary>
    /// <param name="tenantId"><see cref="long"/> 租户Id</param>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <param name="databaseType"><see cref="DatabaseTypeEnum"/> 数据库类型</param>
    /// <returns></returns>
    public async Task<ConnectionSettingsOptions> GetConnectionSetting(long tenantId, string tenantNo,
        DatabaseTypeEnum databaseType)
    {
        if (string.IsNullOrWhiteSpace(tenantNo))
        {
            throw new UserFriendlyException("租户编号不能为空！");
        }

        // 优先从 HttpContext.Items 中获取
        var connectionSettingsObj =
            FastContext.HttpContext?.Items[
                $"{nameof(Fast)}.{nameof(SqlSugar)}.{nameof(ConnectionSettingsOptions)}.{databaseType.ToString()}"];

        if (connectionSettingsObj is ConnectionSettingsOptions connectionSettings)
            return connectionSettings;

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Database, tenantNo, databaseType.ToString());

        var result = await _centerCache.GetAndSetAsync(cacheKey, async () =>
        {
            var db = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(SqlSugarContext.ConnectionSettings));

            var result = await db.Queryable<MainDatabaseModel>()
                .Includes(e => e.SlaveDatabaseList)
                .Where(wh => wh.TenantId == tenantId && wh.DatabaseType == databaseType)
                .Select(sl => new ConnectionSettingsOptions
                {
                    ConnectionId = sl.MainId.ToString(),
                    DbType = sl.DbType,
                    ServiceIp = _hostEnvironment.IsDevelopment()
                        // 开发环境使用公网地址
                        ? sl.PublicIp
                        // 生产环境使用内网地址
                        : sl.IntranetIp,
                    Port = sl.Port,
                    DbName = sl.DbName,
                    DbUser = sl.DbUser,
                    DbPwd = sl.DbPwd,
                    CustomConnectionStr = sl.CustomConnectionStr,
                    CommandTimeOut = sl.CommandTimeOut,
                    SugarSqlExecMaxSeconds = sl.SugarSqlExecMaxSeconds,
                    DiffLog = sl.DiffLog,
                    DisableAop = sl.DisableAop,
                    SlaveConnectionList = sl.SlaveDatabaseList.Select(dSl => new SlaveConnectionInfo
                        {
                            ServiceIp = _hostEnvironment.IsDevelopment()
                                // 开发环境使用公网地址
                                ? string.IsNullOrWhiteSpace(dSl.PublicIp) ? sl.PublicIp : dSl.PublicIp
                                // 生产环境使用内网地址
                                :
                                string.IsNullOrWhiteSpace(dSl.IntranetIp) ? sl.IntranetIp : dSl.IntranetIp,
                            Port = dSl.Port ?? sl.Port,
                            DbName = string.IsNullOrWhiteSpace(dSl.DbName) ? sl.DbName : dSl.DbName,
                            DbUser = string.IsNullOrWhiteSpace(dSl.DbUser) ? sl.DbUser : dSl.DbUser,
                            DbPwd = string.IsNullOrWhiteSpace(dSl.DbPwd) ? sl.DbPwd : dSl.DbPwd,
                            CustomConnectionStr = sl.CustomConnectionStr,
                            HitRate = dSl.HitRate
                        })
                        .ToList()
                })
                .SingleAsync();

            if (result == null)
            {
                var message = $"未能找到对应类型【{databaseType.ToString()}】所存在的 Database 信息！";
                _logger.LogError($"TenantId：{tenantId}；TenantNo：{tenantNo}；{message}");
                throw new UserFriendlyException(message);
            }

            return result;
        });

        if (FastContext.HttpContext != null)
        {
            // 放入 HttpContext.Items 中
            FastContext.HttpContext.Items[
                    $"{nameof(Fast)}.{nameof(SqlSugar)}.{nameof(ConnectionSettingsOptions)}.{databaseType.ToString()}"] =
                result;
        }

        return result;
    }

    /// <summary>
    /// 删除缓存
    /// </summary>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <param name="databaseType"><see cref="DatabaseTypeEnum"/> 数据库类型</param>
    /// <returns></returns>
    public async Task DeleteCache(string tenantNo, DatabaseTypeEnum databaseType)
    {
        if (string.IsNullOrWhiteSpace(tenantNo))
        {
            throw new UserFriendlyException("租户编号不能为空！");
        }

        if (FastContext.HttpContext != null)
        {
            // 删除 HttpContext.Items 中的
            if (FastContext.HttpContext.Items.ContainsKey(
                    $"{nameof(Fast)}.{nameof(SqlSugar)}.{nameof(ConnectionSettingsOptions)}.{databaseType.ToString()}"))
            {
                FastContext.HttpContext.Items.Remove(
                    $"{nameof(Fast)}.{nameof(SqlSugar)}.{nameof(ConnectionSettingsOptions)}.{databaseType.ToString()}");
            }
        }


        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Database, tenantNo, databaseType.ToString());

        await _centerCache.DelAsync(cacheKey);
    }

    /// <summary>
    /// 删除所有缓存
    /// </summary>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <returns></returns>
    public async Task DeleteAllCache(string tenantNo)
    {
        if (string.IsNullOrWhiteSpace(tenantNo))
        {
            throw new UserFriendlyException("租户编号不能为空！");
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Database, tenantNo, "*");
        await _centerCache.DelByPatternAsync(cacheKey);
    }
}