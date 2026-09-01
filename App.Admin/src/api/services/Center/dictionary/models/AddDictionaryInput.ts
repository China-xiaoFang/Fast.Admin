import type { AddDictionaryItemInput } from "./AddDictionaryItemInput";
import type { DictionaryValueTypeEnum } from "@/api/enums/DictionaryValueTypeEnum";

/**
 * 添加字典输入
 */
export interface AddDictionaryInput {
	/**
	 * 字典Key
	 */
	dictionaryKey?: string;
	/**
	 * 字典名称
	 */
	dictionaryName?: string;
	/**
	 * 
	 */
	valueType?: DictionaryValueTypeEnum;
	/**
	 * 备注
	 */
	remark?: string;
	/**
	 * 字典项集合
	 */
	dictionaryItemList?: AddDictionaryItemInput[];
}

