import { axiosUtil } from "@fast-china/axios";
import { QueryPositionPagedOutput } from "./models/QueryPositionPagedOutput";
import { QueryPositionDetailOutput } from "./models/QueryPositionDetailOutput";
import { AddPositionInput } from "./models/AddPositionInput";
import { EditPositionInput } from "./models/EditPositionInput";
import { PositionIdInput } from "./models/PositionIdInput";

/**
 * Fast.Admin.Service.Position.PositionService 职位服务Api
 */
export const positionApi = {
  /**
   * 职位选择器
   */
  positionSelector() {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/position/positionSelector",
      method: "get",
      requestType: "query",
    });
  },
  /**
   * 获取职位分页列表
   */
  queryPositionPaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<QueryPositionPagedOutput>>({
      url: "/position/queryPositionPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取职位详情
   */
  queryPositionDetail(positionId: number) {
    return axiosUtil.request<QueryPositionDetailOutput>({
      url: "/position/queryPositionDetail",
      method: "get",
      params: {
        positionId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加职位
   */
  addPosition(data: AddPositionInput) {
    return axiosUtil.request({
      url: "/position/addPosition",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑职位
   */
  editPosition(data: EditPositionInput) {
    return axiosUtil.request({
      url: "/position/editPosition",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 删除职位
   */
  deletePosition(data: PositionIdInput) {
    return axiosUtil.request({
      url: "/position/deletePosition",
      method: "post",
      data,
      requestType: "delete",
    });
  },
};
