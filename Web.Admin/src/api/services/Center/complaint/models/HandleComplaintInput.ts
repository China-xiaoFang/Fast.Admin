/**
 * 处理投诉输入
 */
export interface HandleComplaintInput {
	/**
	 * 投诉Id
	 */
	complaintId?: string;
	/**
	 * 处理描述
	 */
	handleDescription?: string;
	/**
	 * 备注
	 */
	remark?: string;
	/**
	 * 
	 */
	rowVersion?: string;
}

