import { EditionEnum } from "@/api/enums/EditionEnum";

/**
 * Fast.Center.Service.Application.Dto.QueryApplicationPagedOutput 获取应用分页列表输出
 */
export interface QueryApplicationPagedOutput {
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
   * 备注
   */
  remark?: string;
  /**
   * 
   */
  departmentId?: number;
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

