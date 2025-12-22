import { axiosUtil } from "@fast-china/axios";
import { PagedResult } from "fast-element-plus";
import { QueryPasswordRecordPagedOutput } from "./models/QueryPasswordRecordPagedOutput";
import { QueryPasswordRecordPagedInput } from "./models/QueryPasswordRecordPagedInput";

/**
 * Fast.Center.Service.PasswordRecord.PasswordRecordService 密码映射服务Api
 */
export const passwordRecordApi = {
  /**
   * 获取密码记录分页列表
   */
  queryPasswordRecordPaged(data: QueryPasswordRecordPagedInput) {
    return axiosUtil.request<PagedResult<QueryPasswordRecordPagedOutput>>({
      url: "/passwordRecord/queryPasswordRecordPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
};
