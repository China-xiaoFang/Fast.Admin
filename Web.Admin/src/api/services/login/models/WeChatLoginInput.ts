/**
 * Fast.Center.Service.Login.Dto.WeChatLoginInput 微信登录输入
 */
export interface WeChatLoginInput {
  /**
   * wx.login 获取到的Code
   */
  weChatCode?: string;
  /**
   * 加密算法的初始向量
   */
  iv?: string;
  /**
   * 包括敏感数据在内的完整用户信息的加密数据
   */
  encryptedData?: string;
}

