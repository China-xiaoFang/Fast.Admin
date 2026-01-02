import { DiffLogTypeEnum } from "@/api/enums/DiffLogTypeEnum";
import { SugarParameter } from "./SugarParameter";

/**
 * Fast.CenterLog.Entity.SqlDiffLogModel Sql差异日志Model类
 */
export interface SqlDiffLogModel {
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
  diffType?: DiffLogTypeEnum;
  /**
   * 表名称
   */
  tableName?: string;
  /**
   * 表描述
   */
  tableDescription?: string;
  /**
   * 业务数据
   */
  businessData?: any;
  /**
   * 旧的列信息
   */
  beforeColumnList?: Array<any>;
  /**
   * 新的列信息
   */
  afterColumnList?: Array<any>;
  /**
   * 执行秒数
   */
  executeSeconds?: number;
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
   * 差异时间
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

