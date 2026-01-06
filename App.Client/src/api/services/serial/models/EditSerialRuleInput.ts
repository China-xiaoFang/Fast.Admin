import { SerialDateTypeEnum } from "@/api/enums/SerialDateTypeEnum";
import { SerialSpacerEnum } from "@/api/enums/SerialSpacerEnum";

/**
 * Fast.Admin.Service.Serial.Dto.EditSerialRuleInput 编辑序号规则输入
 */
export interface EditSerialRuleInput {
  /**
   * 序号规则Id
   */
  serialRuleId?: number;
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
  /**
   * 
   */
  rowVersion?: number;
}

