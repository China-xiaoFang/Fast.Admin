import { PagedInput } from "fast-element-plus";
import { EmployeeStatusEnum } from "@/api/enums/EmployeeStatusEnum";
import { GenderEnum } from "@/api/enums/GenderEnum";
import { NationEnum } from "@/api/enums/NationEnum";
import { EducationLevelEnum } from "@/api/enums/EducationLevelEnum";
import { PoliticalStatusEnum } from "@/api/enums/PoliticalStatusEnum";
import { AcademicQualificationsEnum } from "@/api/enums/AcademicQualificationsEnum";
import { AcademicSystemEnum } from "@/api/enums/AcademicSystemEnum";
import { DegreeEnum } from "@/api/enums/DegreeEnum";

/**
 * Fast.Admin.Service.Employee.Dto.QueryEmployeePagedInput 获取职员分页列表输入
 */
export interface QueryEmployeePagedInput extends PagedInput  {
  /**
   * 
   */
  status?: EmployeeStatusEnum;
  /**
   * 
   */
  sex?: GenderEnum;
  /**
   * 
   */
  nation?: NationEnum;
  /**
   * 籍贯
   */
  nativePlace?: string;
  /**
   * 
   */
  educationLevel?: EducationLevelEnum;
  /**
   * 
   */
  politicalStatus?: PoliticalStatusEnum;
  /**
   * 毕业学院
   */
  graduationCollege?: string;
  /**
   * 
   */
  academicQualifications?: AcademicQualificationsEnum;
  /**
   * 
   */
  academicSystem?: AcademicSystemEnum;
  /**
   * 
   */
  degree?: DegreeEnum;
  /**
   * 部门Id
   */
  departmentId?: number;
  /**
   * 
   */
  readonly isOrderBy?: boolean;
}

