import { VisitTypeEnum } from "@/api/enums/VisitTypeEnum";

/**
 * Fast.CenterLog.Entity.VisitLogModel 访问日志Model类
 */
export interface VisitLogModel {
  /**
   * 记录Id
   */
  recordId?: number;
  /**
   * 账号Id
   */
  accountId?: number;
  /**
   * 账号
   */
  account?: string;
  /**
   * 手机
   */
  mobile?: string;
  /**
   * 昵称
   */
  nickName?: string;
  /**
   * 
   */
  visitType?: VisitTypeEnum;
  /**
   * 访问时间
   */
  createdTime?: Date;
  /**
   * 租户Id
   */
  tenantId?: number;
  /**
   * 租户名称
   */
  tenantName?: string;
  /**
   * 
   */
  device?: string;
  /**
   * 
   */
  os?: string;
  /**
   * 
   */
  browser?: string;
  /**
   * 
   */
  province?: string;
  /**
   * 
   */
  city?: string;
  /**
   * 
   */
  ip?: string;
  /**
   * 
   */
  departmentId?: number;
  /**
   * 
   */
  departmentName?: string;
  /**
   * 
   */
  createdUserId?: number;
  /**
   * 
   */
  createdUserName?: string;
}

