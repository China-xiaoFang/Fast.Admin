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