import { DictionaryItemTypeEnum } from "@/api/enums/DictionaryItemTypeEnum";
import { YesOrNotEnum } from "@/api/enums/YesOrNotEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * Fast.Center.Service.Dictionary.Dto.QueryDictionaryDetailOutput.QueryDictionaryItemDetailDto 获取字典项详情Dto
 */
export interface QueryDictionaryItemDetailDto {
  /**
   * 字典项Id
   */
  dictionaryItemId?: number;
  /**
   * 字典项名称
   */
  label?: string;
  /**
   * 字典项值
   */
  value?: string;
  /**
   * 
   */
  type?: DictionaryItemTypeEnum;
  /**
   * 排序
   */
  order?: number;
  /**
   * 提示
   */
  tips?: string;
  /**
   * 
   */
  visible?: YesOrNotEnum;
  /**
   * 
   */
  status?: CommonStatusEnum;
}

