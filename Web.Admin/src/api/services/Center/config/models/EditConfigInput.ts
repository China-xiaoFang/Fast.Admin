/**
 * 编辑配置输入
 */
export interface EditConfigInput {
	/**
	 * 配置Id
	 */
	configId?: string;
	/**
	 * 配置名称
	 */
	configName?: string;
	/**
	 * 配置值
	 */
	configValue?: string;
	/**
	 * 备注
	 */
	remark?: string;
	/**
	 * 
	 */
	rowVersion?: string;
}

