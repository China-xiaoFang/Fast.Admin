import fastChina from "@fast-china/eslint-config";
import { defineConfig } from "eslint/config";

export default defineConfig(
	...fastChina,
	{
		name: "fast/uni-app/vue",
		languageOptions: {
			globals: {
				// Uni-App 的条件编译全局变量
				APP_PLUS: true,
				APP_ANDROID: true,
				APP_IOS: true,
				H5: true,
				MP_WEIXIN: true,
				MP_ALIPAY: true,
				MP_BAIDU: true,
				MP_TOUTIAO: true,
				MP_QQ: true,
				MP_JD: true,
				MP_KUAISHOU: true,
				MP_XHS: true,
				MP_LARK: true,
				MP_DINGTALK: true,
				MP_TAOBAO: true,
				MP_ALIPAY_LIFE: true,
				QUICKAPP_HUAWEI: true,
				QUICKAPP_LENOVO: true,
				QUICKAPP_MI: true,
				MP_HARMONY: true,
			},
		},
	},
	{
		name: "fast/ignores",
		ignores: ["src/api"],
	},
	{
		name: "@fast-china/typescript/components",
		files: ["src/components/**", "src/pages_**/components/**"],
		rules: {
			// 允许定义未使用的变量
			"no-unused-vars": "off",
		},
	},
	{
		name: "@fast-china/json/pages",
		files: ["**/src/pages.json"],
		rules: {
			// 允许注释
			"jsonc/no-comments": "off",
		},
	}
);
