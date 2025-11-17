// ReSharper disable once CheckNamespace

namespace Fast.Center.Enum;

/// <summary>
/// <see cref="PasswordTypeEnum"/> 密码类型枚举
/// </summary>
[Flags]
[FastEnum("密码类型枚举")]
public enum PasswordTypeEnum : byte
{
    /// <summary>
    /// MD5
    /// </summary>
    [Description("MD5")] MD5 = 1,

    /// <summary>
    /// SHA1
    /// </summary>
    [Description("SHA1")] SHA1 = 2
}