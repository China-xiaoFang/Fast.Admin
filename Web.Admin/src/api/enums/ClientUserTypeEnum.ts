/**
 * 客户端用户类型枚举
 */
export enum ClientUserTypeEnum {
	/**
	 * 小程序
	 */
	MiniProgram = 1,
	/**
	 * 公众号
	 */
	OfficialAccount = 2,
	/**
	 * 服务号
	 */
	ServiceAccount = 4,
	/**
	 * 开放平台
	 */
	OpenPlatform = 8,
	/**
	 * 企业微信
	 */
	WorkWeChat = 16,
}
