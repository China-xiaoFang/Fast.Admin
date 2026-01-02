import { SugarParameter } from "./SugarParameter";

/**
 * Fast.CenterLog.Entity.SqlExecutionLogModel Sql执行日志Model类
 */
export interface SqlExecutionLogModel {
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
   * 原始Sql
   */
  rawSql?: string;
  /**
   * Sql参数
   */
  parameters?: Array<SugarParameter>;
  /**
   * 纯Sql，参数化之后的Sql
   */
  pureSql?: string;
  /**
   * 执行时间
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

