import { EditionEnum } from "@/api/enums/EditionEnum";
import { MenuTypeEnum } from "@/api/enums/MenuTypeEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * Fast.Center.Service.Menu.Dto.QueryMenuPagedInput 获取菜单列表输入
 */
export interface QueryMenuPagedInput extends PagedInput  {
  /**
   * 
   */
  edition?: EditionEnum;
  /**
   * 应用Id
   */
  appId?: number;
  /**
   * 模块Id
   */
  moduleId?: number;
  /**
   * 
   */
  menuType?: MenuTypeEnum;
  /**
   * 是否桌面端
   */
  hasDesktop?: boolean;
  /**
   * 是否Web端
   */
  hasWeb?: boolean;
  /**
   * 是否移动端
   */
  hasMobile?: boolean;
  /**
   * 是否显示
   */
  visible?: boolean;
  /**
   * 
   */
  status?: CommonStatusEnum;
}

