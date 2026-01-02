import { DbType } from "@/api/enums/DbType";
import { ParameterDirection } from "@/api/enums/ParameterDirection";
import { DataRowVersion } from "@/api/enums/DataRowVersion";

/**
 * 
 */
export interface SugarParameter {
  /**
   * 
   */
  _Size?: number;
  /**
   * 
   */
  isRefCursor?: boolean;
  /**
   * 
   */
  isClob?: boolean;
  /**
   * 
   */
  isNClob?: boolean;
  /**
   * 
   */
  isNvarchar2?: boolean;
  /**
   * 
   */
  dbType?: DbType;
  /**
   * 
   */
  direction?: ParameterDirection;
  /**
   * 
   */
  isNullable?: boolean;
  /**
   * 
   */
  parameterName?: string;
  /**
   * 
   */
  scale?: number;
  /**
   * 
   */
  size?: number;
  /**
   * 
   */
  sourceColumn?: string;
  /**
   * 
   */
  sourceColumnNullMapping?: boolean;
  /**
   * 
   */
  udtTypeName?: string;
  /**
   * 
   */
  value?: any;
  /**
   * 
   */
  tempDate?: any;
  /**
   * 
   */
  sourceVersion?: DataRowVersion;
  /**
   * 
   */
  typeName?: string;
  /**
   * 
   */
  isJson?: boolean;
  /**
   * 
   */
  isArray?: boolean;
  /**
   * 
   */
  customDbType?: any;
  /**
   * 
   */
  precision?: number;
}

