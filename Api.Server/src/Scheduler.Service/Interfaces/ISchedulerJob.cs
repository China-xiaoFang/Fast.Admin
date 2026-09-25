// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Scheduler;

/// <summary>
/// 调度作业
/// </summary>
/// <remarks>实现类通过依赖注入创建，构造函数参数必须能够从服务容器中解析</remarks>
public interface ISchedulerJob
{
    /// <summary>
    /// 获取本地作业
    /// </summary>
    /// <returns>本地作业</returns>
    SchedulerLocalJobInfo GetLocalJob();

    /// <summary>
    /// 执行作业
    /// </summary>
    /// <param name="serviceProvider">当前作业的请求作用域服务提供者；指定租户时，作用域内的 <see cref="IUser"/> 已设置为对应租户机器人用户</param>
    /// <param name="db">SqlSugar 上下文</param>
    /// <param name="logInfo">日志信息</param>
    /// <returns>作业执行日志</returns>
    Task<string> Execute(IServiceProvider serviceProvider, ISqlSugarClient db, SchedulerJobLocalLogInfo logInfo);
}
