import { SerialRuleTypeEnum } from "@/api/enums/SerialRuleTypeEnum";
import { SerialDateTypeEnum } from "@/api/enums/SerialDateTypeEnum";
import { SerialSpacerEnum } from "@/api/enums/SerialSpacerEnum";

/**
 * Fast.Admin.Service.Serial.Dto.AddSerialRuleInput 添加序号规则输入
 */
export interface AddSerialRuleInput {
  /**
   * 
   */
  ruleType?: SerialRuleTypeEnum;
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

