/**
 * App运行环境枚举
 */
export enum AppEnvironmentEnum {
  /**
   * Web
   */
  Web = 1,
  /**
   * Windows
   */
  Windows = 2,
  /**
   * Mac
   */
  Mac = 4,
  /**
   * Linux
   */
  Linux = 8,
  /**
   * 桌面端
   */
  Desktop = 14,
  /**
   * Android
   */
  Android = 32,
  /**
   * IOS
   */
  IOS = 64,
  /**
   * 移动端
   */
  Mobile = 96,
  /**
   * 快应用
   */
  QuickApp = 128,
  /**
   * 微信小程序
   */
  WeChatMiniProgram = 256,
  /**
   * QQ小程序
   */
  QQMiniProgram = 512,
  /**
   * 抖音小程序
   */
  TiktokMiniProgram = 1024,
  /**
   * 百度小程序
   */
  BaiduMiniProgram = 2048,
  /**
   * 支付宝小程序
   */
  AlipayMiniProgram = 4096,
  /**
   * 快手小程序
   */
  KuaishouMiniProgram = 8192,
  /**
   * 飞书小程序
   */
  FeishuMiniProgram = 16384,
  /**
   * 钉钉小程序
   */
  DingTalkMiniProgram = 32768,
  /**
   * 京东小程序
   */
  JDMiniProgram = 65536,
  /**
   * 小红书小程序
   */
  XiaohongshuMiniProgram = 131072,
  /**
   * 小程序
   */
  MiniProgram = 262016,
  /**
   * 移动端（三端）
   */
  MobileThree = 262112,
  /**
   * Api
   */
  Api = 16777216,
  /**
   * 其他
   */
  Other = 1073741824,
}
