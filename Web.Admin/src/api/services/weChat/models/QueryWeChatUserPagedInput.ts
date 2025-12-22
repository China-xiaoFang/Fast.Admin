import { PagedInput } from "fast-element-plus";

/**
 * Fast.Center.Service.WeChat.Dto.QueryWeChatUserPagedInput 获取微信用户分页列表输入
 */
export interface QueryWeChatUserPagedInput extends PagedInput {
  /**
   * 应用ID
   */
  appId?: string;
  /**
   * 昵称
   */
  nickName?: string;
  /**
   * OpenId
   */
  openId?: string;
}
