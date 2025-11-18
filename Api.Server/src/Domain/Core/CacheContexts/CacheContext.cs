using Fast.Center.Entity;


namespace Fast.Core;

/// <summary>
/// <see cref="CacheContext"/> 缓存上下文
/// </summary>
/// <remarks>存放一些启动时可加载的静态变量</remarks>
[SuppressSniffer]
public static class CacheContext
{
    /// <summary>
    /// 接口信息缓存
    /// </summary>
    public static List<ApiInfoModel> ApiInfoList { get; set; }
}