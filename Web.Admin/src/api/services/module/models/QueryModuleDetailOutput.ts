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
   * 部门名称
   */
  departmentName?: string;
  /**
   * 创建人名称
   */
  createdUserName?: string;
  /**
   * 创建时间
   */
  createdTime?: Date;
  /**
   * 更新人名称
   */
  updatedUserName?: string;
  /**
   * 更新时间
   */
  updatedTime?: Date;
  /**
   * 行版本
   */
  rowVersion?: number;
}

