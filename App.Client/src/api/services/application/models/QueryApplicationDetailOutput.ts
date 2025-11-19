import { EditionEnum } from "@/api/enums/EditionEnum";
import { QueryApplicationOpenIdDetailDto } from "./QueryApplicationOpenIdDetailDto";

/**
 * Fast.Center.Service.Application.Dto.QueryApplicationDetailOutput 获取应用详情输出
 */
export interface QueryApplicationDetailOutput {
  /**
   * 应用Id
   */
  appId?: number;
  /**
   * 
   */
  edition?: EditionEnum;
  /**
   * 应用编号
   */
  appNo?: string;
  /**
   * 应用名称
   */
  appName?: string;
  /**
   * LogoUrl
   */
  logoUrl?: string;
  /**
   * 主题色
   */
  themeColor?: string;
  /**
   * ICP备案号
   */
  icpSecurityCode?: string;
  /**
   * 公安备案号
   */
  publicSecurityCode?: string;
  /**
   * 用户协议
   */
  userAgreement?: string;
  /**
   * 隐私协议
   */
  privacyAgreement?: string;
  /**
   * 服务协议
   */
  serviceAgreement?: string;
  /**
   * 备注
   */
  remark?: string;
  /**
   * 租户Id
   */
  tenantId?: number;
  /**
   * 租户名称
   */
  tenantName?: string;
  /**
   * 开放平台信息
   */
  openIdList?: Array<QueryApplicationOpenIdDetailDto>;
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

