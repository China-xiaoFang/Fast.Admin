import type { PagedInput } from "fast-element-plus";
import type { OperateLogTypeEnum } from "@/api/enums/OperateLogTypeEnum";

/**
 * 获取操作日志分页列表输入
 */
export interface QueryOperateLogPagedInput extends PagedInput  {
	/**
	 * 职员Id
	 */
	employeeId?: number;
	/**
	 * 
	 */
	operateType?: OperateLogTypeEnum;
	/**
	 * 业务单号
	 */
	bizId?: number;
	/**
	 * 
	 */
	readonly isOrderBy?: boolean;
}

