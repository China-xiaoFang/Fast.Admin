import { SysSerialRuleTypeEnum } from "@/api/enums/SysSerialRuleTypeEnum";
import { SerialDateTypeEnum } from "@/api/enums/SerialDateTypeEnum";
import { SerialSpacerEnum } from "@/api/enums/SerialSpacerEnum";

/**
 * Fast.Center.Service.SysSerial.Dto.AddSysSerialRuleInput 添加系统序号规则输入
 */
export interface AddSysSerialRuleInput {
  /**
   * 
   */
  ruleType?: SysSerialRuleTypeEnum;
  /**
   * 前缀
   */
  prefix?: string;
  /**
   * 
   */
  dateType?: SerialDateTypeEnum;
  /**
   * 
   */
  spacer?: SerialSpacerEnum;
  /**
   * 长度
   */
  length?: number;
}

