/**
 * Fast.Admin.Service.JobLevel.Dto.QueryJobLevelDetailOutput 获取职级详情输出
 */
export interface QueryJobLevelDetailOutput {
  /**
   * 职级Id
   */
  jobLevelId?: number;
  /**
   * 职级名称
   */
  jobLevelName?: string;
  /**
   * 职级等级
   */
  level?: string;
  /**
   * 备注
   */
  remark?: string;
  /**
   * 
   */
  departmentName?: string;
  /**
   * 
   */
  createdUserName?: string;
  /**
   * 
   */
  createdTime?: Date;
  /**
   * 
   */
  updatedUserName?: string;
  /**
   * 
   */
  updatedTime?: Date;
  /**
   * 
   */
  rowVersion?: number;
}

