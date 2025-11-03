import { DictionaryValueTypeEnum } from "@/api/enums/DictionaryValueTypeEnum";
import { YesOrNotEnum } from "@/api/enums/YesOrNotEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import { QueryDictionaryItemDetailDto } from "./QueryDictionaryItemDetailDto";

/**
 * Fast.Center.Service.Dictionary.Dto.QueryDictionaryDetailOutput 获取字典详情输出
 */
export interface QueryDictionaryDetailOutput {
  /**
   * 字典Id
   */
  dictionaryId?: number;
  /**
   * 字典Key
   */
  dictionaryKey?: string;
  /**
   * 字典名称
   */
  dictionaryName?: string;
  /**
   * 
   */
  valueType?: DictionaryValueTypeEnum;
  /**
   * 
   */
  hasFlags?: YesOrNotEnum;
  /**
   * 
   */
  status?: CommonStatusEnum;
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
   * 字典项集合
   */
  dictionaryItemList?: Array<QueryDictionaryItemDetailDto>;
}

