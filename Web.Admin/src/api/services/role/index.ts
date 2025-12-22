import { axiosUtil } from "@fast-china/axios";
import { ElSelectorOutput, PagedInput, PagedResult } from "fast-element-plus";

/**
 * Role Service API
 */
export const roleApi = {
  /**
   * Query role paged list
   */
  queryRolePaged(data: PagedInput) {
    return axiosUtil.request<PagedResult<any>>({
      url: "/role/queryRolePaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * Query role detail
   */
  queryRoleDetail(roleId: number) {
    return axiosUtil.request<any>({
      url: "/role/queryRoleDetail",
      method: "get",
      params: {
        roleId,
      },
      requestType: "query",
    });
  },
  /**
   * Add role
   */
  addRole(data: any) {
    return axiosUtil.request({
      url: "/role/addRole",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * Edit role
   */
  editRole(data: any) {
    return axiosUtil.request({
      url: "/role/editRole",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * Delete role
   */
  deleteRole(data: any) {
    return axiosUtil.request({
      url: "/role/deleteRole",
      method: "post",
      data,
      requestType: "delete",
    });
  },
  /**
   * Authorize menus
   */
  authorizeMenus(data: any) {
    return axiosUtil.request({
      url: "/role/authorizeMenus",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * Authorize buttons
   */
  authorizeButtons(data: any) {
    return axiosUtil.request({
      url: "/role/authorizeButtons",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * Query role menus
   */
  queryRoleMenus(roleId: number) {
    return axiosUtil.request<number[]>({
      url: "/role/queryRoleMenus",
      method: "get",
      params: {
        roleId,
      },
      requestType: "query",
    });
  },
  /**
   * Query role buttons
   */
  queryRoleButtons(roleId: number) {
    return axiosUtil.request<number[]>({
      url: "/role/queryRoleButtons",
      method: "get",
      params: {
        roleId,
      },
      requestType: "query",
    });
  },
  /**
   * Role selector
   */
  roleSelector() {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/role/roleSelector",
      method: "get",
      requestType: "query",
    });
  },
};
