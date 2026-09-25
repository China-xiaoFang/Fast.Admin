// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Admin.Service.Role.Dto;

/// <summary>
/// 角色授权输入
/// </summary>
public class RoleAuthInput : RoleIdInput
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public string RoleName { get; set; }

    /// <summary>
    /// 菜单Id集合
    /// </summary>
    public List<long> MenuIds { get; set; }

    /// <summary>
    /// 按钮Id集合
    /// </summary>
    public List<long> ButtonIds { get; set; }
}
