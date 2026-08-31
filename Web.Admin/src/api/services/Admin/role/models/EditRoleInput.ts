import type { DataScopeTypeEnum } from "@/api/enums/DataScopeTypeEnum";
import type { RoleTypeEnum } from "@/api/enums/RoleTypeEnum";

/**
 * 编辑角色输入
 */
export interface EditRoleInput {
	/**
	 * 角色Id
	 */
	roleId?: number;
	/**
	 * 
	 */
	roleType?: RoleTypeEnum;
	/**
	 * 是否使用系统菜单
	 */
	isSystemMenu?: boolean;
	/**
	 * 角色名称
	 */
	roleName?: string;
	/**
	 * 角色编码
	 */
	roleCode?: string;
	/**
	 * 排序
	 */
	sort?: number;
	/**
	 * 
	 */
	dataScopeType?: DataScopeTypeEnum;
	/**
	 * 自定义数据范围部门Id集合
	 */
	dataScopeDepartmentIds?: number[];
	/**
	 * 可分配的角色Id集合
	 */
	assignableRoleIds?: number[];
	/**
	 * 备注
	 */
	remark?: string;
	/**
	 * 
	 */
	rowVersion?: number;
}

