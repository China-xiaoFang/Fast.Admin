import { CdnImportOptions } from "fast-vite-plugins";

export const getCdnModules = (viteDev: boolean): CdnImportOptions["modules"] => [
	{
		name: "vue",
		var: "Vue",
		path: viteDev ? "dist/vue.runtime.global.js" : "dist/vue.runtime.global.prod.js",
	},
	{
		name: "vue-demi",
		var: "VueDemi",
		path: "lib/index.iife.js",
	},
	{
		name: "@vueuse/shared",
		var: "VueUse",
		path: "dist/index.iife.min.js",
	},
	{
		name: "@vueuse/core",
		var: "VueUse",
		path: "dist/index.iife.min.js",
	},
	{
		name: "vue-router",
		var: "VueRouter",
		path: viteDev ? "dist/vue-router.global.js" : "dist/vue-router.global.prod.js",
	},
	{
		name: "dayjs",
		var: "dayjs",
		path: ["dayjs.min.js", "locale/zh-cn.js"],
	},
	{
		name: "@element-plus/icons-vue",
		var: "ElementPlusIconsVue",
		path: "dist/index.iife.min.js",
	},
	{
		name: "element-plus",
		var: "ElementPlus",
		path: ["dist/index.full.min.js", "dist/locale/zh-cn.min.js"],
		css: ["dist/index.css", "theme-chalk/dark/css-vars.css"],
	},
	{
		name: "@fast-china/utils",
		var: "FastUtils",
		path: "dist/index.global.min.js",
	},
	{
		name: "axios",
		var: "axios",
		path: "dist/axios.min.js",
	},
	{
		name: "@fast-china/axios",
		var: "FastAxios",
		path: "dist/index.global.min.js",
	},
	{
		name: "pinia",
		var: "Pinia",
		path: "dist/pinia.iife.prod.js",
	},
	{
		name: "pinia-plugin-persistedstate",
		var: "piniaPluginPersistedstate",
		path: "dist/index.iife.js",
	},
	{
		name: "@microsoft/signalr",
		var: "signalR",
		path: "dist/browser/signalr.min.js",
	},
	{
		name: "nprogress",
		var: "NProgress",
		path: "nprogress.js",
		css: "nprogress.css",
	},
	{
		name: "lodash",
		var: "_",
		path: "lodash.min.js",
	},
	{
		name: "@fast-element-plus/icons-vue",
		var: "FastElementPlusIconsVue",
		path: "dist/index.iife.min.js",
	},
	{
		name: "fast-element-plus",
		var: "FastElementPlus",
		path: "dist/index.full.min.js",
		css: "dist/styles/index.css",
	},
];
