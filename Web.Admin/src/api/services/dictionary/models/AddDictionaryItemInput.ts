import { TagTypeEnum } from "@/api/enums/TagTypeEnum";
import { YesOrNotEnum } from "@/api/enums/YesOrNotEnum";

/**
 * Fast.Center.Service.Dictionary.Dto.AddDictionaryInput.AddDictionaryItemInput 添加字典项输入
 */
export interface AddDictionaryItemInput {
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
}

