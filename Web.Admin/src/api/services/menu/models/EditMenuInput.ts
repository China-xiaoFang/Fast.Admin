import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { EditMenuButtonDto } from "./EditMenuButtonDto";
import { EditionEnum } from "@/api/enums/EditionEnum";
import { MenuTypeEnum } from "@/api/enums/MenuTypeEnum";
import { YesOrNotEnum } from "@/api/enums/YesOrNotEnum";

/**
 * Fast.Center.Service.Menu.Dto.EditMenuInput 编辑菜单输入
 */
export interface EditMenuInput {
  /**
   * 菜单Id
   */
  menuId?: number;
  /**
   * 
   */
  status?: CommonStatusEnum;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
  /**
   * 按钮信息
   */
  buttonList?: Array<EditMenuButtonDto>;
  /**
   * 
   */
  edition?: EditionEnum;
  /**
   * 模块Id
   */
  moduleId?: number;
  /**
   * 菜单编码
   */
  menuCode?: string;
  /**
   * 菜单名称
   */
  menuName?: string;
  /**
   * 菜单标题
   */
  menuTitle?: string;
  /**
   * 父级Id
   */
  parentId?: number;
  /**
   * 
   */
  menuType?: MenuTypeEnum;
  /**
   * 是否桌面端
   */
  hasDesktop?: boolean;
  /**
   * 桌面端图标
   */
  desktopIcon?: string;
  /**
   * 桌面端路由地址
   */
  desktopRouter?: string;
  /**
   * 是否Web端
   */
  hasWeb?: boolean;
  /**
   * Web端图标
   */
  webIcon?: string;
  /**
   * Web端路由地址
   */
  webRouter?: string;
  /**
   * Web端组件地址
   */
  webComponent?: string;
  /**
   * 是否移动端
   */
  hasMobile?: boolean;
  /**
   * 移动端图标
   */
  mobileIcon?: string;
  /**
   * 移动端路由地址
   */
  mobileRouter?: string;
  /**
   * 内链/外链地址
   */
  link?: string;
  /**
   * 
   */
  visible?: YesOrNotEnum;
  /**
   * 排序
   */
  sort?: number;
}

