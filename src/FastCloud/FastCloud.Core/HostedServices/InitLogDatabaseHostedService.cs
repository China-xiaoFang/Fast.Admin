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

using Fast.Common;
using Fast.FastCloudLog.Entity;
using Fast.NET.Core;
using Fast.SqlSugar;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

// ReSharper disable once CheckNamespace
namespace Fast.FastCloud.Core;

/// <summary>
/// <see cref="InitLogDatabaseHostedService"/> 初始化日志 Database 托管服务
/// </summary>
[Order(2)]
public class InitLogDatabaseHostedService : IHostedService
{
    /// <summary>
    /// SqlSugar实体服务
    /// </summary>
    private readonly ISqlSugarEntityService _sqlSugarEntityService;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    public InitLogDatabaseHostedService(ISqlSugarEntityService sqlSugarEntityService,
        ILogger<InitLogDatabaseHostedService> logger)
    {
        _sqlSugarEntityService = sqlSugarEntityService;
        _logger = logger;
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            // 查询 FastCloudLog 库连接字符串
            var connectionSettings = await _sqlSugarEntityService.GetConnectionSetting(CommonConst.DefaultPlatformId,
                CommonConst.DefaultPlatformNo, DatabaseTypeEnum.FastCloudLog);

            if (connectionSettings == null)
                return;

            // 创建日志库上下文
            var db = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(connectionSettings));
            // 加载 Aop
            SugarEntityFilter.LoadSugarAop(FastContext.HostEnvironment.IsDevelopment(), db);

            // 创建核心库
            db.DbMaintenance.CreateDatabase();

            // 查询核心表是否存在
            var allTableNames = db.SplitHelper<SqlExecutionLogModel>()
                .GetTables();
            if (allTableNames.Count > 0)
                return;

            _logger.LogInformation("开始初始化日志数据库...");

            // 获取所有不分表的Model类型
            var tableTypes = SqlSugarContext.SqlSugarEntityList.Where(wh => !wh.IsSplitTable)
                .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum) wh.SugarDbType == DatabaseTypeEnum.FastCloudLog)
                .Select(sl => sl.EntityType)
                .ToArray();
            // 获取所有分表的Model类型
            var splitTableTypes = SqlSugarContext.SqlSugarEntityList.Where(wh => wh.IsSplitTable)
                .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum) wh.SugarDbType == DatabaseTypeEnum.FastCloudLog)
                .Select(sl => sl.EntityType)
                .ToArray();

            // 创建表
            db.CodeFirst.InitTables(tableTypes);
            db.CodeFirst.SplitTables()
                .InitTables(splitTableTypes);

            _logger.LogInformation("初始化日志数据库成功。");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Init log database error...");
        }
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}