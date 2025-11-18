using Fast.Center.Enum;

namespace Fast.Center.Service.SysSerial.Dto;

/// <summary>
/// <see cref="QuerySysSerialRuleDetailOutput"/>  获取系统序号规则详情输出
/// </summary>
public class QuerySysSerialRuleDetailOutput : PagedOutput
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
}