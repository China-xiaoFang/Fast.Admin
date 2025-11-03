/**
 * Fast.Center.Service.Account.Dto.ChangePasswordInput 账号修改密码输入
 */
export interface ChangePasswordInput {
  /**
   * 旧密码
   */
  oldPassword?: string;
  /**
   * 新密码
   */
  newPassword?: string;
  /**
   * 确认密码
   */
  confirmPassword?: string;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
}

