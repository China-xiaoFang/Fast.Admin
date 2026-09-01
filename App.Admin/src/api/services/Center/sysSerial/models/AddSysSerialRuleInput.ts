import type { SerialDateTypeEnum } from "@/api/enums/SerialDateTypeEnum";
import type { SerialSpacerEnum } from "@/api/enums/SerialSpacerEnum";
import type { SysSerialRuleTypeEnum } from "@/api/enums/SysSerialRuleTypeEnum";

/**
 * 添加系统序号规则输入
 */
export interface AddSysSerialRuleInput {
	/**
	 * 
	 */
	ruleType?: SysSerialRuleTypeEnum;
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

