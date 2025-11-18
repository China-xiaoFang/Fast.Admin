import { ModuleViewTypeEnum } from "@/api/enums/ModuleViewTypeEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * Fast.Center.Service.Module.Dto.QueryModuleDetailOutput 获取模块详情输出
 */
export interface QueryModuleDetailOutput {
  /**
   * 模块Id
   */
  moduleId?: number;
  /**
   * 应用Id
   */
  appId?: number;
  /**
   * 应用名称
   */
  appName?: string;
  /**
   * 模块名称
   */
  moduleName?: string;
  /**
   * 图标
   */
  icon?: string;
  /**
   * 颜色
   */
  color?: string;
  /**
   * 
   */
  viewType?: ModuleViewTypeEnum;
  /**
   * 排序
   */
  sort?: number;
  /**
   * 
   */
  status?: CommonStatusEnum;
  /**
   * 
   */
  departmentId?: number;
  /**
   * 
   */
  departmentName?: string;
  /**
   * 
   */
  createdUserName?: string;
  /**
   * 
   */
  createdTime?: Date;
  /**
   * 
   */
  updatedUserName?: string;
  /**
   * 
   */
  updatedTime?: Date;
  /**
   * 
   */
  rowVersion?: number;
}

