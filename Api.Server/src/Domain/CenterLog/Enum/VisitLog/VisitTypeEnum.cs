

namespace Fast.CenterLog.Enum;

/// <summary>
/// <see cref="VisitTypeEnum"/> 访问类型枚举
/// </summary>
[Flags]
[FastEnum("访问类型枚举")]
public enum VisitTypeEnum : byte
{
    /// <summary>
    /// 登录
    /// </summary>
    [TagType(TagTypeEnum.Primary)] [Description("登录")]
    Login = 1,

    /// <summary>
    /// 登出
    /// </summary>
    [TagType(TagTypeEnum.Warning)] [Description("登出")]
    Logout = 2,

    /// <summary>
    /// 改密
    /// </summary>
    [TagType(TagTypeEnum.Danger)] [Description("改密")]
    ChangePassword = 4,

    /// <summary>
    /// 授权登录
    /// </summary>
    [TagType(TagTypeEnum.Success)] [Description("授权登录")]
    AuthorizedLogin = 8
}