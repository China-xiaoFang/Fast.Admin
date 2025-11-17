// ReSharper disable once CheckNamespace

namespace Fast.Center.Enum;

/// <summary>
/// <see cref="SysSerialRuleTypeEnum"/> 系统序号规则类型枚举
/// </summary>
[FastEnum("系统序号规则类型枚举")]
public enum SysSerialRuleTypeEnum : byte
{
    /// <summary>
    /// 应用编号
    /// </summary>
    [Description("应用编号")] AppNo = 1,

    /// <summary>
    /// 租户编号
    /// </summary>
    [Description("租户编号")] TenantNo = 2
}