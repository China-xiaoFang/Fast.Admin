import { DatabaseTypeEnum } from "@/api/enums/DatabaseTypeEnum";
import { DbType } from "@/api/enums/DbType";
import { QuerySlaveDatabaseOutput } from "./QuerySlaveDatabaseOutput";

/**
 * Fast.Center.Service.Database.Dto.QueryDatabaseDetailOutput 获取数据库详情输出
 */
export interface QueryDatabaseDetailOutput {
  /**
   * 主库Id
   */
  mainId?: number;
  /**
   * 
   */
  databaseType?: DatabaseTypeEnum;
  /**
   * 
   */
  dbType?: DbType;
  /**
   * 公网Ip地址
   */
  publicIp?: string;
  /**
   * 内网Ip地址
   */
  intranetIp?: string;
  /**
   * 端口号
   */
  port?: number;
  /**
   * 数据库名称
   */
  dbName?: string;
  /**
   * 数据库用户
   */
  dbUser?: string;
  /**
   * 数据库密码
   */
  dbPwd?: string;
  /**
   * 自定义连接字符串
   */
  customConnectionStr?: string;
  /**
   * 超时时间，单位秒
   */
  commandTimeOut?: number;
  /**
   * SqlSugar Sql执行最大秒数，如果超过记录警告日志
   */
  sugarSqlExecMaxSeconds?: number;
  /**
   * 差异日志
   */
  diffLog?: boolean;
  /**
   * 禁用 SqlSugar 的 Aop
   */
  disableAop?: boolean;
  /**
   * 是否初始化
   */
  isInitialized?: boolean;
  /**
   * 从数据库集合
   */
  children?: Array<QuerySlaveDatabaseOutput>;
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

