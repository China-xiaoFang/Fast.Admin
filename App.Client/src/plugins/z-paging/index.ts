/** 加载 ZPaging */
export function loadZPaging(): void {
	uni.$zp = {
		config: {
			// 分页默认 pageIndex 为 1
			defaultPageNo: 1,
			// 分页默认 pageSize 为 15
			defaultPageSize: 15,
			// 默认不适用 fixed 布局
			fixed: false,
			// 默认开启底部安全区域适配
			safeAreaInsetBottom: true,
			// 默认空数据图描述文字
			emptyViewText: "暂无数据",

			// 强制关闭inner-list
			forceCloseInnerList: true,

			// 默认显示点击返回顶部按钮
			autoShowBackToTop: true,
		},
	};
}
