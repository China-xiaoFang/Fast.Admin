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
/// <see cref="QueryPlatformPagedOutput"/> 获取平台分页列表输出
/// </summary>
public class QueryPlatformPagedOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 平台编号
    /// </summary>
    public string PlatformNo { get; set; }

    /// <summary>
    /// 平台名称
    /// </summary>
    public string PlatformName { get; set; }

    /// <summary>
    /// 平台简称
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 平台管理员名称
    /// </summary>
    public string AdminName { get; set; }

    /// <summary>
    /// 平台管理员手机
    /// </summary>
    public string AdminMobile { get; set; }

    /// <summary>
    /// LogoUrl
    /// </summary>
    public string LogoUrl { get; set; }

    /// <summary>
    /// 开通时间
    /// </summary>
    public DateTime ActivationTime { get; set; }

    /// <summary>
    /// 平台版本
    /// </summary>
    public EditionEnum Edition { get; set; }

    /// <summary>
    /// 自动续费
    /// </summary>
    public bool AutoRenewal { get; set; }

    /// <summary>
    /// 续费到期时间
    /// </summary>
    /// <remarks>包含当天</remarks>
    public DateTime RenewalExpiryTime { get; set; }

    /// <summary>
    /// 是否试用平台（未正式上线）
    /// </summary>
    /// <remarks>ture表示可以删除所有数据重新初始化</remarks>
    public bool IsTrial { get; set; }

    /// <summary>
    /// 是否已初始化
    /// </summary>
    public bool IsInitialized { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 更新者用户名称
    /// </summary>
    public string UpdatedUserName { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdatedTime { get; set; }
}