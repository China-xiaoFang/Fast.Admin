import type { PasswordOperationTypeEnum } from "@/api/enums/PasswordOperationTypeEnum";
import type { PasswordTypeEnum } from "@/api/enums/PasswordTypeEnum";

/**
 * 获取密码记录分页列表输出
 */
export interface QueryPasswordRecordPagedOutput {
	/**
	 * 记录Id
	 */
	recordId?: number;
	/**
	 * 账号Id
	 */
	accountId?: number;
	/**
	 * 
	 */
	operationType?: PasswordOperationTypeEnum;
	/**
	 * 
	 */
	type?: PasswordTypeEnum;
	/**
	 * 创建时间
	 */
	createdTime?: string;
	/**
	 * 账号Key
	 */
	accountKey?: string;
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
}

