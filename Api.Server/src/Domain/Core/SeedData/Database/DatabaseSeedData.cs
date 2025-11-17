using Fast.Center.Entity;
using Fast.SqlSugar;
using Microsoft.Extensions.Hosting;
using SqlSugar;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="DatabaseSeedData"/> 系统数据库种子数据
/// </summary>
internal static class DatabaseSeedData
{
    /// <summary>
    /// 系统数据库种子数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="tenantId"><see cref="long"/> 租户Id</param>
    /// <param name="tenantCode"><see cref="string"/> 租户编码</param>
    /// <param name="dateTime"><see cref="DateTime"/> 时间</param>
    /// <returns></returns>
    public static async Task SystemDatabaseSeedData(ISqlSugarClient db, long tenantId, string tenantCode,
        DateTime dateTime)
    {
        var isDevelopment = FastContext.HostEnvironment.IsDevelopment();
        await db.Insertable(new List<MainDatabaseModel>
            {
                // 初始化日志库
                new()
                {
                    MainId = YitIdHelper.NextId(),
                    DatabaseType = DatabaseTypeEnum.CenterLog,
                    DbType = SqlSugarContext.ConnectionSettings.DbType ?? DbType.SqlServer,
                    PublicIp = SqlSugarContext.ConnectionSettings.ServiceIp,
                    IntranetIp = "127.0.0.1",
                    Port = SqlSugarContext.ConnectionSettings.Port ?? 1433,
                    DbName = isDevelopment ? "FastCenter_Log_Dev" : "FastCenter_Log",
                    DbUser = SqlSugarContext.ConnectionSettings.DbUser,
                    DbPwd = SqlSugarContext.ConnectionSettings.DbPwd,
                    CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
                    SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
                    DiffLog = false,
                    DisableAop = true,
                    CreatedTime = dateTime,
                    TenantId = tenantId
                },
                // 初始化网关系统库
                new()
                {
                    MainId = YitIdHelper.NextId(),
                    DatabaseType = DatabaseTypeEnum.Gateway,
                    DbType = SqlSugarContext.ConnectionSettings.DbType ?? DbType.SqlServer,
                    PublicIp = SqlSugarContext.ConnectionSettings.ServiceIp,
                    IntranetIp = "127.0.0.1",
                    Port = SqlSugarContext.ConnectionSettings.Port ?? 1433,
                    DbName = isDevelopment ? "FastGateway_Dev" : "FastGateway",
                    DbUser = SqlSugarContext.ConnectionSettings.DbUser,
                    DbPwd = SqlSugarContext.ConnectionSettings.DbPwd,
                    CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
                    SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
                    DiffLog = false,
                    DisableAop = true,
                    CreatedTime = dateTime,
                    TenantId = tenantId
                },
                // 初始化部署系统库
                new()
                {
                    MainId = YitIdHelper.NextId(),
                    DatabaseType = DatabaseTypeEnum.Deploy,
                    DbType = SqlSugarContext.ConnectionSettings.DbType ?? DbType.SqlServer,
                    PublicIp = SqlSugarContext.ConnectionSettings.ServiceIp,
                    IntranetIp = "127.0.0.1",
                    Port = SqlSugarContext.ConnectionSettings.Port ?? 1433,
                    DbName = isDevelopment ? "FastDeploy_Dev" : "FastDeploy",
                    DbUser = SqlSugarContext.ConnectionSettings.DbUser,
                    DbPwd = SqlSugarContext.ConnectionSettings.DbPwd,
                    CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
                    SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
                    DiffLog = false,
                    DisableAop = true,
                    CreatedTime = dateTime,
                    TenantId = tenantId
                },
                // 初始化业务库
                new()
                {
                    MainId = YitIdHelper.NextId(),
                    DatabaseType = DatabaseTypeEnum.Admin,
                    DbType = SqlSugarContext.ConnectionSettings.DbType ?? DbType.SqlServer,
                    PublicIp = SqlSugarContext.ConnectionSettings.ServiceIp,
                    IntranetIp = "127.0.0.1",
                    Port = SqlSugarContext.ConnectionSettings.Port ?? 1433,
                    DbName = isDevelopment ? $"{tenantCode}Admin_Dev" : $"{tenantCode}Admin",
                    DbUser = SqlSugarContext.ConnectionSettings.DbUser,
                    DbPwd = SqlSugarContext.ConnectionSettings.DbPwd,
                    CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
                    SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
                    DiffLog = true,
                    DisableAop = true,
                    CreatedTime = dateTime,
                    TenantId = tenantId
                },
                // 初始化业务日志库
                new()
                {
                    MainId = YitIdHelper.NextId(),
                    DatabaseType = DatabaseTypeEnum.AdminLog,
                    DbType = SqlSugarContext.ConnectionSettings.DbType ?? DbType.SqlServer,
                    PublicIp = SqlSugarContext.ConnectionSettings.ServiceIp,
                    IntranetIp = "127.0.0.1",
                    Port = SqlSugarContext.ConnectionSettings.Port ?? 1433,
                    DbName = isDevelopment ? $"{tenantCode}Admin_Log_Dev" : $"{tenantCode}Admin_Log",
                    DbUser = SqlSugarContext.ConnectionSettings.DbUser,
                    DbPwd = SqlSugarContext.ConnectionSettings.DbPwd,
                    CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
                    SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
                    DiffLog = false,
                    DisableAop = true,
                    CreatedTime = dateTime,
                    TenantId = tenantId
                }
            })
            .ExecuteCommandAsync();
    }

    /// <summary>
    /// 租户数据库种子数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="tenantId"><see cref="long"/> 租户Id</param>
    /// <param name="tenantCode"><see cref="string"/> 租户编码</param>
    /// <param name="dateTime"><see cref="DateTime"/> 时间</param>
    /// <returns></returns>
    public static async Task TenantDatabaseSeedData(ISqlSugarClient db, long tenantId, string tenantCode,
        DateTime dateTime)
    {
        var isDevelopment = FastContext.HostEnvironment.IsDevelopment();
        await db.Insertable(new List<MainDatabaseModel>
            {
                // 初始化业务库
                new()
                {
                    MainId = YitIdHelper.NextId(),
                    DatabaseType = DatabaseTypeEnum.Admin,
                    DbType = SqlSugarContext.ConnectionSettings.DbType ?? DbType.SqlServer,
                    PublicIp = SqlSugarContext.ConnectionSettings.ServiceIp,
                    IntranetIp = "127.0.0.1",
                    Port = SqlSugarContext.ConnectionSettings.Port ?? 1433,
                    DbName = isDevelopment ? $"{tenantCode}Admin_Dev" : $"{tenantCode}Admin",
                    DbUser = SqlSugarContext.ConnectionSettings.DbUser,
                    DbPwd = SqlSugarContext.ConnectionSettings.DbPwd,
                    CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
                    SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
                    DiffLog = true,
                    DisableAop = true,
                    CreatedTime = dateTime,
                    TenantId = tenantId
                },
                // 初始化业务日志库
                new()
                {
                    MainId = YitIdHelper.NextId(),
                    DatabaseType = DatabaseTypeEnum.AdminLog,
                    DbType = SqlSugarContext.ConnectionSettings.DbType ?? DbType.SqlServer,
                    PublicIp = SqlSugarContext.ConnectionSettings.ServiceIp,
                    IntranetIp = "127.0.0.1",
                    Port = SqlSugarContext.ConnectionSettings.Port ?? 1433,
                    DbName = isDevelopment ? $"{tenantCode}Admin_Log_Dev" : $"{tenantCode}Admin_Log",
                    DbUser = SqlSugarContext.ConnectionSettings.DbUser,
                    DbPwd = SqlSugarContext.ConnectionSettings.DbPwd,
                    CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
                    SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
                    DiffLog = false,
                    DisableAop = true,
                    CreatedTime = dateTime,
                    TenantId = tenantId
                }
            })
            .ExecuteCommandAsync();
    }
}