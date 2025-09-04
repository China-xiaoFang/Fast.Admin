import { PagedInput } from "fast-element-plus";

/**
 * Fast.FastCloud.Service.Platform.Dto.QueryPlatformRenewalRecordInput 获取平台续费记录分页列表输入
 */
export interface QueryPlatformRenewalRecordInput extends PagedInput  {
  /**
   * 平台Id
   */
  platformId?: number;
}

