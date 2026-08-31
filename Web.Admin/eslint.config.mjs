import fastChina from "@fast-china/eslint-config";
import { createLodashConfigs, createMarkdownConfigs } from "@fast-china/eslint-config/configs";
import { defineConfig } from "eslint/config";

export default defineConfig([
	...fastChina,
	...createMarkdownConfigs(),
	...createLodashConfigs("lodash"),
	{
		name: "fast-admin/linter-options",
		linterOptions: {
			// 检查没有改变规则状态的内联 ESLint 配置。
			reportUnusedInlineConfigs: "error",
		},
	},
	{
		name: "fast-admin/web",
		files: ["**/*.{js,mjs,cjs,jsx,ts,mts,cts,tsx,vue}"],
		rules: {
			// 禁止动态执行字符串代码。
			"no-eval": "error",
			// 禁止使用 javascript: URL。
			"no-script-url": "error",
			// Promise executor 的返回值不会被使用。
			"no-promise-executor-return": "error",
			// 检查使用普通字符串引号书写的模板表达式。
			"no-template-curly-in-string": "error",
		},
	},
	{
		name: "fast-admin/typescript",
		files: ["**/*.{ts,mts,cts,tsx,vue}"],
		rules: {
			// 禁止显式声明 any。
			"@typescript-eslint/no-explicit-any": "error",
			// 允许将 any 类型的值赋值给其他变量。
			"@typescript-eslint/no-unsafe-assignment": "off",
			// 允许将 any 类型的值作为函数参数传递。
			"@typescript-eslint/no-unsafe-argument": "off",
			// 允许调用 any 类型的值。
			"@typescript-eslint/no-unsafe-call": "off",
			// 允许访问 any 类型值的属性和方法。
			"@typescript-eslint/no-unsafe-member-access": "off",
			// 允许函数返回 any 类型的值。
			"@typescript-eslint/no-unsafe-return": "off",
			// 允许 Vue 模板和 TSX 属性使用异步事件处理函数。
			"@typescript-eslint/no-misused-promises": [
				"error",
				{
					checksVoidReturn: {
						attributes: false,
					},
				},
			],
			// 公共模块边界必须显式声明返回类型。
			"@typescript-eslint/explicit-module-boundary-types": ["error", { allowArgumentsExplicitlyTypedAsAny: false }],
			// 非 Vue 文件中的函数必须显式声明返回类型。
			"@typescript-eslint/explicit-function-return-type": "error",
			// 类型导出统一使用 export type。
			"@typescript-eslint/consistent-type-exports": "error",
			// 类型导入统一生成独立的 import type 语句。
			"@typescript-eslint/no-import-type-side-effects": "error",
			// 可以使用可选链时统一使用可选链。
			"@typescript-eslint/prefer-optional-chain": "error",
		},
	},
	{
		name: "fast-admin/vue",
		files: ["**/*.vue"],
		rules: {
			// Vue 组件内部方法允许使用 TypeScript 返回类型推断。
			"@typescript-eslint/explicit-module-boundary-types": "off",
			"@typescript-eslint/explicit-function-return-type": "off",
			// 禁止在 setup 中以丢失响应性的方式使用 Props。
			"vue/no-setup-props-reactivity-loss": "error",
		},
	},
]);
