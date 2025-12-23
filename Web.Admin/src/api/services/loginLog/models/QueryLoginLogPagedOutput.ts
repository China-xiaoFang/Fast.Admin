/**
 * Fast.CenterLog.Service.LoginLog.Dto.QueryLoginLogPagedOutput 获取登录日志分页列表输出
 */
export interface QueryLoginLogPagedOutput {
  /**
   * 登录日志Id
   */
  loginLogId?: number;
  /**
   * 用户名
   */
  userName?: string;
  /**
   * IP地址
   */
  ip?: string;
  /**
   * 登录地点
   */
  location?: string;
  /**
   * 浏览器
   */
  browser?: string;
  /**
   * 操作系统
   */
  os?: string;
  /**
   * 是否成功
   */
  success?: boolean;
  /**
   * 提示消息
   */
  message?: string;
  /**
   * 登录时间
   */
  loginTime?: Date;
}
