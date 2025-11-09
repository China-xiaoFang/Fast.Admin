/**
 * Fast.Center.Service.Login.Dto.WeChatAuthLoginInput 微信授权登录输入
 */
export interface WeChatAuthLoginInput {
  /**
   * wx.login 获取到的Code
   */
  weChatCode?: string;
  /**
   * 动态令牌
   */
  code?: string;
}

