import { axiosUtil } from "@fast-china/axios";
import type { ElSelectorOutput, PagedInput, PagedResult } from "fast-element-plus";
import type { AccountIdInput } from "./models/AccountIdInput";
import type { ChangePasswordInput } from "./models/ChangePasswordInput";
import type { EditAccountInput } from "./models/EditAccountInput";
import type { QueryAccountDetailOutput } from "./models/QueryAccountDetailOutput";
import type { QueryAccountPagedInput } from "./models/QueryAccountPagedInput";
import type { QueryAccountPagedOutput } from "./models/QueryAccountPagedOutput";

/**
 * 账号服务Api
 */
export const accountApi = {
	/**
	 * 账号选择器
	 */
	accountSelector(data: PagedInput): Promise<PagedResult<ElSelectorOutput<string>>> {
		return axiosUtil.request<PagedResult<ElSelectorOutput<string>>>({
			url: "/account/accountSelector",
			method: "post",
			data,
			requestType: "query",
		});
	},
	/**
	 * 获取账号分页列表
	 */
	queryAccountPaged(data: QueryAccountPagedInput): Promise<PagedResult<QueryAccountPagedOutput>> {
		return axiosUtil.request<PagedResult<QueryAccountPagedOutput>>({
			url: "/account/queryAccountPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
	/**
	 * 获取账号详情
	 */
	queryAccountDetail(accountId: string): Promise<QueryAccountDetailOutput> {
		return axiosUtil.request<QueryAccountDetailOutput>({
			url: "/account/queryAccountDetail",
			method: "get",
			params: {
				accountId,
			},
			requestType: "query",
		});
	},
	/**
	 * 获取编辑账号详情
	 */
	queryEditAccountDetail(): Promise<EditAccountInput> {
		return axiosUtil.request<EditAccountInput>({
			url: "/account/queryEditAccountDetail",
			method: "get",
			requestType: "query",
		});
	},
	/**
	 * 编辑账号
	 */
	editAccount(data: EditAccountInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/account/editAccount",
			method: "post",
			data,
			requestType: "edit",
		});
	},
	/**
	 * 账号修改密码
	 */
	changePassword(data: ChangePasswordInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/account/changePassword",
			method: "post",
			data,
			requestType: "edit",
		});
	},
	/**
	 * 账号解除锁定
	 */
	unlock(data: AccountIdInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/account/unlock",
			method: "post",
			data,
			requestType: "edit",
		});
	},
	/**
	 * 账号重置密码
	 */
	resetPassword(data: AccountIdInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/account/resetPassword",
			method: "post",
			data,
			requestType: "edit",
		});
	},
	/**
	 * 账号更改状态
	 */
	changeStatus(data: AccountIdInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/account/changeStatus",
			method: "post",
			data,
			requestType: "edit",
		});
	},
};
