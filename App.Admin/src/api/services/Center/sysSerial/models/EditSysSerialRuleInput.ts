import type { SerialDateTypeEnum } from "@/api/enums/SerialDateTypeEnum";
import type { SerialSpacerEnum } from "@/api/enums/SerialSpacerEnum";

/**
 * 编辑系统序号规则输入
 */
export interface EditSysSerialRuleInput {
	/**
	 * 序号规则Id
	 */
	serialRuleId?: string;
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
	/**
	 * 
	 */
	rowVersion?: string;
}

