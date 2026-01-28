import { axiosUtil } from "@fast-china/axios";
import { PagedInput, PagedResult } from "fast-element-plus";
import { QuerySerialRulePagedOutput } from "./models/QuerySerialRulePagedOutput";
import { QuerySerialRuleDetailOutput } from "./models/QuerySerialRuleDetailOutput";
import { AddSerialRuleInput } from "./models/AddSerialRuleInput";
import { EditSerialRuleInput } from "./models/EditSerialRuleInput";

/**
 * Fast.Admin.Service.Serial.SerialService 序号规则服务Api
 */
export const serialApi = {
  /**
   * 获取序号规则分页列表
   */
  querySerialRulePaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<QuerySerialRulePagedOutput>>({
      url: "/serial/querySerialRulePaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取序号规则详情
   */
  querySerialRuleDetail(serialRuleId: number) {
    return axiosUtil.request<QuerySerialRuleDetailOutput>({
      url: "/serial/querySerialRuleDetail",
      method: "get",
      params: {
        serialRuleId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加序号规则
   */
  addSerialRule(data: AddSerialRuleInput) {
    return axiosUtil.request({
      url: "/serial/addSerialRule",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑序号规则
   */
  editSerialRule(data: EditSerialRuleInput) {
    return axiosUtil.request({
      url: "/serial/editSerialRule",
      method: "post",
      data,
      requestType: "edit",
    });
  },
};
