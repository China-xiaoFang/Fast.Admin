import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { EditionEnum } from "@/api/enums/EditionEnum";
import { TenantTypeEnum } from "@/api/enums/TenantTypeEnum";

/**
 * Fast.Center.Service.Tenant.Dto.QueryTenantDetailOutput 获取租户详情输出
 */
export interface QueryTenantDetailOutput {
  /**
   * 租户Id
   */
  tenantId?: number;
  /**
   * 租户编号
   */
  tenantNo?: string;
  /**
   * 租户编码
   */
  tenantCode?: string;
  /**
   * 状态
   */
  status?: CommonStatusEnum;
  /**
   * 租户名称
   */
  tenantName?: string;
  /**
   * 租户简称
   */
  shortName?: string;
  /**
   * 租户英文名称
   */
  spellName?: string;
  /**
   * 版本
   */
  edition?: EditionEnum;
  /**
   * 租户管理员账号Id
   */
  adminAccountId?: number;
  /**
   * 租户管理员名称
   */
  adminName?: string;
  /**
   * 租户管理员手机
   */
  adminMobile?: string;
  /**
   * 租户管理员邮箱
   */
  adminEmail?: string;
  /**
   * 租户管理员电话
   */
  adminPhone?: string;
  /**
   * 租户机器人名称
   */
  robotName?: string;
  /**
   * 租户类型
   */
  tenantType?: TenantTypeEnum;
  /**
   * LogoUrl
   */
  logoUrl?: string;
  /**
   * 允许删除数据
   */
  allowDeleteData?: boolean;
  /**
   * 初次登录设备
   */
  firstLoginDevice?: string;
  /**
   * 初次登录操作系统（版本）
   */
  firstLoginOS?: string;
  /**
   * 初次登录浏览器（版本）
   */
  firstLoginBrowser?: string;
  /**
   * 初次登录省份
   */
  firstLoginProvince?: string;
  /**
   * 初次登录城市
   */
  firstLoginCity?: string;
  /**
   * 初次登录Ip
   */
  firstLoginIp?: string;
  /**
   * 初次登录时间
   */
  firstLoginTime?: Date;
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
   * 密码错误次数
   */
  passwordErrorTime?: number;
  /**
   * 锁定开始时间
   */
  lockStartTime?: Date;
  /**
   * 锁定结束时间
   */
  lockEndTime?: Date;
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

