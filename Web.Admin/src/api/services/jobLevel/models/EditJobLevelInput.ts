/**
 * Fast.Admin.Service.JobLevel.Dto.EditJobLevelInput 编辑职级输入
 */
export interface EditJobLevelInput {
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
  level?: number;
  /**
   * 备注
   */
  remark?: string;
  /**
   * 
   */
  rowVersion?: number;
}

