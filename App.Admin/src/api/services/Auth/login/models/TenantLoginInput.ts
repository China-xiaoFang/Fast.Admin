/**
 * 租户登录输入
 */
export interface TenantLoginInput {
	/**
	 * 账号Key
	 */
	accountKey?: string;
	/**
	 * 用户Key
	 */
	userKey?: string;
	/**
	 * 原始密码；生产环境必须通过 HTTPS 传输
	 */
	password?: string;
}

