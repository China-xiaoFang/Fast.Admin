import { axiosUtil } from "@fast-china/axios";
import type { AxiosProgressEvent } from "axios";
import type { QueryFilePagedOutput } from "./models/QueryFilePagedOutput";
import type { QueryFilePagedInput } from "./models/QueryFilePagedInput";
import type { DownloadFileInput } from "./models/DownloadFileInput";

/**
 * Fast.Center.Service.File.FileService 文件服务Api
 */
export const fileApi = {
  /**
   * 获取文件分页列表
   */
  queryFilePaged(data: QueryFilePagedInput) {
    return axiosUtil.request<PagedResult<QueryFilePagedOutput>>({
      url: "/file/queryFilePaged",
      method: "post",
      data,
      requestType: "query",
    });
  },
  /**
   * 下载文件
   */
  download(data: DownloadFileInput) {
    return axiosUtil.request({
      url: "/file/download",
      method: "post",
      data,
      responseType: "blob",
      autoDownloadFile: true,
      requestType: "download",
    });
  },
  /**
   * 上传Logo
   */
  uploadLogo(filePath: string, onUploadProgress?: (progressEvent: AxiosProgressEvent) => void) {
    return axiosUtil.request<string>({
      url: "/file/uploadLogo",
      method: "upload",
      name: "file",
      filePath,
      onUploadProgress,
      cancelDuplicateRequest: false,
      requestType: "upload",
    });
  },
  /**
   * 上传头像
   */
  uploadAvatar(filePath: string, onUploadProgress?: (progressEvent: AxiosProgressEvent) => void) {
    return axiosUtil.request<string>({
      url: "/file/uploadAvatar",
      method: "upload",
      name: "file",
      filePath,
      onUploadProgress,
      cancelDuplicateRequest: false,
      requestType: "upload",
    });
  },
  /**
   * 上传证件照
   */
  uploadIdPhoto(filePath: string, onUploadProgress?: (progressEvent: AxiosProgressEvent) => void) {
    return axiosUtil.request<string>({
      url: "/file/uploadIdPhoto",
      method: "upload",
      name: "file",
      filePath,
      onUploadProgress,
      cancelDuplicateRequest: false,
      requestType: "upload",
    });
  },
  /**
   * 上传富文本
   */
  uploadEditor(filePath: string, onUploadProgress?: (progressEvent: AxiosProgressEvent) => void) {
    return axiosUtil.request<string>({
      url: "/file/uploadEditor",
      method: "upload",
      name: "file",
      filePath,
      onUploadProgress,
      cancelDuplicateRequest: false,
      requestType: "upload",
    });
  },
  /**
   * 上传文件
   */
  uploadFile(filePath: string, onUploadProgress?: (progressEvent: AxiosProgressEvent) => void) {
    return axiosUtil.request<string>({
      url: "/file/uploadFile",
      method: "upload",
      name: "file",
      filePath,
      onUploadProgress,
      cancelDuplicateRequest: false,
      requestType: "upload",
    });
  },
};
