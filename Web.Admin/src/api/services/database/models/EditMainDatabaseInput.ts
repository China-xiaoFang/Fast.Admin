/**
 * Fast.Center.Service.Database.Dto.EditMainDatabaseInput 编辑主库模板输入
 */
export interface EditMainDatabaseInput {
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
   * 备注
   */
  remark?: string;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
}
