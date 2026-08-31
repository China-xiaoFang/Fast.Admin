declare global {
	/** Vite 环境 */
	type ViteEnv = "production" | "development" | "test" | "staging";

	/** 网络类型 */
	type INetworkType = "wifi" | "2g" | "3g" | "4g" | "5g" | "ethernet" | "unknown" | "none";

	/** TabBar */
	type ITabBar = {
		/** 路径 */
		path: string;
		/** 图标 */
		icon: string;
		/** 标题 */
		title: string;
		/** 凸起 */
		bulge?: boolean;
		/** 禁用，锁定 */
		disable?: boolean;
	};

	/** FaTable 默认时间搜索支持的快捷日期范围。 */
	type FaTableDataRange = "Past1D" | "Past3D" | "Past1W" | "Past1M" | "Past3M" | "Past6M" | "Past1Y" | "Past3Y";

	/** FaTable 枚举列的选项配置。 */
	interface FaTableEnumColumnCtx {
		/**
		 * 选项框显示的文字
		 */
		label: string;
		/**
		 * 选项框值
		 */
		value: string | number | boolean;
		/**
		 * 显示
		 */
		show?: boolean;
		/**
		 * 是否禁用此选项
		 */
		disabled?: boolean;
		/**
		 * 为树形选择是，可以通过 children 属性指定子选项
		 */
		children?: FaTableEnumColumnCtx[];
		/**
		 * 提示
		 */
		tips?: string;
		/**
		 * Tag的类型，默认 "primary"
		 */
		type?: "primary" | "success" | "info" | "warning" | "danger";

		[key: string]: any;
	}

	/** FaTable 统一分页返回结果。 */

	export interface PagedResult<Output = Record<string, any>> {
		/**
		 * 当前页
		 */
		pageIndex?: number;
		/**
		 * 当前页码
		 */
		pageSize?: number;
		/**
		 * 总页数
		 */
		totalPage?: number;
		/**
		 * 总条数
		 */
		totalRows?: number;
		/**
		 * Data
		 */
		rows?: Output[];
		/**
		 * 是否有上一页
		 */
		hasPrevPages?: boolean;
		/**
		 * 是否有下一页
		 */
		hasNextPages?: boolean;
		/**
		 * 程序集名称
		 */
		assemblyName?: string;
		/**
		 * 完全限定名称
		 */
		fullName?: string;
	}

	/** FaTable 分页搜索运算符。 */
	enum PagedSearchTypeEnum {
		/**
		 * 模糊匹配
		 */
		Like = 1,
		/**
		 * 等于
		 */
		Equal = 2,
		/**
		 * 不等于
		 */
		NotEqual = 3,
		/**
		 * 大于
		 */
		GreaterThan = 4,
		/**
		 * 大于等于
		 */
		GreaterThanOrEqual = 5,
		/**
		 * 小于
		 */
		LessThan = 6,
		/**
		 * 小于等于
		 */
		LessThanOrEqual = 7,
		/**
		 * 包含
		 */
		Include = 8,
		/**
		 * 排除
		 */
		NotInclude = 9,
	}

	/** FaTable 单个分页搜索条件。 */
	interface PagedSearchInput {
		/**
		 * 搜索字段英文
		 */
		enField?: string;
		/**
		 * 搜索字段中文
		 */
		cnField?: string;
		/**
		 * 搜索值
		 */
		value?: string;
		/**
		 * 搜索类型
		 */
		type?: PagedSearchTypeEnum;
	}

	/** FaTable 单个分页排序条件。 */
	interface PagedSortInput {
		/**
		 * 排序字段英文
		 */
		enField?: string;
		/**
		 * 排序字段中文
		 */
		cnField?: string;
		/**
		 * 排序方法
		 * 'ascending' | 'descending'
		 */
		mode?: string;
	}

	/** FaTable 分页、搜索和排序请求参数。 */
	interface PagedInput {
		/**
		 * 当前页面索引值，默认为1
		 */
		pageIndex?: number;
		/**
		 * 页码容量
		 */
		pageSize?: number;
		/**
		 * 搜索值
		 */
		searchValue?: string;
		/**
		 * 搜索时间
		 */
		searchTimeList?: (Date | string)[];
		/**
		 * 搜索集合
		 */
		searchList?: PagedSearchInput[];
		/**
		 * 排序集合
		 */
		sortList?: PagedSortInput[];
		/**
		 * 启用分页
		 * @default true
		 */
		enablePaged?: boolean;
		/** 业务接口附加的查询字段。 */

		[key: string]: any;
	}

	/** 选择器标准化后的选项数据。 */

	interface ElSelectorOutput<T = ElSelectorValue, Data = any> {
		/**
		 * 显示
		 */
		label?: string;
		/**
		 * 值
		 */
		value?: T;
		/**
		 * 附加数据
		 */
		data?: Data;
		/**
		 * 是否隐藏
		 */
		hide?: boolean;
		/**
		 * 是否禁用
		 */
		disabled?: boolean;
		/**
		 * 子节点
		 */
		children?: ElSelectorOutput<T, Data>[];

		[key: string]: any;
	}

	/** 树组件标准化后的节点数据。 */

	interface ElTreeOutput<T = ElTreeValue, Data = any> {
		/**
		 * 显示
		 */
		label?: string;
		/**
		 * 值
		 */
		value?: T;
		/**
		 * 附加数据
		 */
		data?: Data;
		/**
		 * 是否隐藏
		 */
		hide?: boolean;
		/**
		 * 是否禁用
		 */
		disabled?: boolean;
		/**
		 * 子节点
		 */
		children?: ElTreeOutput<T, Data>[];
		/**
		 * 是否显示数量
		 */
		showQuantity?: boolean;
		/**
		 * 数量
		 */
		quantity?: number;

		[key: string]: any;
	}
}

export {};
