

namespace Fast.Shared;

/// <summary>
/// <see cref="ResponseEncipherAttribute"/> 响应加密特性
/// </summary>
[SuppressSniffer]
[AttributeUsage(AttributeTargets.Method)]
public class ResponseEncipherAttribute : Attribute
{
}