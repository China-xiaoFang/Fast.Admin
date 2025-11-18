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

using Quartz;

namespace Fast.Scheduler;

/// <summary>
/// <see cref="JobDataMap"/> 拓展类
/// </summary>
[SuppressSniffer]
public static class JobDataMapExtension
{
    /// <summary>
    /// 获取可空类型的枚举值，只支持 Int 类型的枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jobData"></param>
    /// <param name="key"></param>
    /// <returns></returns>、
    public static T GetEnum<T>(this JobDataMap jobData, string key) where T : Enum
    {
        // 获取值
        var value = jobData.GetInt(key);

        // 获取枚举类型
        var enumType = typeof(T);

        var flagValues = 0;
        foreach (var enumValue in Enum.GetValues(enumType))
        {
            flagValues |= Convert.ToInt32(enumValue);
        }

        if ((value & flagValues) == value)
        {
            return (T) Enum.ToObject(enumType, value);
        }

        if (Enum.IsDefined(enumType, value))
        {
            return (T) Enum.ToObject(enumType, value);
        }

        throw new ArgumentNullException(key);
    }

    /// <summary>
    /// 获取可空类型的枚举值，只支持 Int 类型的枚举
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jobData"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T? GetNullableEnum<T>(this JobDataMap jobData, string key) where T : struct, Enum
    {
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (!jobData.ContainsKey(key))
        {
            return null;
        }

        if (jobData[key] == null)
        {
            return null;
        }

        // 获取值
        var value = Convert.ToInt32(jobData[key]);

        // 获取枚举类型
        var enumType = typeof(T);
        var underlyingType = Nullable.GetUnderlyingType(enumType) ?? enumType;

        var flagValues = 0;
        foreach (var enumValue in Enum.GetValues(underlyingType))
        {
            flagValues |= Convert.ToInt32(enumValue);
        }

        if ((value & flagValues) == value)
        {
            return (T) Enum.ToObject(underlyingType, value);
        }

        if (Enum.IsDefined(underlyingType, value))
        {
            return (T) Enum.ToObject(underlyingType, value);
        }

        return null;
    }

    /// <summary>
    /// 获取 Int 类型值
    /// </summary>
    /// <param name="jobData"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static int? GetNullableInt(this JobDataMap jobData, string key)
    {
        // 判断是否存在
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (!jobData.ContainsKey(key))
        {
            return null;
        }

        if (jobData[key] == null)
        {
            return null;
        }

        return Convert.ToInt32(jobData[key]);
    }

    /// <summary>
    /// 获取 Long 类型值
    /// </summary>
    /// <param name="jobData"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static long? GetNullableLong(this JobDataMap jobData, string key)
    {
        // 判断是否存在
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (!jobData.ContainsKey(key))
        {
            return null;
        }

        if (jobData[key] == null)
        {
            return null;
        }

        return Convert.ToInt64(jobData[key]);
    }

    /// <summary>
    /// 获取 DateTime 类型值
    /// </summary>
    /// <param name="jobData"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static DateTime? GetNullableDateTime(this JobDataMap jobData, string key)
    {
        // 判断是否存在
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (!jobData.ContainsKey(key))
        {
            return null;
        }

        if (jobData[key] == null)
        {
            return null;
        }

        return DateTime.Parse(jobData[key]
            .ToString());
    }

    /// <summary>
    /// 获取 Boolean 类型值
    /// </summary>
    /// <param name="jobData"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool? GetNullableBoolean(this JobDataMap jobData, string key)
    {
        // 判断是否存在
        // ReSharper disable once CanSimplifyDictionaryLookupWithTryGetValue
        if (!jobData.ContainsKey(key))
        {
            return null;
        }

        if (jobData[key] == null)
        {
            return null;
        }

        return Convert.ToBoolean(jobData[key]
            .ToString());
    }
}