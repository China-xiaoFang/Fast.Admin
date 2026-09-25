// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Core;

/// <summary>
/// 授权刷新信息Dto
/// </summary>
public class RefreshAuthDto
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public AppEnvironmentEnum DeviceType { get; set; }

    /// <summary>
    /// 应用编号
    /// </summary>
    public string AppNo { get; set; }

    /// <summary>
    /// 租户编号
    /// </summary>
    public string TenantNo { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    public string EmployeeNo { get; set; }

    /// <summary>
    /// 角色Id集合
    /// </summary>
    public List<long> RoleIdList { get; set; } = new();

    /// <summary>
    /// 角色名称集合
    /// </summary>
    public List<string> RoleNameList { get; set; } = new();

    /// <summary>
    /// 角色类型
    /// </summary>
    public RoleTypeEnum RoleType { get; set; }

    /// <summary>
    /// 数据范围类型
    /// </summary>
    public DataScopeTypeEnum DataScopeType { get; set; }

    /// <summary>
    /// 自定义数据范围部门Id集合
    /// </summary>
    public List<long> DataScopeDepartmentIdList { get; set; } = new();

    /// <summary>
    /// 菜单编码集合
    /// </summary>
    public List<string> MenuCodeList { get; set; } = new();

    /// <summary>
    /// 按钮编码集合
    /// </summary>
    public List<string> ButtonCodeList { get; set; } = new();
}
