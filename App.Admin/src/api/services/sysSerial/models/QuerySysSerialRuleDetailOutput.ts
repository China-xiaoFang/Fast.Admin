import { SysSerialRuleTypeEnum } from "@/api/enums/SysSerialRuleTypeEnum";
import { SysSerialDateTypeEnum } from "@/api/enums/SysSerialDateTypeEnum";
import { SysSerialSpacerEnum } from "@/api/enums/SysSerialSpacerEnum";

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

