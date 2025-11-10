import { AuthMenuInfoDto } from "./AuthMenuInfoDto";

/**
 * Fast.Admin.Service.Auth.Dto.AuthModuleInfoDto 授权模块信息Dto
 */
export interface AuthModuleInfoDto {
  /**
   * 模块Id
   */
  id?: number;
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
   * 菜单集合
   */
  children?: Array<AuthMenuInfoDto>;
}

