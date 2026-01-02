// ------------------------------------------------------------------------
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

using Fast.Center.Entity;
using Fast.SqlSugar;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

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

            var data = await db.Queryable<MainDatabaseModel>()
                .Includes(e => e.SlaveDatabaseList)
                .Where(wh => wh.TenantId == tenantId && wh.DatabaseType == databaseType)
                .SingleAsync();

            if (data == null)
            {
                var message = $"未能找到对应类型【{databaseType.ToString()}】所存在的 Database 信息！";
                _logger.LogError($"TenantId：{tenantId}；TenantNo：{tenantNo}；{message}");
                throw new UserFriendlyException(message);
            }

            return new ConnectionSettingsOptions
            {
                ConnectionId = data.MainId.ToString(),
                DbType = data.DbType.ToDbType(),
                ServiceIp = _hostEnvironment.IsDevelopment()
                    // 开发环境使用公网地址
                    ? data.PublicIp
                    // 生产环境使用内网地址
                    : data.IntranetIp,
                Port = data.Port,
                DbName = data.DbName,
                DbUser = data.DbUser,
                DbPwd = data.DbPwd,
                CustomConnectionStr = data.CustomConnectionStr,
                CommandTimeOut = data.CommandTimeOut,
                SugarSqlExecMaxSeconds = data.SugarSqlExecMaxSeconds,
                DiffLog = data.DiffLog,
                DisableAop = data.DisableAop,
                SlaveConnectionList = data.SlaveDatabaseList.Select(dSl => new SlaveConnectionInfo
                    {
                        ServiceIp = _hostEnvironment.IsDevelopment()
                            // 开发环境使用公网地址
                            ? string.IsNullOrWhiteSpace(dSl.PublicIp) ? data.PublicIp : dSl.PublicIp
                            // 生产环境使用内网地址
                            :
                            string.IsNullOrWhiteSpace(dSl.IntranetIp) ? data.IntranetIp : dSl.IntranetIp,
                        Port = dSl.Port ?? data.Port,
                        DbName = string.IsNullOrWhiteSpace(dSl.DbName) ? data.DbName : dSl.DbName,
                        DbUser = string.IsNullOrWhiteSpace(dSl.DbUser) ? data.DbUser : dSl.DbUser,
                        DbPwd = string.IsNullOrWhiteSpace(dSl.DbPwd) ? data.DbPwd : dSl.DbPwd,
                        CustomConnectionStr = data.CustomConnectionStr,
                        HitRate = dSl.HitRate
                    })
                    .ToList()
            };
        });

        if (FastContext.HttpContext != null)
        {
            // 放入 HttpContext.Items 中
            FastContext.HttpContext.Items[
                $"{nameof(Fast)}.{nameof(SqlSugar)}.{nameof(ConnectionSettingsOptions)}.{databaseType.ToString()}"] = result;
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