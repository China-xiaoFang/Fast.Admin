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
   * 创建者用户名称
   */
  createdUserName?: string;
  /**
   * 创建时间
   */
  createdTime?: Date;
  /**
   * 更新者用户名称
   */
  updatedUserName?: string;
  /**
   * 更新时间
   */
  updatedTime?: Date;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
  /**
   * 开放平台信息
   */
  openIdList?: Array<QueryApplicationOpenIdDetailDto>;
}

