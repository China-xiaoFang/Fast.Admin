import { GenderEnum } from "@/api/enums/GenderEnum";

/**
 * Fast.Center.Service.Account.Dto.EditAccountInput 编辑账号输入
 */
export interface EditAccountInput {
  /**
   * 邮箱
   */
  email?: string;
  /**
   * 昵称
   */
  nickName?: string;
  /**
   * 头像
   */
  avatar?: string;
  /**
   * 电话
   */
  phone?: string;
  /**
   * 
   */
  sex?: GenderEnum;
  /**
   * 生日
   */
  birthday?: Date;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
}

