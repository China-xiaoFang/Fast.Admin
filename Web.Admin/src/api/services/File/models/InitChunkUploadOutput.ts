/**
 * Fast.File.Applications.Dto.InitChunkUploadOutput 初始化分片上传输出
 */
export interface InitChunkUploadOutput {
  /**
   * 上传标识
   */
  uploadId: string;
  /**
   * 是否秒传（文件已存在）
   */
  uploaded: boolean;
  /**
   * 文件访问地址（秒传时返回）
   */
  fileLocation?: string;
}
