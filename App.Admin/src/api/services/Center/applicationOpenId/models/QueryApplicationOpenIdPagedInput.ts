import type { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import type { EnvironmentTypeEnum } from "@/api/enums/EnvironmentTypeEnum";

/**
 * 获取应用标识分页列表输入
 */
export interface QueryApplicationOpenIdPagedInput extends PagedInput  {
	/**
	 * 应用Id
	 */
	appId?: string;
	/**
	 * 
	 */
	appType?: AppEnvironmentEnum;
	/**
	 * 
	 */
	environmentType?: EnvironmentTypeEnum;
	/**
	 * 
	 */
	readonly isOrderBy?: boolean;
}

