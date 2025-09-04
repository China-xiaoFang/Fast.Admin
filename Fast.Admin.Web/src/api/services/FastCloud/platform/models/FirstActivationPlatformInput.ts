import { EditionEnum } from "@/api/enums/EditionEnum";
import { RenewalDurationEnum } from "@/api/enums/RenewalDurationEnum";
import { DbType } from "@/api/enums/DbType";

/**
 * Fast.FastCloud.Service.Platform.Dto.FirstActivationPlatformInput 初次开通平台输入
 */
export interface FirstActivationPlatformInput {
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
   * 平台管理员名称
   */
  adminName?: string;
  /**
   * 平台管理员手机
   */
  adminMobile?: string;
  /**
   * 平台管理员邮箱
   */
  adminEmail?: string;
  /**
   * 平台管理员电话
   */
  adminPhone?: string;
  /**
   * LogoUrl
   */
  logoUrl?: string;
  /**
   * 
   */
  edition?: EditionEnum;
  /**
   * 
   */
  duration?: RenewalDurationEnum;
  /**
   * 续费金额
   */
  amount?: number;
  /**
   * 
   */
  dbType?: DbType;
  /**
   * 公网Ip地址
   */
  publicIp?: string;
  /**
   * 内网Ip地址
   */
  intranetIp?: string;
  /**
   * 端口号
   */
  port?: number;
  /**
   * 数据库名称
   */
  dbName?: string;
  /**
   * 数据库用户
   */
  dbUser?: string;
  /**
   * 数据库密码
   */
  dbPwd?: string;
  /**
   * 自定义连接字符串
   */
  customConnectionStr?: string;
  /**
   * 备注
   */
  remark?: string;
}

