import fastChinaFlat from "@fast-china/eslint-config/flat";
import { importUseLodashRules } from "@fast-china/eslint-config/rules";
import { defineConfig } from "eslint/config";

export default defineConfig(
	...fastChinaFlat,
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
	}
);
