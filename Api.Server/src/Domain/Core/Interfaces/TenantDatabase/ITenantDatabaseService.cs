

namespace Fast.Core;

/// <summary>
/// <see cref="ITenantDatabaseService"/> 租户数据库服务
/// </summary>
/// <remarks>需要在 Admin 服务中实现</remarks>
public interface ITenantDatabaseService
{
    /// <summary>
    /// 初始化租户数据库
    /// </summary>
    /// <param name="tenantId"><see cref="long"/> 租户Id</param>
    /// <returns></returns>
    Task InitTenantDatabase(long tenantId);
}