// ReSharper disable once CheckNamespace

namespace Fast.Core;

/// <summary>
/// <see cref="CacheConst"/> 缓存常量
/// </summary>
[SuppressSniffer]
public class CacheConst
{
    /// <summary>
    /// 获取缓存Key
    /// </summary>
    /// <param name="cacheKey"><see cref="string"/> 缓存Key</param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string GetCacheKey(string cacheKey, params object[] args)
    {
        return string.Format(cacheKey, args);
    }

    /// <summary>
    /// 授权用户
    /// </summary>
    /// <remarks>{0}应用编号, {1}租户编号, {2}登录环境，{3}工号</remarks>
    public const string AuthUser = "{0}:{1}:Auth:{2}:{3}";

    /// <summary>
    /// <see cref="Center"/> 管理后台
    /// </summary>
    public class Center
    {
        /// <summary>
        /// 数据库
        /// </summary>
        /// <remarks>{0}租户编号，{1}数据库名类型</remarks>
        public const string Database = "Database:{0}:{1}";

        /// <summary>
        /// 配置
        /// </summary>
        /// <remarks>{0}配置编码</remarks>
        public const string Config = "Config:{0}";

        /// <summary>
        /// 租户
        /// </summary>
        /// <remarks>{0}租户编号</remarks>
        public const string Tenant = "Tenant:{0}";

        /// <summary>
        /// 机器人
        /// </summary>
        /// <remarks>{0}租户编号</remarks>
        public const string Rabot = "Rabot:{0}";

        /// <summary>
        /// 应用
        /// </summary>
        /// <remarks>{0}应用标识</remarks>
        public const string App = "App:{0}";

        /// <summary>
        /// 商户号
        /// </summary>
        /// <remarks>{0}商户号</remarks>
        public const string Merchant = "Merchant:{0}";

        /// <summary>
        /// 字典
        /// </summary>
        public const string Dictionary = "Dictionary";

        /// <summary>
        /// 表格配置
        /// </summary>
        /// <remarks>{0}表格Key</remarks>
        public const string TableConfig = "TableConfig:{0}";

        /// <summary>
        /// 用户表格配置缓存
        /// </summary>
        /// <remarks>{0}表格Key，{1}租户编号, {2}工号</remarks>
        public const string UserTableConfigCache = "TableConfig:{0}:{1}:{2}";
    }
}