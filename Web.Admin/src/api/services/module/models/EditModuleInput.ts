import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { ModuleViewTypeEnum } from "@/api/enums/ModuleViewTypeEnum";

/**
 * Fast.Center.Service.Menu.Dto.EditModuleInput 编辑模块输入
 */
export interface EditModuleInput {
  /**
   * 模块Id
   */
  moduleId?: number;
  /**
   * 
   */
  status?: CommonStatusEnum;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
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

