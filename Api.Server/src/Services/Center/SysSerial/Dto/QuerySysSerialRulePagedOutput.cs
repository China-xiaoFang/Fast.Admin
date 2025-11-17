using Fast.Center.Enum;

namespace Fast.Center.Service.SysSerial.Dto;

/// <summary>
/// <see cref="QuerySysSerialRulePagedOutput"/>  获取系统序号规则分页列表输出
/// </summary>
public class QuerySysSerialRulePagedOutput
{
    /// <summary>
    /// 序号规则Id
    /// </summary>
    public long SerialRuleId { get; set; }

    /// <summary>
    /// 规则类型
    /// </summary>
    public SysSerialRuleTypeEnum RuleType { get; set; }

    /// <summary>
    /// 前缀
    /// </summary>
    public string Prefix { get; set; }

    /// <summary>
    /// 时间类型
    /// </summary>
    public SysSerialDateTypeEnum DateType { get; set; }

    /// <summary>
    /// 分隔符
    /// </summary>
    public SysSerialSpacerEnum Spacer { get; set; }

    /// <summary>
    /// 长度
    /// </summary>
    public int Length { get; set; }

    /// <summary>
    /// 最后一个序号
    /// </summary>
    public long? LastSerial { get; set; }

    /// <summary>
    /// 最后一个序号编号
    /// </summary>
    public string LastSerialNo { get; set; }

    /// <summary>
    /// 最后一个序号生成时间
    /// </summary>
    public DateTime? LastTime { get; set; }

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