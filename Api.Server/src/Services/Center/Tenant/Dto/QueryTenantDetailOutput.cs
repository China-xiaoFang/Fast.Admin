// ------------------------------------------------------------------------
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fast.Center.Enum;

namespace Fast.Center.Service.Tenant.Dto;

/// <summary>
/// <see cref="QueryTenantDetailOutput"/> 获取租户详情输出
/// </summary>
public class QueryTenantDetailOutput : PagedOutput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public long TenantId { get; set; }

    /// <summary>
    /// 租户编号
    /// </summary>
    public string TenantNo { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    /// <remarks>单号生成前缀</remarks>
    public string TenantCode { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 租户简称
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// 租户英文名称
    /// </summary>
    /// <remarks>根据 <see cref="TenantName"/> 生成的拼音</remarks>
    public string SpellName { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 租户管理员账号Id
    /// </summary>
    public long AdminAccountId { get; set; }

    /// <summary>
    /// 租户管理员名称
    /// </summary>
    public string AdminName { get; set; }

    /// <summary>
    /// 租户管理员手机
    /// </summary>
    public string AdminMobile { get; set; }

    /// <summary>
    /// 租户管理员邮箱
    /// </summary>
    public string AdminEmail { get; set; }

    /// <summary>
    /// 租户管理员电话
    /// </summary>
    public string AdminPhone { get; set; }

    /// <summary>
    /// 租户机器人名称
    /// </summary>
    public string RobotName { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    public TenantTypeEnum TenantType { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    public string LogoUrl { get; set; }

    /// <summary>
    /// 允许删除数据
    /// </summary>
    public bool AllowDeleteData { get; set; }

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
}
