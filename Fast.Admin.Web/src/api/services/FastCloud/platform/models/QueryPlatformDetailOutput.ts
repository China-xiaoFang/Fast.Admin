import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { EditionEnum } from "@/api/enums/EditionEnum";

/**
 * 获取平台详情输出
 */
export interface QueryPlatformDetailOutput {
  /**
   * 平台管理员邮箱
   */
  adminEmail?: string;
  /**
   * 平台管理员电话
   */
  adminPhone?: string;
  /**
   * 主键Id
   */
  id?: number;
  /**
   * 平台编号
   */
  platformNo?: string;
  /**
   * 平台名称
   */
  platformName?: string;
  /**
   * 平台简称
   */
  shortName?: string;
  /**
   * 
   */
  status?: CommonStatusEnum;
  /**
   * 平台管理员名称
   */
  adminName?: string;
  /**
   * 平台管理员手机
   */
  adminMobile?: string;
  /**
   * LogoUrl
   */
  logoUrl?: string;
  /**
   * 开通时间
   */
  activationTime?: Date;
  /**
   * 
   */
  edition?: EditionEnum;
  /**
   * 自动续费
   */
  autoRenewal?: boolean;
  /**
   * 续费到期时间
   */
  renewalExpiryTime?: Date;
  /**
   * 是否试用平台（未正式上线）
   */
  isTrial?: boolean;
  /**
   * 是否已初始化
   */
  isInitialized?: boolean;
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
}

