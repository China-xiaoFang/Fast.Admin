import { OperateLogTypeEnum } from "@/api/enums/OperateLogTypeEnum";

/**
 * Fast.Center.Service.RequestLog.Dto.QueryOperateLogPagedInput 获取操作日志分页列表输入
 */
export interface QueryOperateLogPagedInput extends PagedInput  {
  /**
   * 职员Id
   */
  employeeId?: number;
  /**
   * 
   */
  operateType?: OperateLogTypeEnum;
}

