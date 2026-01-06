/**
 * Sugar数据库类型枚举
 */
export enum SugarDbType {
  /**
   * MySql
   */
  MySql = 0,
  /**
   * SqlServer
   */
  SqlServer = 1,
  /**
   * SqLite
   */
  Sqlite = 2,
  /**
   * Oracle
   */
  Oracle = 3,
  /**
   * PostgreSQL
   */
  PostgreSQL = 4,
  /**
   * 达梦
   */
  Dm = 5,
  /**
   * Kdbndp
   */
  Kdbndp = 6,
  /**
   * Oscar
   */
  Oscar = 7,
  /**
   * MySql Connector
   */
  MySqlConnector = 8,
  /**
   * Microsoft Access
   */
  Access = 9,
  /**
   * OpenGauss
   */
  OpenGauss = 10,
  /**
   * QuestDB时序数据库
   */
  QuestDB = 11,
  /**
   * HG
   */
  HG = 12,
  /**
   * ClickHouse列式数据库
   */
  ClickHouse = 13,
  /**
   * 南大通用GBase
   */
  GBase = 14,
  /**
   * ODBC
   */
  Odbc = 15,
  /**
   * 蚂蚁OceanBase
   */
  OceanBaseForOracle = 16,
  /**
   * TDengine时序数据库
   */
  TDengine = 17,
  /**
   * 华为GaussDB
   */
  GaussDB = 18,
  /**
   * 蚂蚁OceanBase
   */
  OceanBase = 19,
  /**
   * PingCAP TiDB分布式数据库
   */
  Tidb = 20,
  /**
   * 海量数据Vastbase
   */
  Vastbase = 21,
  /**
   * 阿里云PolarDB
   */
  PolarDB = 22,
  /**
   * Apache Doris
   */
  Doris = 23,
  /**
   * 虚谷数据库
   */
  Xugu = 24,
  /**
   * 中兴通讯GoldenDB
   */
  GoldenDB = 25,
  /**
   * 腾讯云TDSQL PostgreSQL版ODBC
   */
  TDSQLForPGODBC = 26,
  /**
   * 腾讯云TDSQL
   */
  TDSQL = 27,
  /**
   * SAP HANA
   */
  HANA = 28,
  /**
   * IBM DB2
   */
  DB2 = 29,
  /**
   * 华为GaussDB
   */
  GaussDBNative = 30,
  /**
   * DuckDB
   */
  DuckDB = 31,
  /**
   * MongoDB
   */
  MongoDb = 32,
  /**
   * 自定义
   */
  Custom = 900,
}
