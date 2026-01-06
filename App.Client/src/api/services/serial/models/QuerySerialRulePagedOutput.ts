import { SerialRuleTypeEnum } from "@/api/enums/SerialRuleTypeEnum";
import { SerialDateTypeEnum } from "@/api/enums/SerialDateTypeEnum";
import { SerialSpacerEnum } from "@/api/enums/SerialSpacerEnum";

/**
 * Fast.Admin.Service.Serial.Dto.QuerySerialRulePagedOutput 获取序号规则分页列表输出
 */
export interface QuerySerialRulePagedOutput {
  /**
   * 序号规则Id
   */
  serialRuleId?: number;
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

