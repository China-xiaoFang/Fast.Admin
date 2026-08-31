import { axiosUtil } from "@fast-china/axios";
import type { ElSelectorOutput } from "fast-element-plus";
import type { AddDepartmentInput } from "./models/AddDepartmentInput";
import type { DepartmentIdInput } from "./models/DepartmentIdInput";
import type { EditDepartmentInput } from "./models/EditDepartmentInput";
import type { QueryDepartmentDetailOutput } from "./models/QueryDepartmentDetailOutput";
import type { QueryDepartmentPagedInput } from "./models/QueryDepartmentPagedInput";
import type { QueryDepartmentPagedOutput } from "./models/QueryDepartmentPagedOutput";

/**
 * 部门服务Api
 */
export const departmentApi = {
	/**
	 * 部门选择器
	 */
	departmentSelector(orgId: number): Promise<ElSelectorOutput<number>[]> {
		return axiosUtil.request<ElSelectorOutput<number>[]>({
			url: "/department/departmentSelector",
			method: "get",
			params: {
				orgId,
			},
			requestType: "query",
		});
	},
	/**
	 * 获取部门列表
	 */
	queryDepartmentPaged(data: QueryDepartmentPagedInput): Promise<QueryDepartmentPagedOutput[]> {
		return axiosUtil.request<QueryDepartmentPagedOutput[]>({
			url: "/department/queryDepartmentPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
	/**
	 * 获取部门详情
	 */
	queryDepartmentDetail(departmentId: number): Promise<QueryDepartmentDetailOutput> {
		return axiosUtil.request<QueryDepartmentDetailOutput>({
			url: "/department/queryDepartmentDetail",
			method: "get",
			params: {
				departmentId,
			},
			requestType: "query",
		});
	},
	/**
	 * 添加部门
	 */
	addDepartment(data: AddDepartmentInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/department/addDepartment",
			method: "post",
			data,
			requestType: "add",
		});
	},
	/**
	 * 编辑部门
	 */
	editDepartment(data: EditDepartmentInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/department/editDepartment",
			method: "post",
			data,
			requestType: "edit",
		});
	},
	/**
	 * 删除部门
	 */
	deleteDepartment(data: DepartmentIdInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/department/deleteDepartment",
			method: "post",
			data,
			requestType: "delete",
		});
	},
};
