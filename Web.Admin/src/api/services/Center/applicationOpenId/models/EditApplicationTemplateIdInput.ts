import type { ApplicationTemplateTypeEnum } from "@/api/enums/ApplicationTemplateTypeEnum";

/**
 * 编辑应用模板Id输入
 */
export interface EditApplicationTemplateIdInput {
	/**
	 * 记录Id
	 */
	recordId?: string;
	/**
	 * 
	 */
	templateType?: ApplicationTemplateTypeEnum;
	/**
	 * 模板Id
	 */
	templateId?: string;
}

