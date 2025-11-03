import { TagTypeEnum } from "@/api/enums/TagTypeEnum";
import { YesOrNotEnum } from "@/api/enums/YesOrNotEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * Fast.Center.Service.Dictionary.Dto.EditDictionaryInput.EditDictionaryItemInput 编辑字典项输入
 */
export interface EditDictionaryItemInput {
  /**
   * 字典项Id
   */
  dictionaryItemIdId?: number;
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
   * 
   */
  visible?: YesOrNotEnum;
  /**
   * 
   */
  status?: CommonStatusEnum;
}

