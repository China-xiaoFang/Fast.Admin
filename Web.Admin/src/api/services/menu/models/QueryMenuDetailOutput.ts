import { EditionEnum } from "@/api/enums/EditionEnum";
import { MenuTypeEnum } from "@/api/enums/MenuTypeEnum";
import { YesOrNotEnum } from "@/api/enums/YesOrNotEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { QueryMenuButtonDetailDto } from "./QueryMenuButtonDetailDto";

/**
 * Fast.Center.Service.Menu.Dto.QueryMenuDetailOutput 获取菜单详情输出
 */
export interface QueryMenuDetailOutput {
  /**
   * 菜单Id
   */
  menuId?: number;
  /**
   * 
   */
  edition?: EditionEnum;
  /**
   * 应用Id
   */
  appId?: number;
  /**
   * 应用名称
   */
  appName?: string;
  /**
   * 模块Id
   */
  moduleId?: number;
  /**
   * 模块名称
   */
  moduleName?: string;
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
  /**
   * 
   */
  status?: CommonStatusEnum;
  /**
   * 创建者用户名称
   */
  createdUserName?: string;
  /**
   * 创建时间
   */
  createdTime?: Date;
  /**
   * 更新者用户名称
   */
  updatedUserName?: string;
  /**
   * 更新时间
   */
  updatedTime?: Date;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
  /**
   * 按钮信息
   */
  buttonList?: Array<QueryMenuButtonDetailDto>;
}

