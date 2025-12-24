import { axiosUtil } from "@fast-china/axios";
import { ElSelectorOutput, PagedResult } from "fast-element-plus";
import { QueryRolePagedOutput } from "./models/QueryRolePagedOutput";
import { QueryRolePagedInput } from "./models/QueryRolePagedInput";
import { QueryRoleDetailOutput } from "./models/QueryRoleDetailOutput";
import { AddRoleInput } from "./models/AddRoleInput";
import { EditRoleInput } from "./models/EditRoleInput";
import { RoleIdInput } from "./models/RoleIdInput";
import { RoleAuthInput } from "./models/RoleAuthInput";

/**
 * Fast.Admin.Service.Role.RoleService 角色服务Api
 */
export const roleApi = {
  /**
   * 角色选择器
   */
  roleSelector() {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/role/roleSelector",
      method: "get",
      requestType: "query",
    });
  },
  /**
   * 获取角色分页列表
   */
  queryRolePaged(data: QueryRolePagedInput) {
    return axiosUtil.request<PagedResult<QueryRolePagedOutput>>({
      url: "/role/queryRolePaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取角色详情
   */
  queryRoleDetail(roleId: number) {
    return axiosUtil.request<QueryRoleDetailOutput>({
      url: "/role/queryRoleDetail",
      method: "get",
      params: {
        roleId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加角色
   */
  addRole(data: AddRoleInput) {
    return axiosUtil.request({
      url: "/role/addRole",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑角色
   */
  editRole(data: EditRoleInput) {
    return axiosUtil.request({
      url: "/role/editRole",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 删除角色
   */
  deleteRole(data: RoleIdInput) {
    return axiosUtil.request({
      url: "/role/deleteRole",
      method: "post",
      data,
      requestType: "delete",
    });
  },
  /**
   * 角色授权
   */
  roleAuth(data: RoleAuthInput) {
    return axiosUtil.request({
      url: "/role/roleAuth",
      method: "post",
      data,
      requestType: "edit",
    });
  },
};
