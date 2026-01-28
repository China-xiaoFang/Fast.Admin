import { axiosUtil } from "@fast-china/axios";
import { QueryModuleDetailOutput } from "./models/QueryModuleDetailOutput";
import { AddModuleInput } from "./models/AddModuleInput";
import { EditModuleInput } from "./models/EditModuleInput";
import { ModuleIdInput } from "./models/ModuleIdInput";

/**
 * Fast.Center.Service.Menu.MenuService 菜单服务Api
 */
export const moduleApi = {
  /**
   * 模块选择器
   */
  moduleSelector(appId: number) {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/module/moduleSelector",
      method: "get",
      params: {
        appId,
      },
      requestType: "query",
    });
  },
  /**
   * 获取模块详情
   */
  queryModuleDetail(moduleId: number) {
    return axiosUtil.request<QueryModuleDetailOutput>({
      url: "/module/queryModuleDetail",
      method: "get",
      params: {
        moduleId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加模块
   */
  addModule(data: AddModuleInput) {
    return axiosUtil.request({
      url: "/module/addModule",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑模块
   */
  editModule(data: EditModuleInput) {
    return axiosUtil.request({
      url: "/module/editModule",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 删除模块
   */
  deleteModule(data: ModuleIdInput) {
    return axiosUtil.request({
      url: "/module/deleteModule",
      method: "post",
      data,
      requestType: "delete",
    });
  },
  /**
   * 模块更改状态
   */
  changeStatus(data: ModuleIdInput) {
    return axiosUtil.request({
      url: "/module/changeStatus",
      method: "post",
      data,
      requestType: "edit",
    });
  },
};
