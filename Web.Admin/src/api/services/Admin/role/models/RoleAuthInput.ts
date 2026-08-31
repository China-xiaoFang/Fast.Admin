/**
 * 角色授权输入
 */
export interface RoleAuthInput {
	/**
	 * 角色名称
	 */
	roleName?: string;
	/**
	 * 菜单Id集合
	 */
	menuIds?: number[];
	/**
	 * 按钮Id集合
	 */
	buttonIds?: number[];
	/**
	 * 角色Id
	 */
	roleId?: number;
	/**
	 * 
	 */
	rowVersion?: number;
}

