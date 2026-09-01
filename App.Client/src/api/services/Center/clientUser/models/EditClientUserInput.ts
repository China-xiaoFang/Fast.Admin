import type { GenderEnum } from "@/api/enums/GenderEnum";

/**
 * 编辑客户端用户输入
 */
export interface EditClientUserInput {
	/**
	 * 手机
	 */
	mobile?: string;
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
	sex?: GenderEnum;
	/**
	 * 
	 */
	rowVersion?: string;
}

