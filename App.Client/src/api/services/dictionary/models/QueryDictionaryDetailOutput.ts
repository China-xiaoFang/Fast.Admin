import { DictionaryValueTypeEnum } from "@/api/enums/DictionaryValueTypeEnum";
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
   * Flags枚举
   */
  hasFlags?: boolean;
  /**
   * 
   */
  status?: CommonStatusEnum;
  /**
   * 备注
   */
  remark?: string;
  /**
   * 字典项集合
   */
  dictionaryItemList?: Array<QueryDictionaryItemDetailDto>;
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

