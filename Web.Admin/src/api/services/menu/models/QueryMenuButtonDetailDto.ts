import { EditionEnum } from "@/api/enums/EditionEnum";
import { CommonStatusEnum } from "@/api/enums/CommonStatusEnum";

/**
 * Fast.Center.Service.Menu.Dto.QueryMenuDetailOutput.QueryMenuButtonDetailDto 获取菜单按钮详情Dto
 */
export interface QueryMenuButtonDetailDto {
  /**
   * 按钮Id
   */
  buttonId?: number;
  /**
   * 
   */
  edition?: EditionEnum;
  /**
   * 按钮编码
   */
  buttonCode?: string;
  /**
   * 按钮名称
   */
  buttonName?: string;
  /**
   * 是否桌面端
   */
  hasDesktop?: boolean;
  /**
   * 是否Web端
   */
  hasWeb?: boolean;
  /**
   * 是否移动端
   */
  hasMobile?: boolean;
  /**
   * 排序
   */
  sort?: number;
  /**
   * 
   */
  status?: CommonStatusEnum;
}

