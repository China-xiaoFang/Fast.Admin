import { PagedInput } from "fast-element-plus";

/**
 * Fast.Center.Service.Database.Dto.QuerySlaveDatabasePagedInput 获取从库模板分页列表输入
 */
export interface QuerySlaveDatabasePagedInput extends PagedInput {
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
}
