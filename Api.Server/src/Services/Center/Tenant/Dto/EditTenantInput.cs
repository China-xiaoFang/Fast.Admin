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

namespace Fast.Center.Service.Tenant.Dto;

public class EditTenantInput : UpdateVersionInput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [LongRequired(ErrorMessage = "租户Id不能为空")]
    public long TenantId { get; set; }

    /// <summary>
    /// 租户编码
    /// </summary>
    [StringRequired(ErrorMessage = "租户编码不能为空"), MaxLength(5, ErrorMessage = "租户编码不能超过5个字符")]
    public string TenantCode { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [EnumRequired(ErrorMessage = "状态不能为空")]
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    [StringRequired(ErrorMessage = "租户名称不能为空")]
    public string TenantName { get; set; }

    /// <summary>
    /// 租户简称
    /// </summary>
    [StringRequired(ErrorMessage = "租户简称不能为空")]
    public string ShortName { get; set; }

    /// <summary>
    /// 租户英文名称
    /// </summary>
    [StringRequired(ErrorMessage = "租户英文名称不能为空")]
    public string SpellName { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    [EnumRequired(ErrorMessage = "版本不能为空", AllowZero = true)]
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 租户管理员名称
    /// </summary>
    [StringRequired(ErrorMessage = "租户管理员名称不能为空")]
    public string AdminName { get; set; }

    /// <summary>
    /// 租户管理员手机
    /// </summary>
    [StringRequired(ErrorMessage = "租户管理员手机不能为空")]
    [RegularExpression(RegexConst.Mobile, ErrorMessage = "手机格式不正确")]
    public string AdminMobile { get; set; }

    /// <summary>
    /// 租户管理员邮箱
    /// </summary>
    [StringRequired(ErrorMessage = "租户管理员邮箱不能为空")]
    [RegularExpression(RegexConst.EmailAddress, ErrorMessage = "邮箱格式不正确")]
    public string AdminEmail { get; set; }

    /// <summary>
    /// 租户管理员电话
    /// </summary>
    public string AdminPhone { get; set; }

    /// <summary>
    /// 租户机器人名称
    /// </summary>
    [StringRequired(ErrorMessage = "租户机器人名称不能为空")]
    public string RobotName { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    [StringRequired(ErrorMessage = "LogoUrl不能为空")]
    public string LogoUrl { get; set; }
}