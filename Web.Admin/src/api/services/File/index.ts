import { axiosUtil } from "@fast-china/axios";
import { PagedResult } from "fast-element-plus";
import { QueryFilePagedOutput } from "./models/QueryFilePagedOutput";
import { QueryFilePagedInput } from "./models/QueryFilePagedInput";
import { DownloadFileInput } from "./models/DownloadFileInput";
import { InitChunkUploadInput } from "./models/InitChunkUploadInput";
import { InitChunkUploadOutput } from "./models/InitChunkUploadOutput";
import { MergeChunkInput } from "./models/MergeChunkInput";
import { ChunkUploadProgressOutput } from "./models/ChunkUploadProgressOutput";

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
      url: "/fileStorage/download",
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
  uploadLogo(data: FormData) {
    return axiosUtil.request<string>({
      url: "/fileStorage/uploadLogo",
      method: "post",
      data,
      cancelDuplicateRequest: false,
      requestType: "upload",
    });
  },
  /**
   * 上传头像
   */
  uploadAvatar(data: FormData) {
    return axiosUtil.request<string>({
      url: "/fileStorage/uploadAvatar",
      method: "post",
      data,
      cancelDuplicateRequest: false,
      requestType: "upload",
    });
  },
  /**
   * 上传证件照
   */
  uploadIdPhoto(data: FormData) {
    return axiosUtil.request<string>({
      url: "/fileStorage/uploadIdPhoto",
      method: "post",
      data,
      cancelDuplicateRequest: false,
      requestType: "upload",
    });
  },
  /**
   * 上传富文本
   */
  uploadEditor(data: FormData) {
    return axiosUtil.request<string>({
      url: "/fileStorage/uploadEditor",
      method: "post",
      data,
      cancelDuplicateRequest: false,
      requestType: "upload",
    });
  },
  /**
   * 上传文件
   */
  uploadFile(data: FormData) {
    return axiosUtil.request<string>({
      url: "/fileStorage/uploadFile",
      method: "post",
      data,
      cancelDuplicateRequest: false,
      requestType: "upload",
    });
  },
  /**
   * 初始化分片上传
   */
  initChunkUpload(data: InitChunkUploadInput) {
    return axiosUtil.request<InitChunkUploadOutput>({
      url: "/fileStorage/initChunkUpload",
      method: "post",
      data,
      requestType: "upload",
    });
  },
  /**
   * 上传分片
   */
  uploadChunk(data: FormData) {
    return axiosUtil.request({
      url: "/fileStorage/uploadChunk",
      method: "post",
      data,
      cancelDuplicateRequest: false,
      requestType: "upload",
    });
  },
  /**
   * 获取分片上传进度
   */
  getChunkUploadProgress(uploadId: string) {
    return axiosUtil.request<ChunkUploadProgressOutput>({
      url: "/fileStorage/getChunkUploadProgress",
      method: "get",
      params: { uploadId },
      requestType: "query",
    });
  },
  /**
   * 合并分片
   */
  mergeChunk(data: MergeChunkInput) {
    return axiosUtil.request<string>({
      url: "/fileStorage/mergeChunk",
      method: "post",
      data,
      requestType: "upload",
    });
  },
};
