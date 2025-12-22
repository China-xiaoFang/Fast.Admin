/**
 * Fast.Center.Service.Database.Dto.QueryMainDatabasePagedOutput 获取主库模板分页列表输出
 */
export interface QueryMainDatabasePagedOutput {
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
   * 状态
   */
  status?: number;
  /**
   * 备注
   */
  remark?: string;
  /**
   * 创建时间
   */
  createdTime?: Date;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
}
