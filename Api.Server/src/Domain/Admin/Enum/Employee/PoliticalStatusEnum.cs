

namespace Fast.Admin.Enum;

/// <summary>
/// <see cref="PoliticalStatusEnum"/> 政治面貌枚举
/// </summary>
[Flags]
[FastEnum("政治面貌枚举")]
public enum PoliticalStatusEnum : byte
{
    /// <summary>
    /// 群众
    /// </summary>
    [Description("群众")] Masses = 1,

    /// <summary>
    /// 共青团员
    /// </summary>
    [Description("共青团员")] YouthLeague = 2,

    /// <summary>
    /// 中共党员
    /// </summary>
    [Description("中共党员")] PartyMember = 4,

    /// <summary>
    /// 民主党派
    /// </summary>
    [Description("民主党派")] Democratic = 8,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")] Other = 16
}