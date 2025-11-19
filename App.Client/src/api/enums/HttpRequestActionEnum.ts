/**
 * Http请求行为枚举
 */
export enum HttpRequestActionEnum {
  /**
   * 未知
   */
  None = 0,
  /**
   * 鉴权
   */
  Auth = 1,
  /**
   * 分页
   */
  Paged = 11,
  /**
   * 查询
   */
  Query = 12,
  /**
   * 添加
   */
  Add = 21,
  /**
   * 编辑
   */
  Edit = 31,
  /**
   * 删除
   */
  Delete = 41,
  /**
   * 提交
   */
  Submit = 51,
  /**
   * 上传
   */
  Upload = 61,
  /**
   * 下载
   */
  Download = 62,
  /**
   * 导入
   */
  Import = 71,
  /**
   * 导出
   */
  Export = 72,
  /**
   * 通知
   */
  Notify = 253,
  /**
   * 回调
   */
  Callback = 254,
  /**
   * 其他
   */
  Other = 255,
}
