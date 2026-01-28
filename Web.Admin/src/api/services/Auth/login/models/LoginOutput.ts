import { LoginStatusEnum } from "@/api/enums/LoginStatusEnum";
import { LoginTenantOutput } from "./LoginTenantOutput";

/**
 * Fast.Center.Service.Login.Dto.LoginOutput 登录输出
 */
export interface LoginOutput {
  /**
   * 
   */
  status?: LoginStatusEnum;
  /**
   * 消息
   */
  message?: string;
  /**
   * 账号Key
   */
  accountKey?: string;
  /**
   * 昵称
   */
  nickName?: string;
  /**
   * 头像
   */
  avatar?: string;
  /**
   * 租户集合
   */
  tenantList?: Array<LoginTenantOutput>;
}

