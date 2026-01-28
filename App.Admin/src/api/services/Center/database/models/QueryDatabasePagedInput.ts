import { DatabaseTypeEnum } from "@/api/enums/DatabaseTypeEnum";
import { SugarDbType } from "@/api/enums/SugarDbType";

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
  dbType?: SugarDbType;
  /**
   * 租户Id
   */
  tenantId?: number;
  /**
   * 
   */
  readonly isOrderBy?: boolean;
}

