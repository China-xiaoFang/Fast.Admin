import { axiosUtil } from "@fast-china/axios";
import { QuerySysSerialRulePagedOutput } from "./models/QuerySysSerialRulePagedOutput";
import { QuerySysSerialRuleDetailOutput } from "./models/QuerySysSerialRuleDetailOutput";
import { AddSysSerialRuleInput } from "./models/AddSysSerialRuleInput";
import { EditSysSerialRuleInput } from "./models/EditSysSerialRuleInput";

/**
 * Fast.Center.Service.SysSerial.SysSerialService 系统序号规则服务Api
 */
export const sysSerialApi = {
  /**
   * 获取系统序号规则分页列表
   */
  querySysSerialRulePaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<QuerySysSerialRulePagedOutput>>({
      url: "/sysSerial/querySysSerialRulePaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取系统序号规则详情
   */
  querySysSerialRuleDetail(serialRuleId: number) {
    return axiosUtil.request<QuerySysSerialRuleDetailOutput>({
      url: "/sysSerial/querySysSerialRuleDetail",
      method: "get",
      params: {
        serialRuleId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加系统序号规则
   */
  addSysSerialRule(data: AddSysSerialRuleInput) {
    return axiosUtil.request({
      url: "/sysSerial/addSysSerialRule",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑系统序号规则
   */
  editSysSerialRule(data: EditSysSerialRuleInput) {
    return axiosUtil.request({
      url: "/sysSerial/editSysSerialRule",
      method: "post",
      data,
      requestType: "edit",
    });
  },
};
