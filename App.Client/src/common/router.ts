/**
 * @description 常用路由
 */
export const CommonRoute = {
	/** WebView页面 */
	WebView: "/pages/webView/index",
	/** 用户协议 */
	UserAgreement: "/pages/agreement/user/index",
	/** 隐私协议 */
	PrivacyAgreement: "/pages/agreement/privacy/index",
	/** 服务协议 */
	ServiceAgreement: "/pages/agreement/service/index",
	/** 投诉建议 */
	ComplaintPage: "/pages/complaint/page/index",
	/** 投诉建议 */
	ComplaintSubmit: "/pages/complaint/submit/index",
	/** 首页 */
	Home: "/pages/tabBar/home/index",
	/** 个人中心 */
	My: "/pages/tabBar/my/index",
};

/**
 * @description TabBar 路由
 */
export const TabBarRoute = [CommonRoute.Home, CommonRoute.My];
