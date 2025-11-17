// ReSharper disable once CheckNamespace

namespace Fast.Scheduler;

/// <summary>
/// <see cref="DisableWaitAttribute"/> 禁用等待
/// </summary>
/// <remarks>针对 LocalJob 且是全部租户作业的，禁用随机等待操作</remarks>
[AttributeUsage(AttributeTargets.Class)]
public class DisableWaitAttribute : Attribute
{
}