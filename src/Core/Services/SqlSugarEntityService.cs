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
using Fast.Common;
using Fast.SqlSugar;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="SqlSugarEntityService"/> SqlSugar实体服务
/// </summary>
public class SqlSugarEntityService : ISqlSugarEntityService, IScopedDependency
{
    /// <summary>
    /// 缓存
    /// </summary>
    private readonly ICache<CenterCCL> _centerCache;

    /// <summary>
    /// 请求上下文
    /// </summary>
    private readonly HttpContext _httpContext;

    /// <summary>
    /// 运行环境
    /// </summary>
    private readonly IHostEnvironment _hostEnvironment;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    public SqlSugarEntityService(ICache<CenterCCL> centerCache, IHttpContextAccessor httpContextAccessor,
        IHostEnvironment hostEnvironment, ILogger<ISqlSugarEntityService> logger)
    {
        _centerCache = centerCache;
        _httpContext = httpContextAccessor.HttpContext;
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
        var cacheKey = CacheConst.GetCacheKey(CacheConst.DatabaseInfo, tenantNo, databaseType.ToString());

        // 优先从 HttpContext.Items 中获取
        var connectionSettingsObj
            = _httpContext?.Items[
                $"{nameof(Fast)}.{nameof(SqlSugar)}.{nameof(ConnectionSettingsOptions)}.{databaseType.ToString()}"];

        if (connectionSettingsObj is ConnectionSettingsOptions connectionSettings)
        {
            return connectionSettings;
        }

        return await _centerCache.GetAndSetAsync(cacheKey,
            async () =>
            {
                var db = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(SqlSugarContext.ConnectionSettings));

                var slaveAdaptConfig = new TypeAdapterConfig();
                slaveAdaptConfig.NewConfig<SlaveDatabaseModel, SlaveConnectionInfo>()
                    .Map(e => e.ServiceIp,
                        e => _hostEnvironment.IsDevelopment()
                            // 开发环境使用公网地址
                            ? e.PublicIp
                            // 生产环境使用内网地址
                            : e.IntranetIp);

                var result = await db.Queryable<MainDatabaseModel>()
                    .Includes(e => e.SlaveDatabaseList.Where(wh => !wh.IsDeleted)
                        .ToList())
                    .Where(wh => !wh.IsDeleted && wh.TenantId == tenantId && wh.DatabaseType == databaseType)
                    .Select(sl => new ConnectionSettingsOptions
                        {
                            ServiceIp = _hostEnvironment.IsDevelopment()
                                // 开发环境使用公网地址
                                ? sl.PublicIp
                                // 生产环境使用内网地址
                                : sl.IntranetIp,
                            SlaveConnectionList = sl.SlaveDatabaseList
                                .Select(dSl => dSl.Adapt<SlaveConnectionInfo>(slaveAdaptConfig))
                                .ToList()
                        },
                        true)
                    .SingleAsync();

                if (result == null)
                {
                    var message = $"未能找到对应类型【{databaseType.ToString()}】所存在的 Database 信息！";
                    _logger.LogError($"TenantId：{tenantId}；TenantNo：{tenantNo}；{message}");
                    throw new ArgumentNullException(message);
                }

                if (_httpContext != null)
                {
                    // 放入 HttpContext.Items 中
                    _httpContext.Items[
                        $"{nameof(Fast)}.{nameof(SqlSugar)}.{nameof(ConnectionSettingsOptions)}.{databaseType.ToString()}"]
                    = result;
                }

                return result;
            });
    }
}