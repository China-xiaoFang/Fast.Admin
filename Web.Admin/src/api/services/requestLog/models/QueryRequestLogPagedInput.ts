import { PagedInput } from "fast-element-plus";
import { YesOrNotEnum } from "@/api/enums/YesOrNotEnum";
import { HttpRequestActionEnum } from "@/api/enums/HttpRequestActionEnum";
import { HttpRequestMethodEnum } from "@/api/enums/HttpRequestMethodEnum";

/**
 * Fast.Center.Service.RequestLog.Dto.QueryRequestLogPagedInput 获取请求日志分页列表输入
 */
export interface QueryRequestLogPagedInput extends PagedInput  {
  /**
   * 账号Id
   */
  accountId?: number;
  /**
   * 
   */
  success?: YesOrNotEnum;
  /**
   * 
   */
  operationAction?: HttpRequestActionEnum;
  /**
   * 
   */
  requestMethod?: HttpRequestMethodEnum;
  /**
   * 租户Id
   */
  tenantId?: number;
}

