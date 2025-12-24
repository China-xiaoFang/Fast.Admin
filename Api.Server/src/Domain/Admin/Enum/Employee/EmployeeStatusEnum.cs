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

namespace Fast.Admin.Enum;

/// <summary>
/// <see cref="EmployeeStatusEnum"/> 职员状态枚举
/// </summary>
[Flags]
[FastEnum("职员状态枚举")]
public enum EmployeeStatusEnum : byte
{
    /// <summary>
    /// 无
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("无")]
    None = 0,

    /// <summary>
    /// 临时工
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("临时")]
    Temporary = 1,

    /// <summary>
    /// 试用期
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("试用")]
    Probation = 2,

    /// <summary>
    /// 实习生
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("实习生")]
    Intern = 4,

    /// <summary>
    /// 外包
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("外包")]
    Outsourcing = 8,

    /// <summary>
    /// 挂职
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("挂职")]
    Secondment = 16,

    /// <summary>
    /// 正式
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("正式")]
    Formal = 32,

    /// <summary>
    /// 离职
    /// </summary>
    [TagType(TagTypeEnum.Danger)]
    [Description("离职")]
    Resigned = 64
}