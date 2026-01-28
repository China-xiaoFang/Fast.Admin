import { axiosUtil } from "@fast-china/axios";
import { QueryEmployeePagedOutput } from "./models/QueryEmployeePagedOutput";
import { QueryEmployeePagedInput } from "./models/QueryEmployeePagedInput";
import { QueryEmployeeDetailOutput } from "./models/QueryEmployeeDetailOutput";
import { AddEmployeeInput } from "./models/AddEmployeeInput";
import { EditEmployeeInput } from "./models/EditEmployeeInput";
import { ChangeStatusInput } from "./models/ChangeStatusInput";
import { EmployeeResignedInput } from "./models/EmployeeResignedInput";
import { BindLoginAccountInput } from "./models/BindLoginAccountInput";
import { EmployeeIdInput } from "./models/EmployeeIdInput";
import { EmployeeAuthInput } from "./models/EmployeeAuthInput";

/**
 * Fast.Admin.Service.Employee.EmployeeService 职员服务Api
 */
export const employeeApi = {
  /**
   * 职员选择器
   */
  employeeSelector(data: PagedInput) {
    return axiosUtil.request<PagedResult<ElSelectorOutput<number>>>({
      url: "/employee/employeeSelector",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取职员分页列表
   */
  queryEmployeePaged(data: QueryEmployeePagedInput) {
    return axiosUtil.request<PagedResult<QueryEmployeePagedOutput>>({
      url: "/employee/queryEmployeePaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 获取职员详情
   */
  queryEmployeeDetail(employeeId: number) {
    return axiosUtil.request<QueryEmployeeDetailOutput>({
      url: "/employee/queryEmployeeDetail",
      method: "get",
      params: {
        employeeId,
      },
      requestType: "query",
    });
  },
  /**
   * 添加职员
   */
  addEmployee(data: AddEmployeeInput) {
    return axiosUtil.request({
      url: "/employee/addEmployee",
      method: "post",
      data,
      requestType: "add",
    });
  },
  /**
   * 编辑职员
   */
  editEmployee(data: EditEmployeeInput) {
    return axiosUtil.request({
      url: "/employee/editEmployee",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 职员更改状态
   */
  changeStatus(data: ChangeStatusInput) {
    return axiosUtil.request({
      url: "/employee/changeStatus",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 职员离职
   */
  employeeResigned(data: EmployeeResignedInput) {
    return axiosUtil.request({
      url: "/employee/employeeResigned",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 绑定登录账号
   */
  bindLoginAccount(data: BindLoginAccountInput) {
    return axiosUtil.request({
      url: "/employee/bindLoginAccount",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 更改登录状态
   */
  changeLoginStatus(data: EmployeeIdInput) {
    return axiosUtil.request({
      url: "/employee/changeLoginStatus",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 职员授权
   */
  employeeAuth(data: EmployeeAuthInput) {
    return axiosUtil.request({
      url: "/employee/employeeAuth",
      method: "post",
      data,
      requestType: "edit",
    });
  },
  /**
   * 获取职员授权菜单
   */
  queryEmployeeAuthMenu(data: EmployeeIdInput) {
    return axiosUtil.request<EmployeeAuthInput>({
      url: "/employee/queryEmployeeAuthMenu",
      method: "post",
      data,
      requestType: "query",
    });
  },
};
