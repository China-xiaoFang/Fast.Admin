/**
 * Fast.Center.Service.Database.Dto.QueryMainDatabaseDetailOutput 获取主库模板详情输出
 */
export interface QueryMainDatabaseDetailOutput {
  /**
   * 主库Id
   */
  mainDatabaseId?: number;
  /**
   * 数据库名称
   */
  databaseName?: string;
  /**
   * 数据库类型
   */
  dbType?: number;
  /**
   * 主机地址
   */
  host?: string;
  /**
   * 端口
   */
  port?: number;
  /**
   * 用户名
   */
  userName?: string;
  /**
   * 密码
   */
  password?: string;
  /**
   * 状态
   */
  status?: number;
  /**
   * 备注
   */
  remark?: string;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
}
