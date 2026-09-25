// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.AdminLog.Domain;

namespace Fast.Admin.Service.OperateLog.Dto;

/// <summary>
/// 获取操作日志分页列表输入
/// </summary>
public class QueryOperateLogPagedInput : PagedInput
{
    /// <summary>
    /// 职员Id
    /// </summary>
    public long? EmployeeId { get; set; }

    /// <summary>
    /// 操作类型
    /// </summary>
    [SugarColumn(ColumnDescription = "操作类型")]
    public OperateLogTypeEnum? OperateType { get; set; }

    /// <summary>
    /// 业务单号
    /// </summary>
    public long? BizId { get; set; }
}
