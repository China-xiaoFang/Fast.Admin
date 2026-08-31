import type { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import type { TagTypeEnum } from "@/api/enums/TagTypeEnum";

/**
 * 编辑字典项输入
 */
export interface EditDictionaryItemInput {
	/**
	 * 字典项Id
	 */
	dictionaryItemId?: number;
	/**
	 * 字典项名称
	 */
	label?: string;
	/**
	 * 字典项值
	 */
	value?: string;
	/**
	 * 
	 */
	type?: TagTypeEnum;
	/**
	 * 排序
	 */
	order?: number;
	/**
	 * 提示
	 */
	tips?: string;
	/**
	 * 是否显示
	 */
	visible?: boolean;
	/**
	 * 
	 */
	status?: CommonStatusEnum;
}

