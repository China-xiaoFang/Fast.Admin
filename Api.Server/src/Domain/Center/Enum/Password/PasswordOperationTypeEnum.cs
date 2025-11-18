

namespace Fast.Center.Enum;

/// <summary>
/// <see cref="PasswordOperationTypeEnum"/> 密码操作类型枚举
/// </summary>
[Flags]
[FastEnum("密码操作类型枚举")]
public enum PasswordOperationTypeEnum : byte
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <remarks>注册或初始化</remarks>
    [TagType(TagTypeEnum.Info)] [Description("创建")]
    Create = 1,

    /// <summary>
    /// 修改
    /// </summary>
    /// <remarks>用户修改密码</remarks>
    [TagType(TagTypeEnum.Warning)] [Description("修改")]
    Change = 2,

    /// <summary>
    /// 重置
    /// </summary>
    /// <remarks>管理员重置密码</remarks>
    [TagType(TagTypeEnum.Danger)] [Description("重置")]
    Reset = 4,

    /// <summary>
    /// 找回
    /// </summary>
    /// <remarks>用户找回密码</remarks>
    [TagType(TagTypeEnum.Danger)] [Description("找回")]
    Recover = 8
}