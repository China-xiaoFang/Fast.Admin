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

using Fast.FastCloud.Enum;

namespace Fast.FastCloud.Service.Platform.Dto;

/// <summary>
/// <see cref="FirstActivationPlatformInput"/> 初次开通平台输入
/// </summary>
public class FirstActivationPlatformInput
{
    /// <summary>
    /// 平台编号
    /// </summary>
    [StringRequired(ErrorMessage = "平台编号不能为空")]
    public string PlatformNo { get; set; }

    /// <summary>
    /// 平台名称
    /// </summary>
    [StringRequired(ErrorMessage = "平台名称不能为空")]
    public string PlatformName { get; set; }

    /// <summary>
    /// 平台简称
    /// </summary>
    [StringRequired(ErrorMessage = "平台简称不能为空")]
    public string ShortName { get; set; }

    /// <summary>
    /// 平台管理员名称
    /// </summary>
    [StringRequired(ErrorMessage = "平台管理员名称不能为空")]
    public string AdminName { get; set; }

    /// <summary>
    /// 平台管理员手机
    /// </summary>
    [StringRequired(ErrorMessage = "平台管理员手机不能为空")]
    public string AdminMobile { get; set; }

    /// <summary>
    /// 平台管理员邮箱
    /// </summary>
    [StringRequired(ErrorMessage = "平台管理员邮箱不能为空")]
    public string AdminEmail { get; set; }

    /// <summary>
    /// 平台管理员电话
    /// </summary>
    public string AdminPhone { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    public string LogoUrl { get; set; }

    /// <summary>
    /// 平台版本
    /// </summary>
    [EnumRequired(ErrorMessage = "平台版本不能为空")]
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 续费时长
    /// </summary>
    public RenewalDurationEnum Duration { get; set; }

    /// <summary>
    /// 续费金额
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// 数据库类型，用于区分使用的是那个类型的数据库
    /// </summary>
    [EnumRequired(ErrorMessage = "数据库类型不能为空")]
    public DbType DbType { get; set; }

    /// <summary>
    /// 公网Ip地址
    /// </summary>
    [StringRequired(ErrorMessage = "公网Ip地址不能为空")]
    public string PublicIp { get; set; }

    /// <summary>
    /// 内网Ip地址
    /// </summary>
    [StringRequired(ErrorMessage = "内网Ip地址不能为空")]
    public string IntranetIp { get; set; }

    /// <summary>
    /// 端口号
    /// </summary>
    [IntRequired(ErrorMessage = "端口号不能为空")]
    public int Port { get; set; }

    /// <summary>
    /// 数据库名称
    /// </summary>
    [StringRequired(ErrorMessage = "数据库名称不能为空")]
    public string DbName { get; set; }

    /// <summary>
    /// 数据库用户
    /// </summary>
    [StringRequired(ErrorMessage = "数据库用户不能为空")]
    public string DbUser { get; set; }

    /// <summary>
    /// 数据库密码
    /// </summary>
    [StringRequired(ErrorMessage = "数据库密码不能为空")]
    public string DbPwd { get; set; }

    /// <summary>
    /// 自定义连接字符串
    /// </summary>
    public string CustomConnectionStr { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}