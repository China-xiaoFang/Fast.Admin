import { PasswordOperationTypeEnum } from "@/api/enums/PasswordOperationTypeEnum";
import { PasswordTypeEnum } from "@/api/enums/PasswordTypeEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * Fast.Center.Service.PasswordRecord.Dto.QueryPasswordRecordPagedOutput 获取密码记录分页列表输出
 */
export interface QueryPasswordRecordPagedOutput {
  /**
   * 记录Id
   */
  recordId?: number;
  /**
   * 账号Id
   */
  accountId?: number;
  /**
   * 
   */
  operationType?: PasswordOperationTypeEnum;
  /**
   * 
   */
  type?: PasswordTypeEnum;
  /**
   * 密码
   */
  password?: string;
  /**
   * 创建时间
   */
  createdTime?: Date;
  /**
   * 账号Key
   */
  accountKey?: string;
  /**
   * 手机
   */
  mobile?: string;
  /**
   * 邮箱
   */
  email?: string;
  /**
   * 
   */
  status?: CommonStatusEnum;
  /**
   * 昵称
   */
  nickName?: string;
  /**
   * 头像
   */
  avatar?: string;
}

