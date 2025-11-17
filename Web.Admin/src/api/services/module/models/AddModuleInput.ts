import { ModuleViewTypeEnum } from "@/api/enums/ModuleViewTypeEnum";

/**
 * Fast.Center.Service.Menu.Dto.AddModuleInput 添加模块输入
 */
export interface AddModuleInput {
  /**
   * 应用Id
   */
  appId?: number;
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
}

