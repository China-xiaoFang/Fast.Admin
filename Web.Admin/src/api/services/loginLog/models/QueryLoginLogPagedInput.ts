import { PagedInput } from "fast-element-plus";

/**
 * Fast.CenterLog.Service.LoginLog.Dto.QueryLoginLogPagedInput 获取登录日志分页列表输入
 */
export interface QueryLoginLogPagedInput extends PagedInput {
  /**
   * 用户名
   */
  userName?: string;
  /**
   * IP地址
   */
  ip?: string;
  /**
   * 状态
   */
  success?: boolean;
  /**
   * 开始时间
   */
  startTime?: string;
  /**
   * 结束时间
   */
  endTime?: string;
}
