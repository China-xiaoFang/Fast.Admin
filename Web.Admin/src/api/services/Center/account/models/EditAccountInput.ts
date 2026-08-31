/**
 * 编辑账号输入
 */
export interface EditAccountInput {
	/**
	 * 手机
	 */
	mobile?: string;
	/**
	 * 邮箱
	 */
	email?: string;
	/**
	 * 昵称
	 */
	nickName?: string;
	/**
	 * 头像
	 */
	avatar?: string;
	/**
	 * 
	 */
	rowVersion?: number;
}

