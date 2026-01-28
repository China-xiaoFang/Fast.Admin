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
	}
);
