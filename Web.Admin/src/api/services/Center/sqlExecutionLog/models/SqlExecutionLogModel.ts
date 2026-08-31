/**
 * Sql执行日志表Model类
 */
export interface SqlExecutionLogModel {
	/**
	 * 记录Id
	 */
	recordId?: number;
	/**
	 * 账号Id
	 */
	accountId?: number;
	/**
	 * 手机
	 */
	mobile?: string;
	/**
	 * 昵称
	 */
	nickName?: string;
	/**
	 * 执行秒数
	 */
	executeSeconds?: number;
	/**
	 * 纯SQL，参数化后的SQL
	 */
	pureSql?: string;
	/**
	 * 执行时间
	 */
	createdTime?: string;
	/**
	 * 租户Id
	 */
	tenantId?: number;
	/**
	 * 租户名称
	 */
	tenantName?: string;
	/**
	 * 
	 */
	device?: string;
	/**
	 * 
	 */
	os?: string;
	/**
	 * 
	 */
	browser?: string;
	/**
	 * 
	 */
	province?: string;
	/**
	 * 
	 */
	city?: string;
	/**
	 * 
	 */
	ip?: string;
	/**
	 * 
	 */
	departmentId?: number;
	/**
	 * 
	 */
	departmentName?: string;
	/**
	 * 
	 */
	createdUserId?: number;
	/**
	 * 
	 */
	createdUserName?: string;
}

