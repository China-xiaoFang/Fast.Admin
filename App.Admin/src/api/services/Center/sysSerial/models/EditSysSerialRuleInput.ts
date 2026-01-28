import { SerialDateTypeEnum } from "@/api/enums/SerialDateTypeEnum";
import { SerialSpacerEnum } from "@/api/enums/SerialSpacerEnum";

/**
 * Fast.Center.Service.SysSerial.Dto.EditSysSerialRuleInput 编辑系统序号规则输入
 */
export interface EditSysSerialRuleInput {
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

