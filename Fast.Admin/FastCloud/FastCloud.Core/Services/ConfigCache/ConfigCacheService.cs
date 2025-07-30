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

using Fast.Cache;
using Fast.DependencyInjection;
using Fast.FastCloud.Entity;
using Fast.NET.Core;
using Fast.SqlSugar;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Fast.FastCloud.Core;

/// <summary>
/// <see cref="ConfigCacheService"/> 配置缓存服务
/// </summary>
public class ConfigCacheService : IConfigCacheService, IScopedDependency
{
    /// <summary>
    /// 仓储
    /// </summary>
    private readonly ISqlSugarRepository<ConfigModel> _repository;

    /// <summary>
    /// 缓存
    /// </summary>
    private readonly ICache _cache;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    public ConfigCacheService(ISqlSugarRepository<ConfigModel> repository, ICache cache, ILogger<IConfigCacheService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    /// <summary>
    /// 获取配置
    /// </summary>
    /// <param name="configCode"><see cref="string"/> 配置编码</param>
    /// <returns></returns>
    public async Task<string> GetConfig(string configCode)
    {
        if (string.IsNullOrWhiteSpace(configCode))
        {
            throw new UserFriendlyException("配置编码不能为空！");
        }

        // 优先从 HttpContext.Items 中获取
        var configValue = FastContext.HttpContext?.Items[$"{nameof(Fast)}.{nameof(ConfigModel.ConfigCode)}.{configCode}"]
            .ToString();

        if (!string.IsNullOrWhiteSpace(configValue))
            return configValue;

        var cacheKey = CacheConst.GetCacheKey(CacheConst.ConfigInfo, configCode);

        var configModel = await _cache.GetAndSetAsync(cacheKey, async () =>
        {
            var result = await _repository.Entities.Where(wh => wh.ConfigCode == configCode)
                .SingleAsync();

            if (result == null)
            {
                var message = $"未能找到对应配置【{configCode}】信息！";
                _logger.LogError($"ConfigCode：{configCode}；{message}");
                throw new ArgumentNullException(message);
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
            _logger.LogError($"ConfigCode：{configCode}；{message}");
            throw new ArgumentNullException(message);
        }

        return configModel.ConfigValue;
    }

    /// <summary>
    /// 删除配置
    /// </summary>
    /// <param name="configCode"><see cref="string"/> 配置编码</param>
    /// <returns></returns>
    public async Task DeleteConfig(string configCode)
    {
        if (string.IsNullOrWhiteSpace(configCode))
        {
            throw new UserFriendlyException("配置编码不能为空！");
        }

        if (FastContext.HttpContext != null)
        {
            // 删除 HttpContext.Items 中的
            if (FastContext.HttpContext.Items.ContainsKey($"{nameof(Fast)}.{nameof(ConfigModel.ConfigCode)}.{configCode}"))
            {
                FastContext.HttpContext.Items.Remove($"{nameof(Fast)}.{nameof(ConfigModel.ConfigCode)}.{configCode}");
            }
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.ConfigInfo, configCode);

        await _cache.DelAsync(cacheKey);
    }
}