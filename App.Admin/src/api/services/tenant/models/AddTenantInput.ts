import { EditionEnum } from "@/api/enums/EditionEnum";

/**
 * Fast.Center.Service.Tenant.Dto.AddTenantInput 添加租户输入
 */
export interface AddTenantInput {
  /**
   * 租户编码
   */
  tenantCode?: string;
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
   * LogoUrl
   */
  logoUrl?: string;
}

