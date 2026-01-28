import { axiosUtil } from "@fast-china/axios";
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
   * 部门选择器
   */
  departmentSelector(orgId: number) {
    return axiosUtil.request<ElSelectorOutput<number>[]>({
      url: "/department/departmentSelector",
      method: "get",
      params: {
        orgId,
      },
      requestType: "query",
    });
  },
  /**
   * 获取部门列表
   */
  queryDepartmentPaged(data: QueryDepartmentPagedInput) {
    return axiosUtil.request<QueryDepartmentPagedOutput[]>({
      url: "/department/queryDepartmentPaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取部门详情
   */
  queryDepartmentDetail(departmentId: number) {
    return axiosUtil.request<QueryDepartmentDetailOutput>({
      url: "/department/queryDepartmentDetail",
      method: "get",
      params: {
        departmentId,
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
