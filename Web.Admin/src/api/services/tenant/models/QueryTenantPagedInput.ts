import { PagedInput } from "fast-element-plus";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { EditionEnum } from "@/api/enums/EditionEnum";
import { TenantTypeEnum } from "@/api/enums/TenantTypeEnum";

/**
 * Fast.Center.Service.Tenant.Dto.QueryTenantPagedInput 获取租户分页列表输入
 */
export interface QueryTenantPagedInput extends PagedInput  {
  /**
   * 
   */
  status?: CommonStatusEnum;
  /**
   * 
   */
  edition?: EditionEnum;
  /**
   * 租户管理员手机
   */
  adminMobile?: string;
  /**
   * 租户管理员邮箱
   */
  adminEmail?: string;
  /**
   * 
   */
  tenantType?: TenantTypeEnum;
  /**
   * 
   */
  readonly isOrderBy?: boolean;
}

