import { TagTypeEnum } from "@/api/enums/TagTypeEnum";
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
  type?: TagTypeEnum;
  /**
   * 排序
   */
  order?: number;
  /**
   * 提示
   */
  tips?: string;
  /**
   * 是否显示
   */
  visible?: boolean;
  /**
   * 
   */
  status?: CommonStatusEnum;
}

