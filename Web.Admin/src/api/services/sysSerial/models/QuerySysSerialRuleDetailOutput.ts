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
   * 创建者用户名称
   */
  createdUserName?: string;
  /**
   * 创建时间
   */
  createdTime?: Date;
  /**
   * 更新者用户名称
   */
  updatedUserName?: string;
  /**
   * 更新时间
   */
  updatedTime?: Date;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
}

