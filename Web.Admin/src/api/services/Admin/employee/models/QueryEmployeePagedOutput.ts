import type { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";
import type { EmployeeStatusEnum } from "@/api/enums/EmployeeStatusEnum";
import type { GenderEnum } from "@/api/enums/GenderEnum";

/**
 * 获取职员分页列表输出
 */
export interface QueryEmployeePagedOutput {
	/**
	 * 职员Id
	 */
	employeeId?: number;
	/**
	 * 工号
	 */
	employeeNo?: string;
	/**
	 * 姓名
	 */
	employeeName?: string;
	/**
	 * 手机
	 */
	mobile?: string;
	/**
	 * 
	 */
	status?: EmployeeStatusEnum;
	/**
	 * 邮箱
	 */
	email?: string;
	/**
	 * 
	 */
	sex?: GenderEnum;
	/**
	 * 证件照
	 */
	idPhoto?: string;
	/**
	 * 入职日期
	 */
	entryDate?: string;
	/**
	 * 离职日期
	 */
	resignDate?: string;
	/**
	 * 备注
	 */
	remark?: string;
	/**
	 * 创建者用户名称
	 */
	createdUserName?: string;
	/**
	 * 创建时间
	 */
	createdTime?: string;
	/**
	 * 更新者用户名称
	 */
	updatedUserName?: string;
	/**
	 * 更新时间
	 */
	updatedTime?: string;
	/**
	 * 更新版本控制字段
	 */
	rowVersion?: number;
	/**
	 * 机构Id
	 */
	orgId?: number;
	/**
	 * 机构名称
	 */
	orgName?: string;
	/**
	 * 机构名称
	 */
	orgNames?: string[];
	/**
	 * 部门Id
	 */
	departmentId?: number;
	/**
	 * 部门名称
	 */
	departmentName?: string;
	/**
	 * 部门名称
	 */
	departmentNames?: string[];
	/**
	 * 职位Id
	 */
	positionId?: number;
	/**
	 * 职位名称
	 */
	positionName?: string;
	/**
	 * 职级Id
	 */
	jobLevelId?: number;
	/**
	 * 职级名称
	 */
	jobLevelName?: string;
	/**
	 * 是否为负责人
	 */
	isPrincipal?: boolean;
	/**
	 * 角色名称
	 */
	roleNames?: string;
	/**
	 * 
	 */
	accountStatus?: CommonStatusEnum;
	/**
	 * 账号手机
	 */
	accountMobile?: string;
	/**
	 * 账号邮箱
	 */
	accountEmail?: string;
	/**
	 * 账号昵称
	 */
	accountNickName?: string;
	/**
	 * 最后登录时间
	 */
	lastLoginTime?: string;
}

