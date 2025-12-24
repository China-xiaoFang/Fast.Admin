import { PagedInput } from "fast-element-plus";
import { WeChatUserTypeEnum } from "@/api/enums/WeChatUserTypeEnum";
import { GenderEnum } from "@/api/enums/GenderEnum";

/**
 * Fast.Center.Service.WeChat.Dto.QueryWeChatUserPagedInput 获取微信用户分页列表输入
 */
export interface QueryWeChatUserPagedInput extends PagedInput  {
  /**
   * 应用Id
   */
  appId?: number;
  /**
   * 用户类型
   */
  userType?: WeChatUserTypeEnum;
  /**
   * 性别
   */
  sex?: GenderEnum;
}

