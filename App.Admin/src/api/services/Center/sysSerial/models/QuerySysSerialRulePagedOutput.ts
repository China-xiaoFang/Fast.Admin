import { SysSerialRuleTypeEnum } from "@/api/enums/SysSerialRuleTypeEnum";
import { SerialDateTypeEnum } from "@/api/enums/SerialDateTypeEnum";
import { SerialSpacerEnum } from "@/api/enums/SerialSpacerEnum";

/**
 * Fast.Center.Service.SysSerial.Dto.QuerySysSerialRulePagedOutput  获取系统序号规则分页列表输出
 */
export interface QuerySysSerialRulePagedOutput {
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
   * 最后一个序号
   */
  lastSerial?: number;
  /**
   * 最后一个序号编号
   */
  lastSerialNo?: string;
  /**
   * 最后一个序号生成时间
   */
  lastTime?: Date;
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

