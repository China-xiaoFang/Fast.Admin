// ReSharper disable once CheckNamespace

namespace Fast.Admin.Enum;

/// <summary>
/// <see cref="SerialRuleTypeEnum"/> 序号规则类型枚举
/// </summary>
[FastEnum("序号规则类型枚举")]
public enum SerialRuleTypeEnum : byte
{
    /// <summary>
    /// 工号
    /// </summary>
    [Description("工号")] EmployeeNo = 1
}