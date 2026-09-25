// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Domain;

/// <summary>
/// 密码操作类型枚举
/// </summary>
[Flags]
[FastEnum("密码操作类型枚举")]
public enum PasswordOperationTypeEnum : byte
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <remarks>注册或初始化</remarks>
    [TagType(TagTypeEnum.Info)]
    [Description("创建")]
    Create = 1,

    /// <summary>
    /// 修改
    /// </summary>
    /// <remarks>用户修改密码</remarks>
    [TagType(TagTypeEnum.Warning)]
    [Description("修改")]
    Change = 2,

    /// <summary>
    /// 重置
    /// </summary>
    /// <remarks>管理员重置密码</remarks>
    [TagType(TagTypeEnum.Danger)]
    [Description("重置")]
    Reset = 4,

    /// <summary>
    /// 找回
    /// </summary>
    /// <remarks>用户找回密码</remarks>
    [TagType(TagTypeEnum.Danger)]
    [Description("找回")]
    Recover = 8
}
