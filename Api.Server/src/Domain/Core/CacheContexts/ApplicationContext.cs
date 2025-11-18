using Fast.Center.Entity;
using Microsoft.Extensions.Logging;
using SqlSugar;


namespace Fast.Core;

/// <summary>
/// <see cref="ApplicationContext"/> 应用上下文
/// </summary>
[SuppressSniffer]
public class ApplicationContext
{
    /// <summary>
    /// 缓存
    /// </summary>
    internal static ICache<CenterCCL> centerCache = FastContext.GetService<ICache<CenterCCL>>();

    /// <summary>
    /// 日志
    /// </summary>
    internal static ILogger logger = FastContext.GetService<ILogger<ApplicationContext>>();

    /// <summary>
    /// 获取应用
    /// </summary>
    /// <param name="openId"><see cref="string"/> 应用标识</param>
    /// <returns></returns>
    public static ApplicationOpenIdModel GetApplicationSync(string openId)
    {
        if (string.IsNullOrWhiteSpace(openId))
        {
            throw new UserFriendlyException("应用标识不能为空！");
        }

        // 优先从 HttpContext.Items 中获取
        if (FastContext.HttpContext?.Items.TryGetValue(
                $"{nameof(Fast)}.{nameof(ApplicationOpenIdModel.OpenId)}.{openId}",
                out var obj)
            == true
            && obj is ApplicationOpenIdModel applicationOpenIdModel)
        {
            return applicationOpenIdModel;
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.App, openId);

        applicationOpenIdModel = centerCache.GetAndSet(cacheKey, () =>
        {
            var repository = FastContext.GetService<ISqlSugarClient>();

            var result = repository.Queryable<ApplicationOpenIdModel>()
                .Includes(e => e.Application)
                .Where(wh => wh.OpenId == openId)
                .Single();

            if (result == null)
            {
                var message = $"未能找到对应应用【{openId}】信息！";
                logger.LogError($"OpenId：{openId}；{message}");
                throw new UserFriendlyException(message);
            }

            return result;
        });

        if (FastContext.HttpContext != null)
        {
            // 放入 HttpContext.Items 中
            FastContext.HttpContext.Items[$"{nameof(Fast)}.{nameof(ApplicationOpenIdModel.OpenId)}.{openId}"] =
                applicationOpenIdModel;
        }

        return applicationOpenIdModel;
    }

    /// <summary>
    /// 获取应用
    /// </summary>
    /// <param name="openId"><see cref="string"/> 应用标识</param>
    /// <returns></returns>
    public static async Task<ApplicationOpenIdModel> GetApplication(string openId)
    {
        if (string.IsNullOrWhiteSpace(openId))
        {
            throw new UserFriendlyException("应用标识不能为空！");
        }

        // 优先从 HttpContext.Items 中获取
        if (FastContext.HttpContext?.Items.TryGetValue(
                $"{nameof(Fast)}.{nameof(ApplicationOpenIdModel.OpenId)}.{openId}",
                out var obj)
            == true
            && obj is ApplicationOpenIdModel applicationOpenIdModel)
        {
            return applicationOpenIdModel;
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.App, openId);

        applicationOpenIdModel = await centerCache.GetAndSetAsync(cacheKey, async () =>
        {
            var repository = FastContext.GetService<ISqlSugarClient>();

            var result = await repository.Queryable<ApplicationOpenIdModel>()
                .Includes(e => e.Application)
                .Where(wh => wh.OpenId == openId)
                .SingleAsync();

            if (result == null)
            {
                var message = $"未能找到对应应用【{openId}】信息！";
                logger.LogError($"OpenId：{openId}；{message}");
                throw new UserFriendlyException(message);
            }

            return result;
        });

        if (FastContext.HttpContext != null)
        {
            // 放入 HttpContext.Items 中
            FastContext.HttpContext.Items[$"{nameof(Fast)}.{nameof(ApplicationOpenIdModel.OpenId)}.{openId}"] =
                applicationOpenIdModel;
        }

        return applicationOpenIdModel;
    }

    /// <summary>
    /// 删除应用
    /// </summary>
    /// <param name="openId"><see cref="string"/> 应用标识</param>
    /// <returns></returns>
    public static async Task DeleteApplication(string openId)
    {
        if (string.IsNullOrWhiteSpace(openId))
        {
            throw new UserFriendlyException("应用标识不能为空！");
        }

        if (FastContext.HttpContext != null)
        {
            // 删除 HttpContext.Items 中的
            if (FastContext.HttpContext.Items.ContainsKey(
                    $"{nameof(Fast)}.{nameof(ApplicationOpenIdModel.OpenId)}.{openId}"))
            {
                FastContext.HttpContext.Items.Remove(
                    $"{nameof(Fast)}.{nameof(ApplicationOpenIdModel.OpenId)}.{openId}");
            }
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.App, openId);

        await centerCache.DelAsync(cacheKey);
    }

    /// <summary>
    /// 删除所有应用
    /// </summary>
    /// <returns></returns>
    public static async Task DeleteAllApplication()
    {
        if (FastContext.HttpContext != null)
        {
            // 清空 HttpContext.Items 中的
            var keys = FastContext.HttpContext.Items.Keys.Where(wh =>
                    wh is string key && key.StartsWith($"{nameof(Fast)}.{nameof(ApplicationOpenIdModel.OpenId)}."))
                .ToList();
            foreach (var key in keys)
            {
                FastContext.HttpContext.Items.Remove(key);
            }
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.App, "*");
        await centerCache.DelByPatternAsync(cacheKey);
    }
}