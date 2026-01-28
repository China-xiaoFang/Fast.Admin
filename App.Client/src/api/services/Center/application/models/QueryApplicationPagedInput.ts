import { EditionEnum } from "@/api/enums/EditionEnum";

/**
 * Fast.Center.Service.Application.Dto.QueryApplicationPagedInput 获取应用分页列表输入
 */
export interface QueryApplicationPagedInput extends PagedInput  {
  /**
   * 
   */
  edition?: EditionEnum;
  /**
   * 
   */
  readonly isOrderBy?: boolean;
}

