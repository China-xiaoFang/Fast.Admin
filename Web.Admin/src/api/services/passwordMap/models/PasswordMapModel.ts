import { PasswordTypeEnum } from "@/api/enums/PasswordTypeEnum";

/**
 * Fast.Center.Entity.PasswordMapModel 密码映射表Model类
 */
export interface PasswordMapModel {
  /**
   * 
   */
  type?: PasswordTypeEnum;
  /**
   * 明文
   */
  plaintext?: string;
  /**
   * 密文
   */
  ciphertext?: string;
}

