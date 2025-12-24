import { PagedInput } from "fast-element-plus";
import { DatabaseTypeEnum } from "@/api/enums/DatabaseTypeEnum";
import { DbType } from "@/api/enums/DbType";

/**
 * Fast.Center.Service.Database.Dto.QueryDatabasePagedInput 获取数据库分页列表输入
 */
export interface QueryDatabasePagedInput extends PagedInput  {
  /**
   * 
   */
  databaseType?: DatabaseTypeEnum;
  /**
   * 
   */
  dbType?: DbType;
  /**
   * 租户Id
   */
  tenantId?: number;
}

