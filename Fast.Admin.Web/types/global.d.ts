declare global {
	/**
	 * 页面状态类型
	 */
	type IPageStateType = "detail" | "add" | "edit" | "copy";
	
	/** 解决 element-plus CDN 引用国际化，TS环境找不到变量的问题*/
	/**
	 * element-plus 中文语言包
	 */
	declare const ElementPlusLocaleZhCn: import("element-plus/es/locale").Language;
}

export {};
