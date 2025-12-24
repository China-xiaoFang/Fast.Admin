import { ModuleViewTypeEnum } from "@/api/enums/ModuleViewTypeEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * Fast.Center.Service.Module.Dto.EditModuleInput 编辑模块输入
 */
export interface EditModuleInput {
  /**
   * 模块Id
   */
  moduleId?: number;
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
   * 查看类型
   */
  viewType?: ModuleViewTypeEnum;
  /**
   * 排序
   */
  sort?: number;
  /**
   * 状态
   */
  status?: CommonStatusEnum;
  /**
   * 行版本
   */
  rowVersion?: number;
}

