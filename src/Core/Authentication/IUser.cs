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
/// <see cref="IUser"/> 授权用户信息
/// </summary>
/// <remarks>作用域注册，保证当前请求管道中是唯一的，并且只会加载一次</remarks>
public interface IUser
{
    /// <summary>
    /// 设备类型
    /// </summary>
    AppEnvironmentEnum DeviceType { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    string DeviceId { get; set; }

    /// <summary>
    /// 应用Id
    /// </summary>
    long AppId { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    string AppName { get; set; }

    /// <summary>
    /// 账号Id
    /// </summary>
    long AccountId { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    string NickName { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    long TenantId { get; set; }

    /// <summary>
    /// 租户编号
    /// </summary>
    string TenantNo { get; set; }

    /// <summary>
    /// 用户Id
    /// </summary>
    long UserId { get; set; }

    /// <summary>
    /// 账户
    /// </summary>
    string Account { get; set; }

    /// <summary>
    /// 职员Id
    /// </summary>
    long EmployeeId { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    string EmployeeNo { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    string EmployeeName { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    long DepartmentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    string DepartmentName { get; set; }

    /// <summary>
    /// 是否超级管理员
    /// </summary>
    bool IsSuperAdmin { get; set; }

    /// <summary>
    /// 是否管理员
    /// </summary>
    bool IsAdmin { get; set; }

    /// <summary>
    /// 最后登录设备
    /// </summary>
    string LastLoginDevice { get; set; }

    /// <summary>
    /// 最后登录操作系统（版本）
    /// </summary>
    string LastLoginOS { get; set; }

    /// <summary>
    /// 最后登录浏览器（版本）
    /// </summary>
    string LastLoginBrowser { get; set; }

    /// <summary>
    /// 最后登录省份
    /// </summary>
    string LastLoginProvince { get; set; }

    /// <summary>
    /// 最后登录城市
    /// </summary>
    string LastLoginCity { get; set; }

    /// <summary>
    /// 最后登录Ip
    /// </summary>
    string LastLoginIp { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    DateTime LastLoginTime { get; set; }

    /// <summary>
    /// 角色Id集合
    /// </summary>
    List<long> RoleIdList { get; set; }

    /// <summary>
    /// 角色名称集合
    /// </summary>
    List<string> RoleNameList { get; set; }

    /// <summary>
    /// 菜单编码集合
    /// </summary>
    List<string> MenuCodeList { get; set; }

    /// <summary>
    /// 按钮编码集合
    /// </summary>
    List<string> ButtonCodeList { get; set; }

    /// <summary>
    /// 设置授权用户
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <remarks>只会赋值一次</remarks>
    void SetAuthUser(AuthUserInfo authUserInfo);

    /// <summary>
    /// 从缓存中获取授权用户信息
    /// </summary>
    /// <param name="deviceType"><see cref="AppEnvironmentEnum"/> 设备类型</param>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <param name="employeeNo"><see cref="string"/> 工号</param>
    /// <returns></returns>
    Task<AuthUserInfo> GetAuthUserInfo(AppEnvironmentEnum deviceType, string tenantNo, string employeeNo);

    /// <summary>
    /// 统一登录
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    Task Login(AuthUserInfo authUserInfo);

    /// <summary>
    /// 刷新登录信息
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    Task Refresh(AuthUserInfo authUserInfo);

    /// <summary>
    /// 统一退出登录
    /// </summary>
    /// <returns></returns>
    Task Logout();
}