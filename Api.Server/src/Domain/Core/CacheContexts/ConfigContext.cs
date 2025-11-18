using Fast.Center.Entity;
using Fast.SqlSugar;
using Microsoft.Extensions.Logging;


namespace Fast.Core;

/// <summary>
/// <see cref="ConfigContext"/> 配置上下文
/// </summary>
[SuppressSniffer]
public class ConfigContext
{
    /// <summary>
    /// 缓存
    /// </summary>
    internal static ICache<CenterCCL> centerCache = FastContext.GetService<ICache<CenterCCL>>();

    /// <summary>
    /// 日志
    /// </summary>
    internal static ILogger logger = FastContext.GetService<ILogger<ConfigContext>>();

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="configCode"><see cref="string"/> 配置编码</param>
    /// <returns></returns>
    public static string GetConfigSync(string configCode)
    {
        if (string.IsNullOrWhiteSpace(configCode))
        {
            throw new UserFriendlyException("配置编码不能为空！");
        }

        // 优先从 HttpContext.Items 中获取
        var configValue = FastContext.HttpContext
            ?.Items[$"{nameof(Fast)}.{nameof(ConfigModel.ConfigCode)}.{configCode}"]
            ?.ToString();

        if (!string.IsNullOrWhiteSpace(configValue))
            return configValue;

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Config, configCode);

        var configModel = centerCache.GetAndSet(cacheKey, () =>
        {
            var repository = FastContext.GetService<ISqlSugarRepository<ConfigModel>>();

            var result = repository.Entities.Where(wh => wh.ConfigCode == configCode)
                .Single();

            if (result == null)
            {
                var message = $"未能找到对应配置【{configCode}】信息！";
                logger.LogError($"ConfigCode：{configCode}；{message}");
                throw new UserFriendlyException(message);
            }

            return result;
        });

        if (FastContext.HttpContext != null)
        {
            // 放入 HttpContext.Items 中
            FastContext.HttpContext.Items[$"{nameof(Fast)}.{nameof(ConfigModel.ConfigCode)}.{configCode}"] =
                configModel.ConfigValue;
        }

        if (string.IsNullOrWhiteSpace(configModel.ConfigValue))
        {
            var message = $"配置【{configCode}】信息值为空！";
            logger.LogError($"ConfigCode：{configCode}；{message}");
            throw new UserFriendlyException(message);
        }

        return configModel.ConfigValue;
    }

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="configCode"><see cref="string"/> 配置编码</param>
    /// <returns></returns>
    public static async Task<string> GetConfig(string configCode)
    {
        if (string.IsNullOrWhiteSpace(configCode))
        {
            throw new UserFriendlyException("配置编码不能为空！");
        }

        // 优先从 HttpContext.Items 中获取
        var configValue = FastContext.HttpContext
            ?.Items[$"{nameof(Fast)}.{nameof(ConfigModel.ConfigCode)}.{configCode}"]
            ?.ToString();

        if (!string.IsNullOrWhiteSpace(configValue))
            return configValue;

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Config, configCode);

        var configModel = await centerCache.GetAndSetAsync(cacheKey, async () =>
        {
            var repository = FastContext.GetService<ISqlSugarRepository<ConfigModel>>();

            var result = await repository.Entities.Where(wh => wh.ConfigCode == configCode)
                .SingleAsync();

            if (result == null)
            {
                var message = $"未能找到对应配置【{configCode}】信息！";
                logger.LogError($"ConfigCode：{configCode}；{message}");
                throw new UserFriendlyException(message);
            }

            return result;
        });

        if (FastContext.HttpContext != null)
        {
            // 放入 HttpContext.Items 中
            FastContext.HttpContext.Items[$"{nameof(Fast)}.{nameof(ConfigModel.ConfigCode)}.{configCode}"] =
                configModel.ConfigValue;
        }

        if (string.IsNullOrWhiteSpace(configModel.ConfigValue))
        {
            var message = $"配置【{configCode}】信息值为空！";
            logger.LogError($"ConfigCode：{configCode}；{message}");
            throw new UserFriendlyException(message);
        }

        return configModel.ConfigValue;
    }

    /// <summary>
    /// 删除配置
    /// </summary>
    /// <param name="configCode"><see cref="string"/> 配置编码</param>
    /// <returns></returns>
    public static async Task DeleteConfig(string configCode)
    {
        if (string.IsNullOrWhiteSpace(configCode))
        {
            throw new UserFriendlyException("配置编码不能为空！");
        }

        if (FastContext.HttpContext != null)
        {
            // 删除 HttpContext.Items 中的
            if (FastContext.HttpContext.Items.ContainsKey(
                    $"{nameof(Fast)}.{nameof(ConfigModel.ConfigCode)}.{configCode}"))
            {
                FastContext.HttpContext.Items.Remove($"{nameof(Fast)}.{nameof(ConfigModel.ConfigCode)}.{configCode}");
            }
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Config, configCode);

        await centerCache.DelAsync(cacheKey);
    }

    /// <summary>
    /// 删除所有配置
    /// </summary>
    /// <returns></returns>
    public static async Task DeleteAllConfig()
    {
        if (FastContext.HttpContext != null)
        {
            // 清空 HttpContext.Items 中的
            var keys = FastContext.HttpContext.Items.Keys.Where(wh =>
                    wh is string key && key.StartsWith($"{nameof(Fast)}.{nameof(ConfigModel.ConfigCode)}."))
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