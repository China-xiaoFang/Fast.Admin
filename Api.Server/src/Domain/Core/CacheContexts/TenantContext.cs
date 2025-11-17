using Fast.Center.Entity;
using Microsoft.Extensions.Logging;
using SqlSugar;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="TenantContext"/> 租户上下文
/// </summary>
[SuppressSniffer]
public class TenantContext
{
    /// <summary>
    /// 缓存
    /// </summary>
    internal static ICache<CenterCCL> centerCache = FastContext.GetService<ICache<CenterCCL>>();

    /// <summary>
    /// 日志
    /// </summary>
    internal static ILogger logger = FastContext.GetService<ILogger<TenantContext>>();

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <returns></returns>
    public static TenantModel GetTenantSync(string tenantNo)
    {
        if (string.IsNullOrWhiteSpace(tenantNo))
        {
            throw new UserFriendlyException("租户编号不能为空！");
        }

        // 优先从 HttpContext.Items 中获取
        if (FastContext.HttpContext?.Items.TryGetValue($"{nameof(Fast)}.{nameof(TenantModel.TenantNo)}.{tenantNo}",
                out var obj)
            == true
            && obj is TenantModel tenantModel)
        {
            return tenantModel;
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Tenant, tenantNo);

        tenantModel = centerCache.GetAndSet(cacheKey, () =>
        {
            var repository = FastContext.GetService<ISqlSugarClient>();

            var result = repository.Queryable<TenantModel>()
                .Where(wh => wh.TenantNo == tenantNo)
                .Single();

            if (result == null)
            {
                var message = $"未能找到对应租户【{tenantNo}】信息！";
                logger.LogError($"TenantNo：{tenantNo}；{message}");
                throw new UserFriendlyException(message);
            }

            return result;
        });

        if (FastContext.HttpContext != null)
        {
            // 放入 HttpContext.Items 中
            FastContext.HttpContext.Items[$"{nameof(Fast)}.{nameof(TenantModel.TenantNo)}.{tenantNo}"] = tenantModel;
        }

        return tenantModel;
    }

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <returns></returns>
    public static async Task<TenantModel> GetTenant(string tenantNo)
    {
        if (string.IsNullOrWhiteSpace(tenantNo))
        {
            throw new UserFriendlyException("租户编号不能为空！");
        }

        // 优先从 HttpContext.Items 中获取
        if (FastContext.HttpContext?.Items.TryGetValue($"{nameof(Fast)}.{nameof(TenantModel.TenantNo)}.{tenantNo}",
                out var obj)
            == true
            && obj is TenantModel tenantModel)
        {
            return tenantModel;
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Tenant, tenantNo);

        tenantModel = await centerCache.GetAndSetAsync(cacheKey, async () =>
        {
            var repository = FastContext.GetService<ISqlSugarClient>();

            var result = await repository.Queryable<TenantModel>()
                .Where(wh => wh.TenantNo == tenantNo)
                .SingleAsync();

            if (result == null)
            {
                var message = $"未能找到对应租户【{tenantNo}】信息！";
                logger.LogError($"TenantNo：{tenantNo}；{message}");
                throw new UserFriendlyException(message);
            }

            return result;
        });

        if (FastContext.HttpContext != null)
        {
            // 放入 HttpContext.Items 中
            FastContext.HttpContext.Items[$"{nameof(Fast)}.{nameof(TenantModel.TenantNo)}.{tenantNo}"] = tenantModel;
        }

        return tenantModel;
    }

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <returns></returns>
    public static async Task DeleteTenant(string tenantNo)
    {
        if (string.IsNullOrWhiteSpace(tenantNo))
        {
            throw new UserFriendlyException("租户编号不能为空！");
        }

        if (FastContext.HttpContext != null)
        {
            // 删除 HttpContext.Items 中的
            if (FastContext.HttpContext.Items.ContainsKey($"{nameof(Fast)}.{nameof(TenantModel.TenantNo)}.{tenantNo}"))
            {
                FastContext.HttpContext.Items.Remove($"{nameof(Fast)}.{nameof(TenantModel.TenantNo)}.{tenantNo}");
            }
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Tenant, tenantNo);

        await centerCache.DelAsync(cacheKey);
    }

    /// <summary>
    /// 删除所有租户
    /// </summary>
    /// <returns></returns>
    public static async Task DeleteAllTenant()
    {
        if (FastContext.HttpContext != null)
        {
            // 清空 HttpContext.Items 中的
            var keys = FastContext.HttpContext.Items.Keys.Where(wh =>
                    wh is string key && key.StartsWith($"{nameof(Fast)}.{nameof(TenantModel.TenantNo)}."))
                .ToList();
            foreach (var key in keys)
            {
                FastContext.HttpContext.Items.Remove(key);
            }
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Config, "*");
        await centerCache.DelByPatternAsync(cacheKey);
    }
}