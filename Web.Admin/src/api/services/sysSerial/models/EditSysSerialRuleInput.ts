import { SysSerialDateTypeEnum } from "@/api/enums/SysSerialDateTypeEnum";
import { SysSerialSpacerEnum } from "@/api/enums/SysSerialSpacerEnum";

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
  dateType?: SysSerialDateTypeEnum;
  /**
   * 
   */
  spacer?: SysSerialSpacerEnum;
  /**
   * 长度
   */
  length?: number;
  /**
   * 
   */
  rowVersion?: number;
}

