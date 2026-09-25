// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.Account.Dto;

/// <summary>
/// 获取账号分页列表输出
/// </summary>
public class QueryAccountPagedOutput
{
    /// <summary>
    /// 账号Id
    /// </summary>
    public long AccountId { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    [SugarSearchValue]
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [SugarSearchValue]
    public string Email { get; set; }

    /// <summary>
    /// 身份验证
    /// </summary>
    public bool IdentityVerification { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarSearchValue]
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 初次登录租户
    /// </summary>
    public string FirstLoginTenantName { get; set; }

    /// <summary>
    /// 初次登录设备
    /// </summary>
    public string FirstLoginDevice { get; set; }

    /// <summary>
    /// 初次登录操作系统（版本）
    /// </summary>
    public string FirstLoginOS { get; set; }

    /// <summary>
    /// 初次登录浏览器（版本）
    /// </summary>
    public string FirstLoginBrowser { get; set; }

    /// <summary>
    /// 初次登录省份
    /// </summary>
    public string FirstLoginProvince { get; set; }

    /// <summary>
    /// 初次登录城市
    /// </summary>
    public string FirstLoginCity { get; set; }

    /// <summary>
    /// 初次登录Ip
    /// </summary>
    public string FirstLoginIp { get; set; }

    /// <summary>
    /// 初次登录时间
    /// </summary>
    public DateTime? FirstLoginTime { get; set; }

    /// <summary>
    /// 最后登录租户
    /// </summary>
    public string LastLoginTenantName { get; set; }

    /// <summary>
    /// 最后登录设备
    /// </summary>
    public string LastLoginDevice { get; set; }

    /// <summary>
    /// 最后登录操作系统（版本）
    /// </summary>
    public string LastLoginOS { get; set; }

    /// <summary>
    /// 最后登录浏览器（版本）
    /// </summary>
    public string LastLoginBrowser { get; set; }

    /// <summary>
    /// 最后登录省份
    /// </summary>
    public string LastLoginProvince { get; set; }

    /// <summary>
    /// 最后登录城市
    /// </summary>
    public string LastLoginCity { get; set; }

    /// <summary>
    /// 最后登录Ip
    /// </summary>
    public string LastLoginIp { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 密码错误次数
    /// </summary>
    public int? PasswordErrorTime { get; set; }

    /// <summary>
    /// 锁定开始时间
    /// </summary>
    public DateTime? LockStartTime { get; set; }

    /// <summary>
    /// 锁定结束时间
    /// </summary>
    public DateTime? LockEndTime { get; set; }

    /// <summary>
    /// 是否锁定
    /// </summary>
    public bool IsLock { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedTime { get; set; }

    /// <summary>
    /// 更新版本控制字段
    /// </summary>
    public long RowVersion { get; set; }
}
