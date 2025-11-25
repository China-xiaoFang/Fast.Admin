import { SysSerialRuleTypeEnum } from "@/api/enums/SysSerialRuleTypeEnum";
import { SerialDateTypeEnum } from "@/api/enums/SerialDateTypeEnum";
import { SerialSpacerEnum } from "@/api/enums/SerialSpacerEnum";

/**
 * Fast.Center.Service.SysSerial.Dto.QuerySysSerialRuleDetailOutput  获取系统序号规则详情输出
 */
export interface QuerySysSerialRuleDetailOutput {
  /**
   * 序号规则Id
   */
  serialRuleId?: number;
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
  /**
   * 
   */
  departmentName?: string;
  /**
   * 
   */
  createdUserName?: string;
  /**
   * 
   */
  createdTime?: Date;
  /**
   * 
   */
  updatedUserName?: string;
  /**
   * 
   */
  updatedTime?: Date;
  /**
   * 
   */
  rowVersion?: number;
}

