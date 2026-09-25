// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.TenantOnlineUser.Dto;

/// <summary>
/// 获取在线用户分页列表输入
/// </summary>
public class QueryTenantOnlineUserPagedInput : PagedInput
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public AppEnvironmentEnum? DeviceType { get; set; }

    /// <summary>
    /// 账号Id
    /// </summary>
    public long? AccountId { get; set; }

    /// <summary>
    /// 职员Id
    /// </summary>
    [SugarColumn(ColumnDescription = "职员Id")]
    public long? EmployeeId { get; set; }
}
