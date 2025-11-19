import { DictionaryValueTypeEnum } from "@/api/enums/DictionaryValueTypeEnum";
import { YesOrNotEnum } from "@/api/enums/YesOrNotEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * Fast.Center.Service.Dictionary.Dto.QueryDictionaryPagedOutput 获取字典分页列表输出
 */
export interface QueryDictionaryPagedOutput {
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

