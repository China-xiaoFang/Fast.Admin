import { EditionEnum } from "@/api/enums/EditionEnum";
import { MenuTypeEnum } from "@/api/enums/MenuTypeEnum";
import { YesOrNotEnum } from "@/api/enums/YesOrNotEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * Fast.Center.Service.Menu.Dto.QueryMenuPagedOutput 获取菜单列表输出
 */
export interface QueryMenuPagedOutput {
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
  /**
   * 
   */
  status?: CommonStatusEnum;
  /**
   * 子节点
   */
  children?: Array<QueryMenuPagedOutput>;
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

