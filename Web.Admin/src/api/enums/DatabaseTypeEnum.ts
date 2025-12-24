/**
 * Fast.Shared.DatabaseTypeEnum 数据库类型枚举
 */
export enum DatabaseTypeEnum {
  /**
   * 系统核心库
   */
  Center = 1,
  /**
   * 系统核心日志库
   */
  CenterLog = 2,
  /**
   * 系统业务库
   */
  Admin = 4,
  /**
   * 系统业务日志库
   */
  AdminLog = 8,
  /**
   * 网关系统库
   */
  Gateway = 16,
  /**
   * 部署系统库
   */
  Deploy = 32,
}
