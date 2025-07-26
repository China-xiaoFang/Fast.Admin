﻿// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

// ReSharper disable once CheckNamespace

namespace Fast.Core;

/// <summary>
/// <see cref="AuthUserInfo"/> 授权用户信息
/// </summary>
[SuppressSniffer]
public class AuthUserInfo
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public virtual AppEnvironmentEnum DeviceType { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    public virtual string DeviceId { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    public virtual long AppId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public virtual string AppName { get; set; }

    /// <summary>
    /// 账号Id
    /// </summary>
    public virtual long AccountId { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    public virtual string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public virtual string NickName { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public virtual long TenantId { get; set; }

    /// <summary>
    /// 租户编号
    /// </summary>
    public virtual string TenantNo { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    public virtual long UserId { get; set; }

    /// <summary>
    /// 账户
    /// </summary>
    public virtual string Account { get; set; }

    /// <summary>
    /// 职员Id
    /// </summary>
    public virtual long EmployeeId { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    public virtual string EmployeeNo { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public virtual string EmployeeName { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    public virtual long DepartmentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    public virtual string DepartmentName { get; set; }

    /// <summary>
    /// 是否超级管理员
    /// </summary>
    public virtual bool IsSuperAdmin { get; set; }

    /// <summary>
    /// 是否管理员
    /// </summary>
    public virtual bool IsAdmin { get; set; }

    /// <summary>
    /// 最后登录设备
    /// </summary>
    public virtual string LastLoginDevice { get; set; }

    /// <summary>
    /// 最后登录操作系统（版本）
    /// </summary>
    public virtual string LastLoginOS { get; set; }

    /// <summary>
    /// 最后登录浏览器（版本）
    /// </summary>
    public virtual string LastLoginBrowser { get; set; }

    /// <summary>
    /// 最后登录省份
    /// </summary>
    public virtual string LastLoginProvince { get; set; }

    /// <summary>
    /// 最后登录城市
    /// </summary>
    public virtual string LastLoginCity { get; set; }

    /// <summary>
    /// 最后登录Ip
    /// </summary>
    public virtual string LastLoginIp { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public virtual DateTime LastLoginTime { get; set; }

    /// <summary>
    /// 角色Id集合
    /// </summary>
    public virtual List<long> RoleIdList { get; set; } = new();

    /// <summary>
    /// 角色名称集合
    /// </summary>
    public List<string> RoleNameList { get; set; } = new();

    /// <summary>
    /// 菜单编码集合
    /// </summary>
    public virtual List<string> MenuCodeList { get; set; } = new();

    /// <summary>
    /// 按钮编码集合
    /// </summary>
    public virtual List<string> ButtonCodeList { get; set; } = new();
}