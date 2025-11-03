/**
 * Fast.SqlSugar.PagedSearchTypeEnum 分页搜索类型枚举
 */
export enum PagedSearchTypeEnum {
  /**
   * 模糊匹配
   */
  Like = 1,
  /**
   * 等于
   */
  Equal = 2,
  /**
   * 不等于
   */
  NotEqual = 3,
  /**
   * 大于
   */
  GreaterThan = 4,
  /**
   * 大于等于
   */
  GreaterThanOrEqual = 5,
  /**
   * 小于
   */
  LessThan = 6,
  /**
   * 小于等于
   */
  LessThanOrEqual = 7,
  /**
   * 包含
   */
  Include = 8,
  /**
   * 排除
   */
  NotInclude = 9,
}
