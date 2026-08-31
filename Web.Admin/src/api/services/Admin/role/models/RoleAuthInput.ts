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
	menuIds?: string[];
	/**
	 * 按钮Id集合
	 */
	buttonIds?: string[];
	/**
	 * 角色Id
	 */
	roleId?: string;
	/**
	 * 
	 */
	rowVersion?: string;
}

