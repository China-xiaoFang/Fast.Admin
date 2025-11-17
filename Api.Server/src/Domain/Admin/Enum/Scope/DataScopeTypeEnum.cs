// ReSharper disable once CheckNamespace

namespace Fast.Admin.Enum;

/// <summary>
/// <see cref="DataScopeTypeEnum"/> 数据范围类型枚举
/// </summary>
[Flags]
[FastEnum("数据范围类型枚举")]
public enum DataScopeTypeEnum : byte
{
    /// <summary>
    /// 全部数据
    /// </summary>
    [Description("全部数据")] All = 1,

    /// <summary>
    /// 本机构及以下数据
    /// </summary>
    /// <remarks>当前主机构下所有部门的数据</remarks>
    [Description("本机构及以下数据")] OrgWithChild = 2,

    /// <summary>
    /// 本机构数据
    /// </summary>
    /// <remarks>当前主机构下直属部门的数据</remarks>
    [Description("本机构数据")] Org = 4,

    /// <summary>
    /// 本部门及以下数据
    /// </summary>
    [Description("本部门及以下数据")] DeptWithChild = 8,

    /// <summary>
    /// 本部门数据
    /// </summary>
    [Description("本部门数据")] Dept = 16,

    /// <summary>
    /// 仅本人数据
    /// </summary>
    [Description("仅本人数据")] Self = 32,

    /// <summary>
    /// 自定义数据
    /// </summary>
    [Description("自定义数据")] Custom = 64
}