/**
 * 登录输入
 */
export interface LoginInput {
	/**
	 * 账号
	 */
	account?: string;
	/**
	 * 原始密码；生产环境必须通过 HTTPS 传输
	 */
	password?: string;
}

