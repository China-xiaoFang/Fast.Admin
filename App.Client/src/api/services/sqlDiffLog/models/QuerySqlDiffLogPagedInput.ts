import { DiffLogTypeEnum } from "@/api/enums/DiffLogTypeEnum";

/**
 * Fast.Center.Service.SqlDiffLog.Dto.QuerySqlDiffLogPagedInput 获取Sql差异日志分页列表输入
 */
export interface QuerySqlDiffLogPagedInput extends PagedInput  {
  /**
   * 账号Id
   */
  accountId?: number;
  /**
   * 
   */
  diffType?: DiffLogTypeEnum;
  /**
   * 租户Id
   */
  tenantId?: number;
}

