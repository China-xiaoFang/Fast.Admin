// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Center.Domain;

namespace Fast.Center.Service.Tenant.Dto;

/// <summary>
/// 获取租户分页列表输入
/// </summary>
public class QueryTenantPagedInput : PagedInput
{
    /// <summary>
    /// 状态
    /// </summary>
    public CommonStatusEnum? Status { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public EditionEnum? Edition { get; set; }

    /// <summary>
    /// 租户管理员手机
    /// </summary>
    public string AdminMobile { get; set; }

    /// <summary>
    /// 租户管理员邮箱
    /// </summary>
    public string AdminEmail { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    public TenantTypeEnum? TenantType { get; set; }
}
