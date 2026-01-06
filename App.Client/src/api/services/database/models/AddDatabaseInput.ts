import { DatabaseTypeEnum } from "@/api/enums/DatabaseTypeEnum";
import { SugarDbType } from "@/api/enums/SugarDbType";

/**
 * Fast.Center.Service.Database.Dto.AddDatabaseInput 添加数据库输入
 */
export interface AddDatabaseInput {
  /**
   * 
   */
  databaseType?: DatabaseTypeEnum;
  /**
   * 
   */
  dbType?: SugarDbType;
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
   * 租户Id
   */
  tenantId?: number;
  /**
   * 是否创建数据库
   */
  isCreateDatabase?: boolean;
}

