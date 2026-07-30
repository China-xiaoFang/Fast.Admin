/**
 * Fast.File.Applications.Dto.ChunkUploadProgressOutput 分片上传进度输出
 */
export interface ChunkUploadProgressOutput {
  /**
   * 上传标识
   */
  uploadId: string;
  /**
   * 文件名称
   */
  fileName: string;
  /**
   * 总分片数
   */
  totalChunks: number;
  /**
   * 已上传分片索引列表
   */
  uploadedChunks: number[];
  /**
   * 是否全部上传完成
   */
  isComplete: boolean;
}
