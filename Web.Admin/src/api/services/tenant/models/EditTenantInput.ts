import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { EditionEnum } from "@/api/enums/EditionEnum";

/**
 * Fast.Center.Service.Tenant.Dto.EditTenantInput 编辑租户输入
 */
export interface EditTenantInput {
  /**
   * 租户Id
   */
  tenantId?: number;
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
  /**
   * 行版本
   */
  rowVersion?: number;
}

