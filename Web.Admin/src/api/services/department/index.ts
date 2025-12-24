import { axiosUtil } from "@fast-china/axios";
import { ElTreeOutput, PagedResult } from "fast-element-plus";
import { QueryDepartmentPagedOutput } from "./models/QueryDepartmentPagedOutput";
import { QueryDepartmentPagedInput } from "./models/QueryDepartmentPagedInput";
import { QueryDepartmentDetailOutput } from "./models/QueryDepartmentDetailOutput";
import { AddDepartmentInput } from "./models/AddDepartmentInput";
import { EditDepartmentInput } from "./models/EditDepartmentInput";
import { DepartmentIdInput } from "./models/DepartmentIdInput";

/**
 * Fast.Admin.Service.Department.DepartmentService 部门服务Api
 */
export const departmentApi = {
  /**
   * 部门树形列表
   */
  departmentTree() {
    return axiosUtil.request<ElTreeOutput<number>[]>({
      url: "/department/departmentTree",
      method: "get",
      requestType: "query",
    });
  },
  /**
   * 获取部门分页列表
   */
  queryDepartmentPaged(data: QueryDepartmentPagedInput) {
    return axiosUtil.request<PagedResult<QueryDepartmentPagedOutput>>({
      url: "/department/queryDepartmentPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取部门详情
   */
  queryDepartmentDetail(orgId: number) {
    return axiosUtil.request<QueryDepartmentDetailOutput>({
      url: "/department/queryDepartmentDetail",
      method: "get",
      params: {
        orgId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加部门
   */
  addDepartment(data: AddDepartmentInput) {
    return axiosUtil.request({
      url: "/department/addDepartment",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑部门
   */
  editDepartment(data: EditDepartmentInput) {
    return axiosUtil.request({
      url: "/department/editDepartment",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 删除部门
   */
  deleteDepartment(data: DepartmentIdInput) {
    return axiosUtil.request({
      url: "/department/deleteDepartment",
      method: "post",
      data,
      requestType: "delete",
    });
  },
};
