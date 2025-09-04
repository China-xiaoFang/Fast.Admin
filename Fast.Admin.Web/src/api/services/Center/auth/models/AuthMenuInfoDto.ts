import { MenuTypeEnum } from "@/api/enums/MenuTypeEnum";
import { YesOrNotEnum } from "@/api/enums/YesOrNotEnum";

/**
 * Fast.Center.Service.Auth.Dto.AuthMenuInfoDto 授权菜单信息Dto
 */
export interface AuthMenuInfoDto {
	/**
	 * 菜单Id
	 */
	id?: number;
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
	 * 图标
	 */
	icon?: string;
	/**
	 * 路由地址
	 */
	router?: string;
	/**
	 * 组件地址
	 */
	component?: string;
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
	 * 子节点
	 */
	children?: Array<AuthMenuInfoDto>;
}
