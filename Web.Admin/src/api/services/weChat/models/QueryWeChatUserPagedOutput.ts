import { WeChatUserTypeEnum } from "@/api/enums/WeChatUserTypeEnum";
import { GenderEnum } from "@/api/enums/GenderEnum";

/**
 * Fast.Center.Service.WeChat.Dto.QueryWeChatUserPagedOutput 获取微信用户分页列表输出
 */
export interface QueryWeChatUserPagedOutput {
  /**
   * 微信用户Id
   */
  weChatId?: number;
  /**
   * 应用ID
   */
  appId?: string;
  /**
   * 用户类型
   */
  userType?: WeChatUserTypeEnum;
  /**
   * 唯一用户标识
   */
  openId?: string;
  /**
   * 统一用户标识
   */
  unionId?: string;
  /**
   * 微信昵称
   */
  nickName?: string;
  /**
   * 头像
   */
  avatar?: string;
  /**
   * 性别
   */
  sex?: GenderEnum;
  /**
   * 国家
   */
  country?: string;
  /**
   * 省份
   */
  province?: string;
  /**
   * 城市
   */
  city?: string;
  /**
   * 创建时间
   */
  createdTime?: Date;
}
