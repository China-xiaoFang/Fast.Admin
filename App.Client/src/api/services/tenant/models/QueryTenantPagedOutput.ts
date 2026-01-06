import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { EditionEnum } from "@/api/enums/EditionEnum";
import { TenantTypeEnum } from "@/api/enums/TenantTypeEnum";

/**
 * Fast.Center.Service.Tenant.Dto.QueryTenantPagedOutput 获取租户分页列表输出
 */
export interface QueryTenantPagedOutput {
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
   * 
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
   * 
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
   * 
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
   * 
   */
  departmentName?: string;
  /**
   * 
   */
  createdUserName?: string;
  /**
   * 
   */
  createdTime?: Date;
  /**
   * 
   */
  updatedUserName?: string;
  /**
   * 
   */
  updatedTime?: Date;
  /**
   * 
   */
  rowVersion?: number;
}

