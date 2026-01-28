import fastChina from "@fast-china/eslint-config";
import { importUseLodashRules } from "@fast-china/eslint-config/rules";
import { defineConfig } from "eslint/config";

export default defineConfig(
	...fastChina,
	{
		name: "fast/ignores",
		ignores: ["src/api", "src/icons"],
	},
	{
		name: "fast/import/lodash",
		files: ["src/**"],
		rules: {
			...importUseLodashRules,
		},
	},
	{
		name: "@fast-china/typescript/md/js",
		files: ["src/components/**"],
		rules: {
			// 允许定义未使用的变量
			"no-unused-vars": "off",
		},
	}
);
