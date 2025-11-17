using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Quartz;

// ReSharper disable once CheckNamespace
namespace Fast.Scheduler;

/// <summary>
/// <see cref="IServiceCollectionExtension"/> 拓展类
/// </summary>
[SuppressSniffer]
public static class IServiceCollectionExtension
{
    /// <summary>
    /// 添加 Quartz 服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddQuartzService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(options =>
        {
            // 设置调度器Id
            options.SchedulerId = "AUTO";

            // 设置关闭时中断作业
            options.InterruptJobsOnShutdown = true;

            // 设置关闭时等待作业中断
            options.InterruptJobsOnShutdownWithWait = true;

            // 默认最大批处理作业数量为 1，这里修改为 10
            options.MaxBatchSize = 10;

            // 默认最大并发为 10，这里修改为 100
            options.UseDefaultThreadPool(100);

            // 配置持久化存储策略
            options.UsePersistentStore(x =>
            {
                // 强制作业数据映射的值被视为字符串，避免对象意外序列化后格式破坏导致的问题，默认为 false
                //x.UseProperties = true;

                // 启用集群模式
                x.UseClustering();

                // 数据库连接字符串
                var connectionString = SqlSugarDatabaseUtil.GetConnectionStr(
                    SqlSugarContext.ConnectionSettings.DbType!.Value,
                    SqlSugarContext.ConnectionSettings);

                switch (SqlSugarContext.ConnectionSettings.DbType)
                {
                    case DbType.MySql:
                        // 使用 MySql 作为持久化存储的提供者
                        x.UseMySql(o =>
                        {
                            // 数据库连接字符串
                            o.ConnectionString = connectionString;
                        }, SqlSugarContext.ConnectionSettings.DbName);
                        break;
                    case DbType.SqlServer:
                        // 使用 Sql Server 作为持久化存储的提供者
                        x.UseSqlServer(o =>
                        {
                            // 数据库连接字符串
                            o.ConnectionString = connectionString;
                        }, SqlSugarContext.ConnectionSettings.DbName);
                        break;
                    case DbType.Sqlite:
                        // 使用 SqlLite 作为持久化存储的提供者
                        x.UseSQLite(o =>
                        {
                            // 数据库连接字符串
                            o.ConnectionString = connectionString;
                        }, SqlSugarContext.ConnectionSettings.DbName);
                        break;
                    case DbType.Oracle:
                        // 使用 Oracle 作为持久化存储的提供者
                        x.UseOracle(o =>
                        {
                            // 数据库连接字符串
                            o.ConnectionString = connectionString;
                        }, SqlSugarContext.ConnectionSettings.DbName);
                        break;
                    case DbType.PostgreSQL:
                        // 使用 Postgres SQL 作为持久化存储的提供者
                        x.UsePostgres(o =>
                        {
                            // 数据库连接字符串
                            o.ConnectionString = connectionString;
                        }, SqlSugarContext.ConnectionSettings.DbName);
                        break;
                }

                // 使用 Newtonsoft.NET 序列化器
                x.UseNewtonsoftJsonSerializer();
            });
        });

        services.AddQuartzHostedService(options =>
        {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;

            // when we need to init another IHostedServices first
            options.StartDelay = TimeSpan.FromSeconds(10);

            // start the task after the application has completed its startup process.
            options.AwaitApplicationStarted = true;
        });

        // 调度器工厂
        services.TryAddSingleton<ContainerConfigurationProcessor>();
        services.TryAddSingleton<IDependencySchedulerFactory, DependencySchedulerFactory>();

        // 本地作业服务
        var ISchedulerJobType = typeof(ISchedulerJob);
        var schedulerJobTypes = MAppContext.EffectiveTypes.Where(wh =>
                ISchedulerJobType.IsAssignableFrom(wh) && wh.IsClass && !wh.IsInterface && !wh.IsAbstract)
            .ToList();
        foreach (var type in schedulerJobTypes)
        {
            services.TryAddScoped(type);
        }

        return services;
    }
}