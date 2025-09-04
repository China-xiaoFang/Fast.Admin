import { axiosUtil } from "@fast-china/axios";
import { ElSelectorOutput, PagedInput, PagedResult } from "fast-element-plus";
import { QueryPlatformPagedOutput } from "./models/QueryPlatformPagedOutput";
import { QueryPlatformDetailOutput } from "./models/QueryPlatformDetailOutput";
import { QueryPlatformRenewalRecordPagedOutput } from "./models/QueryPlatformRenewalRecordPagedOutput";
import { QueryPlatformRenewalRecordInput } from "./models/QueryPlatformRenewalRecordInput";
import { FirstActivationPlatformInput } from "./models/FirstActivationPlatformInput";
import { EditPlatformInput } from "./models/EditPlatformInput";
import { ChangePlatformStatusInput } from "./models/ChangePlatformStatusInput";

/**
 * Fast.FastCloud.Api.PlatformApplication 平台Api
 */
export const platformApi = {
  /**
   * 平台选择器
   */
  platformSelector(data: PagedInput) {
    return axiosUtil.request<PagedResult<ElSelectorOutput<number>>>({
      url: "/platform/platformSelector",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取平台分页列表
   */
  queryPlatformPaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<QueryPlatformPagedOutput>>({
      url: "/platform/queryPlatformPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取平台详情
   */
  queryPlatformDetail(platformId: number) {
    return axiosUtil.request<QueryPlatformDetailOutput>({
      url: "/platform/queryPlatformDetail",
      method: "get",
      params: {
        platformId,
      },
      requestType: "query",
    });
  },
  /**
   * 获取平台续费记录分页列表
   */
  queryPlatformRenewalRecord(data: QueryPlatformRenewalRecordInput) {
    return axiosUtil.request<PagedResult<QueryPlatformRenewalRecordPagedOutput>>({
      url: "/platform/queryPlatformRenewalRecord",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 初次开通平台
   */
  firstActivationPlatform(data: FirstActivationPlatformInput) {
    return axiosUtil.request({
      url: "/platform/firstActivationPlatform",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑平台
   */
  editPlatform(data: EditPlatformInput) {
    return axiosUtil.request({
      url: "/platform/editPlatform",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 启用/禁用平台
   */
  changePlatformStatus(data: ChangePlatformStatusInput) {
    return axiosUtil.request({
      url: "/platform/changePlatformStatus",
      method: "post",
      data,
      requestType: "edit",
    });
  },
};
