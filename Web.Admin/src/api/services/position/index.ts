import { axiosUtil } from "@fast-china/axios";
import { ElSelectorOutput, PagedInput, PagedResult } from "fast-element-plus";

/**
 * Position Service API
 */
export const positionApi = {
  /**
   * Query position paged list
   */
  queryPositionPaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<any>>({
      url: "/position/queryPositionPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * Query position detail
   */
  queryPositionDetail(positionId: number) {
    return axiosUtil.request<any>({
      url: "/position/queryPositionDetail",
      method: "get",
      params: {
        positionId,
      },
      requestType: "query",
    });
  },
  /**
   * Add position
   */
  addPosition(data: any) {
    return axiosUtil.request({
      url: "/position/addPosition",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * Edit position
   */
  editPosition(data: any) {
    return axiosUtil.request({
      url: "/position/editPosition",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * Delete position
   */
  deletePosition(data: any) {
    return axiosUtil.request({
      url: "/position/deletePosition",
      method: "post",
      data,
      requestType: "delete",
    });
  },
  /**
   * Position selector
   */
  positionSelector() {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/position/positionSelector",
      method: "get",
      requestType: "query",
    });
  },
};
