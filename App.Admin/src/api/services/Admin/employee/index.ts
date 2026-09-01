import { axiosUtil } from "@fast-china/axios";
import type { AddEmployeeInput } from "./models/AddEmployeeInput";
import type { BindLoginAccountInput } from "./models/BindLoginAccountInput";
import type { ChangeStatusInput } from "./models/ChangeStatusInput";
import type { EditEmployeeInput } from "./models/EditEmployeeInput";
import type { EmployeeIdInput } from "./models/EmployeeIdInput";
import type { EmployeeResignedInput } from "./models/EmployeeResignedInput";
import type { QueryEmployeeDetailOutput } from "./models/QueryEmployeeDetailOutput";
import type { QueryEmployeePagedInput } from "./models/QueryEmployeePagedInput";
import type { QueryEmployeePagedOutput } from "./models/QueryEmployeePagedOutput";

/**
 * 职员服务Api
 */
export const employeeApi = {
	/**
	 * 添加职员
	 */
	addEmployee(data: AddEmployeeInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/employee/addEmployee",
			method: "post",
			data,
			requestType: "add",
		});
	},
	/**
	 * 编辑本职员
	 */
	editSelfEmployee(data: EditEmployeeInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/employee/editSelfEmployee",
			method: "post",
			data,
			requestType: "edit",
		});
	},
	/**
	 * 编辑职员
	 */
	editEmployee(data: EditEmployeeInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/employee/editEmployee",
			method: "post",
			data,
			requestType: "edit",
		});
	},
	/**
	 * 职员选择器
	 */
	employeeSelector(data: PagedInput): Promise<PagedResult<ElSelectorOutput<string>>> {
		return axiosUtil.request<PagedResult<ElSelectorOutput<string>>>({
			url: "/employee/employeeSelector",
			method: "post",
			data,
			requestType: "query",
		});
	},
	/**
	 * 获取职员分页列表
	 */
	queryEmployeePaged(data: QueryEmployeePagedInput): Promise<PagedResult<QueryEmployeePagedOutput>> {
		return axiosUtil.request<PagedResult<QueryEmployeePagedOutput>>({
			url: "/employee/queryEmployeePaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
	/**
	 * 获取职员详情
	 */
	queryEmployeeDetail(employeeId: string): Promise<QueryEmployeeDetailOutput> {
		return axiosUtil.request<QueryEmployeeDetailOutput>({
			url: "/employee/queryEmployeeDetail",
			method: "get",
			params: {
				employeeId,
			},
			requestType: "query",
		});
	},
	/**
	 * 职员更改状态
	 */
	changeStatus(data: ChangeStatusInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/employee/changeStatus",
			method: "post",
			data,
			requestType: "edit",
		});
	},
	/**
	 * 职员离职
	 */
	employeeResigned(data: EmployeeResignedInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/employee/employeeResigned",
			method: "post",
			data,
			requestType: "edit",
		});
	},
	/**
	 * 绑定登录账号
	 */
	bindLoginAccount(data: BindLoginAccountInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/employee/bindLoginAccount",
			method: "post",
			data,
			requestType: "edit",
		});
	},
	/**
	 * 更改登录状态
	 */
	changeLoginStatus(data: EmployeeIdInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/employee/changeLoginStatus",
			method: "post",
			data,
			requestType: "edit",
		});
	},
};
