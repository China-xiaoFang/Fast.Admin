// ReSharper disable once CheckNamespace

namespace Fast.Scheduler;

/// <summary>
/// <see cref="ISchedulerJob"/> 调度作业
/// </summary>
/// <remarks>注：如果存在DI注入，已经要包含一个空的构造函数</remarks>
public interface ISchedulerJob
{
    /// <summary>
    /// 获取本地作业
    /// </summary>
    /// <returns></returns>
    SchedulerLocalJobInfo GetLocalJob();

    /// <summary>
    /// 执行作业
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> 服务提供者（请求作用域类似于，如果存在 TenantId 则自动注入 IUser 服务）</param>
    /// <param name="db"><see cref="ISqlSugarClient"/> SqlSugar上下文</param>
    /// <param name="logInfo"><see cref="SchedulerJobLocalLogInfo"/> 日志信息</param>
    /// <returns></returns>
    Task<string> Execute(IServiceProvider serviceProvider, ISqlSugarClient db, SchedulerJobLocalLogInfo logInfo);
}