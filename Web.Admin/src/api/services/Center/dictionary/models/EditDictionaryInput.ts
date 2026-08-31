import type { EditDictionaryItemInput } from "./EditDictionaryItemInput";
import type { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import type { DictionaryValueTypeEnum } from "@/api/enums/DictionaryValueTypeEnum";

/**
 * 编辑字典输入
 */
export interface EditDictionaryInput {
	/**
	 * 字典Id
	 */
	dictionaryId?: string;
	/**
	 * 字典名称
	 */
	dictionaryName?: string;
	/**
	 * 
	 */
	valueType?: DictionaryValueTypeEnum;
	/**
	 * 
	 */
	status?: CommonStatusEnum;
	/**
	 * 备注
	 */
	remark?: string;
	/**
	 * 字典项集合
	 */
	dictionaryItemList?: EditDictionaryItemInput[];
	/**
	 * 
	 */
	rowVersion?: string;
}

