import { EditionEnum } from "@/api/enums/EditionEnum";
import { RenewalTypeEnum } from "@/api/enums/RenewalTypeEnum";
import { RenewalDurationEnum } from "@/api/enums/RenewalDurationEnum";

/**
 * Fast.FastCloud.Service.Platform.Dto.QueryPlatformRenewalRecordPagedOutput 获取平台续费记录分页列表输出
 */
export interface QueryPlatformRenewalRecordPagedOutput {
  /**
   * 主键Id
   */
  id?: number;
  /**
   * 
   */
  fromEdition?: EditionEnum;
  /**
   * 
   */
  toEdition?: EditionEnum;
  /**
   * 
   */
  renewalType?: RenewalTypeEnum;
  /**
   * 
   */
  duration?: RenewalDurationEnum;
  /**
   * 续费时间
   */
  renewalTime?: Date;
  /**
   * 本次续费到期时间
   */
  renewalExpiryTime?: Date;
  /**
   * 续费金额
   */
  amount?: number;
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
}

