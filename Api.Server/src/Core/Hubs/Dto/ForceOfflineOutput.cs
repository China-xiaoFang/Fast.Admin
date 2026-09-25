// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Core;

/// <summary>
/// 强制下线输出
/// </summary>
public class ForceOfflineOutput
{
    /// <summary>
    /// 是否为管理员
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    public string EmployeeNo { get; set; }

    /// <summary>
    /// 下线时间
    /// </summary>
    public DateTime OfflineTime { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; }
}
