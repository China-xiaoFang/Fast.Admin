import { EditApplicationOpenIdDto } from "./EditApplicationOpenIdDto";
import { EditionEnum } from "@/api/enums/EditionEnum";

/**
 * Fast.Center.Service.Application.Dto.EditApplicationInput 编辑应用输入
 */
export interface EditApplicationInput {
  /**
   * 应用Id
   */
  appId?: number;
  /**
   * 更新版本控制字段
   */
  rowVersion?: number;
  /**
   * 开放平台信息
   */
  openIdList?: Array<EditApplicationOpenIdDto>;
  /**
   * 
   */
  edition?: EditionEnum;
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
}

