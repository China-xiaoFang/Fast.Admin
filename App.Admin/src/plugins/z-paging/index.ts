/** 加载 ZPaging */
export function loadZPaging(): void {
	uni.$zp = {
		config: {
			// 默认 class
			"paging-class": "z-paging__page",
			// 分页默认 pageIndex 为 1
			"default-page-no": 1,
			// 分页默认 pageSize 为 15
			"default-page-size": 15,
			// 默认不适使用 fixed 布局
			fixed: false,
			// 默认开启底部安全区域适配
			"safe-area-inset-bottom": true,
			// 默认使用页面滚动
			"use-page-scroll": true,

			/** --------reload相关配置-------- */
			// 列表刷新时自动显示下拉刷新view
			"show-refresher-when-reload": true,
			/** --------reload相关配置-------- */

			/** --------下拉刷新配置-------- */
			// 显示最后更新时间
			"show-refresher-update-time": true,
			/** --------reload相关配置-------- */

			/** --------空数据与加载失败-------- */
			// 默认空数据图描述文字
			"empty-view-text": "暂无数据~",
			// 空数据图点击重新加载文字
			"empty-view-reload-text": "立即重试",
			// 空数据图“加载失败”描述文字
			"empty-view-error-text": "加载失败，请稍后重试~",
			/** --------空数据与加载失败-------- */

			/** --------返回顶部按钮-------- */
			// 显示点击返回顶部按钮
			"auto-show-back-to-top": true,
			/** --------返回顶部按钮-------- */

			/** --------虚拟列表-------- */
			// 强制关闭inner-list
			"force-close-inner-list": true,
			/** --------虚拟列表-------- */
		},
	};
}
