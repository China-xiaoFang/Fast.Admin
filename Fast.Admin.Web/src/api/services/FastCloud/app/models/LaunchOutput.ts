import { EditionEnum } from "@/api/enums/EditionEnum";
import { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";

/**
 * Fast.FastCloud.Service.App.Dto.LaunchOutput Launch 输出
 */
export interface LaunchOutput {
  /**
   * 是否平台管理
   */
  hasPlatform?: boolean;
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
   * 服务开始时间
   */
  serviceStartTime?: Date;
  /**
   * 服务结束时间
   */
  serviceEndTime?: Date;
  /**
   * 开放平台Id
   */
  openId?: string;
  /**
   * 
   */
  environmentType?: EnvironmentTypeEnum;
  /**
   * 接口请求地址
   */
  apiUrl?: string;
  /**
   * 接口请求基础地址
   */
  apiBaseUrl?: string;
  /**
   * WebSocket地址
   */
  webSocketUrl?: string;
  /**
   * 接口请求超时时间（毫秒）
   */
  requestTimeout?: number;
  /**
   * 接口请求加密
   */
  requestEncipher?: boolean;
}

