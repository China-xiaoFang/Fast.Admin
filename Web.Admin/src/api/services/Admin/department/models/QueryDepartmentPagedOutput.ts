/**
 * 获取部门分页列表输出
 */
export interface QueryDepartmentPagedOutput {
	/**
	 * 部门Id
	 */
	departmentId?: string;
	/**
	 * 机构Id
	 */
	orgId?: string;
	/**
	 * 机构名称
	 */
	orgName?: string;
	/**
	 * 父级Id
	 */
	parentId?: string;
	/**
	 * 父级名称
	 */
	parentName?: string;
	/**
	 * 父级Id集合
	 */
	parentIds?: string[];
	/**
	 * 父级名称集合
	 */
	parentNames?: string[];
	/**
	 * 部门名称
	 */
	departmentName?: string;
	/**
	 * 部门编码
	 */
	departmentCode?: string;
	/**
	 * 联系人
	 */
	contacts?: string;
	/**
	 * 电话
	 */
	phone?: string;
	/**
	 * 邮箱
	 */
	email?: string;
	/**
	 * 排序
	 */
	sort?: number;
	/**
	 * 数据公开
	 */
	dataPublic?: boolean;
	/**
	 * 备注
	 */
	remark?: string;
	/**
	 * 创建者用户名称
	 */
	createdUserName?: string;
	/**
	 * 创建时间
	 */
	createdTime?: string;
	/**
	 * 更新者用户名称
	 */
	updatedUserName?: string;
	/**
	 * 更新时间
	 */
	updatedTime?: string;
	/**
	 * 更新版本控制字段
	 */
	rowVersion?: string;
	/**
	 * 子级
	 */
	children?: QueryDepartmentPagedOutput[];
}

