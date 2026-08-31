import { axiosUtil } from "@fast-china/axios";
import type { ElSelectorOutput, FaTableEnumColumnCtx, PagedInput, PagedResult } from "fast-element-plus";
import type { AddDictionaryInput } from "./models/AddDictionaryInput";
import type { DictionaryIdInput } from "./models/DictionaryIdInput";
import type { EditDictionaryInput } from "./models/EditDictionaryInput";
import type { QueryDictionaryDetailOutput } from "./models/QueryDictionaryDetailOutput";
import type { QueryDictionaryPagedOutput } from "./models/QueryDictionaryPagedOutput";

/**
 * 字典服务Api
 */
export const dictionaryApi = {
	/**
	 * 获取字典
	 */
	queryDictionary(): Promise<Record<string, FaTableEnumColumnCtx[]>> {
		return axiosUtil.request<Record<string, FaTableEnumColumnCtx[]>>({
			url: "/dictionary/queryDictionary",
			method: "get",
			requestType: "query",
		});
	},
	/**
	 * 字典分页选择器
	 */
	selectorPaged(data: PagedInput): Promise<PagedResult<ElSelectorOutput<number>>> {
		return axiosUtil.request<PagedResult<ElSelectorOutput<number>>>({
			url: "/dictionary/selectorPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
	/**
	 * 获取字典分页列表
	 */
	queryDictionaryPaged(data: PagedInput): Promise<PagedResult<QueryDictionaryPagedOutput>> {
		return axiosUtil.request<PagedResult<QueryDictionaryPagedOutput>>({
			url: "/dictionary/queryDictionaryPaged",
			method: "post",
			data,
			requestType: "query",
		});
	},
	/**
	 * 获取字典详情
	 */
	queryDictionaryDetail(dictionaryId: number): Promise<QueryDictionaryDetailOutput> {
		return axiosUtil.request<QueryDictionaryDetailOutput>({
			url: "/dictionary/queryDictionaryDetail",
			method: "get",
			params: {
				dictionaryId,
			},
			requestType: "query",
		});
	},
	/**
	 * 添加字典
	 */
	addDictionary(data: AddDictionaryInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/dictionary/addDictionary",
			method: "post",
			data,
			requestType: "add",
		});
	},
	/**
	 * 编辑字典
	 */
	editDictionary(data: EditDictionaryInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/dictionary/editDictionary",
			method: "post",
			data,
			requestType: "edit",
		});
	},
	/**
	 * 删除字典
	 */
	deleteDictionary(data: DictionaryIdInput): Promise<unknown> {
		return axiosUtil.request({
			url: "/dictionary/deleteDictionary",
			method: "post",
			data,
			requestType: "delete",
		});
	},
};
