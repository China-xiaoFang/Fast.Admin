import { EditionEnum } from "@/api/enums/EditionEnum";
import { UserTypeEnum } from "@/api/enums/UserTypeEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * Fast.Center.Service.Login.Dto.LoginOutput.LoginTenantOutput 登录租户输出
 */
export interface LoginTenantOutput {
  /**
   * 用户Key
   */
  userKey?: string;
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
   * LogoUrl
   */
  logoUrl?: string;
  /**
   * 工号
   */
  employeeNo?: string;
  /**
   * 姓名
   */
  employeeName?: string;
  /**
   * 证件照
   */
  idPhoto?: string;
  /**
   * 部门Id
   */
  deptId?: number;
  /**
   * 部门名称
   */
  deptName?: string;
  /**
   * 
   */
  userType?: UserTypeEnum;
  /**
   * 
   */
  status?: CommonStatusEnum;
}

