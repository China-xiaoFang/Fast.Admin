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
            return (T)Enum.ToObject(enumType, value);
        }

        if (Enum.IsDefined(enumType, value))
        {
            return (T)Enum.ToObject(enumType, value);
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
            return (T)Enum.ToObject(underlyingType, value);
        }

        if (Enum.IsDefined(underlyingType, value))
        {
            return (T)Enum.ToObject(underlyingType, value);
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