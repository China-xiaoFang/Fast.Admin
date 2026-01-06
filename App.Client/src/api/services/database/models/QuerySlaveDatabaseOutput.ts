/**
 * Fast.Center.Service.Database.Dto.QuerySlaveDatabaseOutput 获取从数据库输出
 */
export interface QuerySlaveDatabaseOutput {
  /**
   * 从库Id
   */
  slaveId?: number;
  /**
   * 主库Id
   */
  mainId?: number;
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
   * 从库命中率
   */
  hitRate?: number;
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

