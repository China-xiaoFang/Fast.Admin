import { PagedInput } from "fast-element-plus";
import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";

/**
 * Fast.Center.Service.ApplicationOpenId.Dto.QueryApplicationOpenIdPagedInput 获取应用标识分页列表输入
 */
export interface QueryApplicationOpenIdPagedInput extends PagedInput  {
  /**
   * 应用Id
   */
  appId?: number;
  /**
   * 
   */
  appType?: AppEnvironmentEnum;
  /**
   * 
   */
  environmentType?: EnvironmentTypeEnum;
}

