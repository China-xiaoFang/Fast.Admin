/**
 * Fast.File.Applications.Dto.InitChunkUploadInput 初始化分片上传输入
 */
export interface InitChunkUploadInput {
  /**
   * 文件名称
   */
  fileName: string;
  /**
   * 文件大小（字节）
   */
  fileSize: number;
  /**
   * 分片大小（字节）
   */
  chunkSize: number;
  /**
   * 总分片数
   */
  totalChunks: number;
  /**
   * 文件MIME类型
   */
  contentType: string;
  /**
   * 文件哈希（MD5，用于秒传）
   */
  fileHash?: string;
}
