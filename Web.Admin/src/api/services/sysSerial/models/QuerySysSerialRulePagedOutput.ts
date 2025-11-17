import { SysSerialRuleTypeEnum } from "@/api/enums/SysSerialRuleTypeEnum";
import { SysSerialDateTypeEnum } from "@/api/enums/SysSerialDateTypeEnum";
import { SysSerialSpacerEnum } from "@/api/enums/SysSerialSpacerEnum";

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
}

