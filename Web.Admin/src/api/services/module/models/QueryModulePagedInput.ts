import { PagedInput } from "fast-element-plus";
import { ModuleViewTypeEnum } from "@/api/enums/ModuleViewTypeEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * Fast.Center.Service.Menu.Dto.QueryModulePagedInput 获取模块分页列表
 */
export interface QueryModulePagedInput extends PagedInput  {
  /**
   * 应用Id
   */
  appId?: number;
  /**
   * 
   */
  viewType?: ModuleViewTypeEnum;
  /**
   * 
   */
  status?: CommonStatusEnum;
}

