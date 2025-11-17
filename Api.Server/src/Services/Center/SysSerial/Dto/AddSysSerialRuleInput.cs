using Fast.Center.Enum;

namespace Fast.Center.Service.SysSerial.Dto;

/// <summary>
/// <see cref="AddSysSerialRuleInput"/> 添加系统序号规则输入
/// </summary>
public class AddSysSerialRuleInput
{
    /// <summary>
    /// 规则类型
    /// </summary>
    [EnumRequired(ErrorMessage = "规则类型不能为空")]
    public SysSerialRuleTypeEnum RuleType { get; set; }

    /// <summary>
    /// 前缀
    /// </summary>
    [MaxLength(5, ErrorMessage = "前缀最长为5个字符")]
    public string Prefix { get; set; }

    /// <summary>
    /// 时间类型
    /// </summary>
    [EnumRequired(ErrorMessage = "时间类型不能为空")]
    public SysSerialDateTypeEnum DateType { get; set; }

    /// <summary>
    /// 分隔符
    /// </summary>
    [EnumRequired(ErrorMessage = "分隔符不能为空")]
    public SysSerialSpacerEnum Spacer { get; set; }

    /// <summary>
    /// 长度
    /// </summary>
    [IntRequired(ErrorMessage = "长度不能为空")]
    public int Length { get; set; }
}