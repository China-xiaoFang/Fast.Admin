import { DatabaseTypeEnum } from "@/api/enums/DatabaseTypeEnum";

/**
 * Fast.Admin.Service.Database.Dto.InitDatabaseInput 同初始化数据库输入
 */
export interface InitDatabaseInput {
  /**
   * 租户Id
   */
  tenantId?: number;
  /**
   * 
   */
  databaseType?: DatabaseTypeEnum;
}

