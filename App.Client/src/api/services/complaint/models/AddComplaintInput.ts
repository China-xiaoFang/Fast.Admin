import { ComplaintTypeEnum } from "@/api/enums/ComplaintTypeEnum";

/**
 * Fast.Center.Service.Complaint.Dto.AddComplaintInput 添加投诉输入
 */
export interface AddComplaintInput {
  /**
   * 
   */
  complaintType?: ComplaintTypeEnum;
  /**
   * 联系电话
   */
  contactPhone?: string;
  /**
   * 联系邮箱
   */
  contactEmail?: string;
  /**
   * 投诉描述
   */
  description?: string;
  /**
   * 附件截图
   */
  attachmentImages?: Array<string>;
}

