/**
 * 获取职位详情输出
 */
export interface QueryPositionDetailOutput {
	/**
	 * 职位Id
	 */
	positionId?: number;
	/**
	 * 职位名称
	 */
	positionName?: string;
	/**
	 * 排序
	 */
	sort?: number;
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

