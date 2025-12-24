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
   * 应用Id
   */
  appId?: number;
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
   * 用户纯手机号码
   */
  purePhoneNumber?: string;
  /**
   * 用户手机号码
   */
  phoneNumber?: string;
  /**
   * 用户手机号码区号
   */
  countryCode?: string;
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
   * 语言
   */
  language?: string;
  /**
   * 最后登录设备
   */
  lastLoginDevice?: string;
  /**
   * 最后登录操作系统（版本）
   */
  lastLoginOS?: string;
  /**
   * 最后登录浏览器（版本）
   */
  lastLoginBrowser?: string;
  /**
   * 最后登录省份
   */
  lastLoginProvince?: string;
  /**
   * 最后登录城市
   */
  lastLoginCity?: string;
  /**
   * 最后登录Ip
   */
  lastLoginIp?: string;
  /**
   * 最后登录时间
   */
  lastLoginTime?: Date;
  /**
   * 手机号更新时间
   */
  mobileUpdateTime?: Date;
  /**
   * 部门名称
   */
  departmentName?: string;
  /**
   * 创建人名称
   */
  createdUserName?: string;
  /**
   * 创建时间
   */
  createdTime?: Date;
  /**
   * 更新人名称
   */
  updatedUserName?: string;
  /**
   * 更新时间
   */
  updatedTime?: Date;
  /**
   * 行版本
   */
  rowVersion?: number;
}

