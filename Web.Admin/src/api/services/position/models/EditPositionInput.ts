/**
 * Fast.Admin.Service.Position.Dto.EditPositionInput 编辑职位输入
 */
export interface EditPositionInput {
  /**
   * 职位Id
   */
  positionId?: number;
  /**
   * 职位名称
   */
  positionName?: string;
  /**
   * 排序
   */
  sort?: number;
  /**
   * 备注
   */
  remark?: string;
  /**
   * 
   */
  rowVersion?: number;
}

