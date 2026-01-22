import { ApplicationTemplateTypeEnum } from "@/api/enums/ApplicationTemplateTypeEnum";

/**
 * Fast.Center.Service.ApplicationOpenId.Dto.EditApplicationOpenIdInput.EditApplicationTemplateIdInput 编辑应用模板Id输入
 */
export interface EditApplicationTemplateIdInput {
  /**
   * 记录Id
   */
  recordId?: number;
  /**
   * 
   */
  templateType?: ApplicationTemplateTypeEnum;
  /**
   * 模板Id
   */
  templateId?: string;
}

