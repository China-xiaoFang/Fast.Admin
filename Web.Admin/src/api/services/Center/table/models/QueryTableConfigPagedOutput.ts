/**
 * 获取表格配置分页列表输出
 */
export interface QueryTableConfigPagedOutput {
	/**
	 * 表格Id
	 */
	tableId?: number;
	/**
	 * 表格Key
	 */
	tableKey?: string;
	/**
	 * 表格名称
	 */
	tableName?: string;
	/**
	 * 备注
	 */
	remark?: string;
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
	createdTime?: string;
	/**
	 * 
	 */
	updatedUserName?: string;
	/**
	 * 
	 */
	updatedTime?: string;
	/**
	 * 
	 */
	rowVersion?: number;
}

