import type { SerialDateTypeEnum } from "@/api/enums/SerialDateTypeEnum";
import type { SerialRuleTypeEnum } from "@/api/enums/SerialRuleTypeEnum";
import type { SerialSpacerEnum } from "@/api/enums/SerialSpacerEnum";

/**
 * 添加序号规则输入
 */
export interface AddSerialRuleInput {
	/**
	 * 
	 */
	ruleType?: SerialRuleTypeEnum;
	/**
	 * 前缀
	 */
	prefix?: string;
	/**
	 * 
	 */
	dateType?: SerialDateTypeEnum;
	/**
	 * 
	 */
	spacer?: SerialSpacerEnum;
	/**
	 * 长度
	 */
	length?: number;
}

