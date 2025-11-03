/**
 * System.EnvironmentTypeEnum 环境类型枚举
 */
export enum EnvironmentTypeEnum {
  /**
   * 生产环境
   */
  Production = 1,
  /**
   * 开发环境
   */
  Development = 2,
  /**
   * 测试环境
   */
  Test = 4,
  /**
   * 测试验收环境
   */
  UAT = 8,
  /**
   * 预生产环境
   */
  PreProduction = 16,
  /**
   * 灰度环境
   */
  GrayDeployment = 32,
  /**
   * 压测环境
   */
  StressTest = 64,
}
