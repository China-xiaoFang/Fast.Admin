import { PagedInput } from "fast-element-plus";

/**
 * Fast.Center.Service.Database.Dto.QueryMainDatabasePagedInput 获取主库模板分页列表输入
 */
export interface QueryMainDatabasePagedInput extends PagedInput {
  /**
   * 数据库名称
   */
  databaseName?: string;
  /**
   * 数据库类型
   */
  dbType?: number;
}
