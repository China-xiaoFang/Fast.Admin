import type { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";

/**
 * 租户在线用户表Model类
 */
export interface TenantOnlineUserModel {
	/**
	 * 连接Id
	 */
	connectionId?: string;
	/**
	 * 
	 */
	deviceType?: AppEnvironmentEnum;
	/**
	 * 设备Id
	 */
	deviceId?: string;
	/**
	 * 应用编号
	 */
	appNo?: string;
	/**
	 * 应用名称
	 */
	appName?: string;
	/**
	 * 账号Id
	 */
	accountId?: string;
	/**
	 * 手机
	 */
	mobile?: string;
	/**
	 * 昵称
	 */
	nickName?: string;
	/**
	 * 头像
	 */
	avatar?: string;
	/**
	 * 职员Id
	 */
	employeeId?: string;
	/**
	 * 工号
	 */
	employeeNo?: string;
	/**
	 * 姓名
	 */
	employeeName?: string;
	/**
	 * 部门Id
	 */
	departmentId?: string;
	/**
	 * 部门名称
	 */
	departmentName?: string;
	/**
	 * 是否超级管理员
	 */
	isSuperAdmin?: boolean;
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
	lastLoginTime?: string;
	/**
	 * 是否在线
	 */
	isOnline?: boolean;
	/**
	 * 下线时间
	 */
	offlineTime?: string;
	/**
	 * 租户Id
	 */
	tenantId?: string;
}

