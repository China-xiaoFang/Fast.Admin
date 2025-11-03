/**
 * Fast.Center.Service.Login.Dto.LoginStatusEnum 登录状态枚举
 */
export enum LoginStatusEnum {
  /**
   * 登录成功
   */
  Success = 1,
  /**
   * 选择租户
   */
  SelectTenant = 2,
  /**
   * 授权过期
   */
  AuthExpired = 4,
}
