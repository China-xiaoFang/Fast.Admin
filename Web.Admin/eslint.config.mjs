import { vueConfig } from "@fast-china/eslint-config";
import { createLodashConfigs, createMarkdownConfigs } from "@fast-china/eslint-config/configs";
import { defineConfig, globalIgnores } from "eslint/config";

export default defineConfig([
	...vueConfig,
	...createMarkdownConfigs(),
	...createLodashConfigs("lodash"),
	globalIgnores(["src/api/**"], "fast-admin/ignores"),
	{
		name: "fast-admin/linter-options",
		linterOptions: {
			// 检查未实际禁用任何问题的 ESLint 禁用指令。
			reportUnusedDisableDirectives: "error",
			// 检查没有改变规则状态的内联 ESLint 配置。
			reportUnusedInlineConfigs: "error",
		},
	},
	{
		name: "fast-admin/web",
		files: ["**/*.{js,mjs,cjs,jsx,ts,mts,cts,tsx,vue}"],
		rules: {
			// 禁止使用 javascript: URL。
			"no-script-url": "error",
			// 检查使用普通字符串引号书写的模板表达式。
			"no-template-curly-in-string": "error",
			// 禁止在使用 target="_blank" 时缺少安全的 rel 属性。
			"vue/no-template-target-blank": "error",
			// 要求原生 button 元素显式指定 type 属性。
			"vue/html-button-has-type": "error",
			// 检查已声明但未使用的模板 ref。
			"vue/no-unused-refs": "warn",
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
		},
	},
]);
