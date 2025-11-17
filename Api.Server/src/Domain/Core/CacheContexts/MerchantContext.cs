using Fast.Center.Entity;
using Microsoft.Extensions.Logging;
using SqlSugar;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="MerchantContext"/> 商户号上下文
/// </summary>
[SuppressSniffer]
public class MerchantContext
{
    /// <summary>
    /// 缓存
    /// </summary>
    internal static ICache<CenterCCL> centerCache = FastContext.GetService<ICache<CenterCCL>>();

    /// <summary>
    /// 日志
    /// </summary>
    internal static ILogger logger = FastContext.GetService<ILogger<MerchantContext>>();

    /// <summary>
    /// 获取商户号
    /// </summary>
    /// <param name="merchantNo"><see cref="string"/> 商户号</param>
    /// <returns></returns>
    public static MerchantModel GetMerchantSync(string merchantNo)
    {
        if (string.IsNullOrWhiteSpace(merchantNo))
        {
            throw new UserFriendlyException("商户号不能为空！");
        }

        // 优先从 HttpContext.Items 中获取
        if (FastContext.HttpContext?.Items.TryGetValue(
                $"{nameof(Fast)}.{nameof(MerchantModel.MerchantNo)}.{merchantNo}",
                out var obj)
            == true
            && obj is MerchantModel merchantModel)
        {
            return merchantModel;
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Merchant, merchantNo);

        merchantModel = centerCache.GetAndSet(cacheKey, () =>
        {
            var repository = FastContext.GetService<ISqlSugarClient>();

            var result = repository.Queryable<MerchantModel>()
                .Where(wh => wh.MerchantNo == merchantNo)
                .Single();

            if (result == null)
            {
                var message = $"未能找到对应商户号【{merchantNo}】信息！";
                logger.LogError($"MerchantNo：{merchantNo}；{message}");
                throw new UserFriendlyException(message);
            }

            return result;
        });

        if (FastContext.HttpContext != null)
        {
            // 放入 HttpContext.Items 中
            FastContext.HttpContext.Items[$"{nameof(Fast)}.{nameof(MerchantModel.MerchantNo)}.{merchantNo}"] =
                merchantModel;
        }

        return merchantModel;
    }

    /// <summary>
    /// 获取商户号
    /// </summary>
    /// <param name="merchantNo"><see cref="string"/> 商户号</param>
    /// <returns></returns>
    public static async Task<MerchantModel> GetMerchant(string merchantNo)
    {
        if (string.IsNullOrWhiteSpace(merchantNo))
        {
            throw new UserFriendlyException("商户号不能为空！");
        }

        // 优先从 HttpContext.Items 中获取
        if (FastContext.HttpContext?.Items.TryGetValue(
                $"{nameof(Fast)}.{nameof(MerchantModel.MerchantNo)}.{merchantNo}",
                out var obj)
            == true
            && obj is MerchantModel merchantModel)
        {
            return merchantModel;
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Merchant, merchantNo);

        merchantModel = await centerCache.GetAndSetAsync(cacheKey, async () =>
        {
            var repository = FastContext.GetService<ISqlSugarClient>();

            var result = await repository.Queryable<MerchantModel>()
                .Where(wh => wh.MerchantNo == merchantNo)
                .SingleAsync();

            if (result == null)
            {
                var message = $"未能找到对应商户号【{merchantNo}】信息！";
                logger.LogError($"MerchantNo：{merchantNo}；{message}");
                throw new UserFriendlyException(message);
            }

            return result;
        });

        if (FastContext.HttpContext != null)
        {
            // 放入 HttpContext.Items 中
            FastContext.HttpContext.Items[$"{nameof(Fast)}.{nameof(MerchantModel.MerchantNo)}.{merchantNo}"] =
                merchantModel;
        }

        return merchantModel;
    }

    /// <summary>
    /// 删除商户号
    /// </summary>
    /// <param name="merchantNo"><see cref="string"/> 商户号</param>
    /// <returns></returns>
    public static async Task DeleteMerchant(string merchantNo)
    {
        if (string.IsNullOrWhiteSpace(merchantNo))
        {
            throw new UserFriendlyException("商户号不能为空！");
        }

        if (FastContext.HttpContext != null)
        {
            // 删除 HttpContext.Items 中的
            if (FastContext.HttpContext.Items.ContainsKey(
                    $"{nameof(Fast)}.{nameof(MerchantModel.MerchantNo)}.{merchantNo}"))
            {
                FastContext.HttpContext.Items.Remove($"{nameof(Fast)}.{nameof(MerchantModel.MerchantNo)}.{merchantNo}");
            }
        }

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.Merchant, merchantNo);

        await centerCache.DelAsync(cacheKey);
    }

    /// <summary>
    /// 删除所有商户号
    /// </summary>
    /// <returns></returns>
    public static async Task DeleteAllMerchant()
    {
        if (FastContext.HttpContext != null)
        {
            // 清空 HttpContext.Items 中的
            var keys = FastContext.HttpContext.Items.Keys.Where(wh =>
                    wh is string key && key.StartsWith($"{nameof(Fast)}.{nameof(MerchantModel.MerchantNo)}."))
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