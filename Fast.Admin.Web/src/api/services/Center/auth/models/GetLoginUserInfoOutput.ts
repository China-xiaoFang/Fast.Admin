import { AuthModuleInfoDto } from "./AuthModuleInfoDto";
import { AuthMenuInfoDto } from "./AuthMenuInfoDto";
import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";

/**
 * Fast.Center.Service.Auth.Dto.GetLoginUserInfoOutput 获取登录用户信息输出
 */
export interface GetLoginUserInfoOutput {
	/**
	 * 模块集合
	 */
	moduleList?: Array<AuthModuleInfoDto>;
	/**
	 * 菜单集合
	 */
	menuList?: Array<AuthMenuInfoDto>;
	/**
	 *
	 */
	deviceType?: AppEnvironmentEnum;
	/**
	 * 设备Id
	 */
	deviceId?: string;
	/**
	 * 用户Id
	 */
	userId?: number;
	/**
	 * 手机
	 */
	mobile?: string;
	/**
	 * 昵称
	 */
	nickName?: string;
	/**
	 * 是否管理员
	 */
	isAdmin?: boolean;
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
	 * 按钮编码集合
	 */
	buttonCodeList?: Array<string>;
}
