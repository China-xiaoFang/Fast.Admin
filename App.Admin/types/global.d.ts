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

	/** @description 日期范围 */
	type FaTableDataRange = "Past1D" | "Past3D" | "Past1W" | "Past1M" | "Past3M" | "Past6M" | "Past1Y" | "Past3Y";

	/** @description FaTable 表格枚举列上下文 */
	type FaTableEnumColumnCtx = {
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
	};

	/** @description FaTable 统一分页返回结果类 */
	type PagedResult<Output = any> = {
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
		rows?: Array<Output>;
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
	};

	/** @description FaTable 分页搜索类型枚举 */
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

	/** @description FaTable 统一分页搜索输入 */
	type PagedSearchInput = {
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
	};

	/** @description FaTable 统一分页排序输入 */
	type PagedSortInput = {
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
	};

	/** @description FaTable 统一分页输入 */
	type PagedInput = {
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
		searchTimeList?: Array<Date | string>;
		/**
		 * 搜索集合
		 */
		searchList?: Array<PagedSearchInput>;
		/**
		 * 排序集合
		 */
		sortList?: Array<PagedSortInput>;
		/**
		 * 启用分页
		 * @default true
		 */
		enablePaged?: boolean;
	};

	/** @description 选择器数据 */
	type ElSelectorOutput<T = any> = T extends string | number | boolean | object
		? {
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
				data?: any;
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
				children?: Array<ElSelectorOutput<T>>;
				[key: string]: any;
			}
		: never;

	/** @description 树形数据 */
	type ElTreeOutput<T = any> = T extends string | number | boolean | object
		? {
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
				data?: any;
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
				children?: Array<ElTreeOutput<T>>;
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
		: never;
}

export {};
