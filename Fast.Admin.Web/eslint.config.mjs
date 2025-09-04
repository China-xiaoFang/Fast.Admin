import fastChinaFlat from "@fast-china/eslint-config/flat";
import { defineConfig } from "eslint/config";

export default defineConfig(...fastChinaFlat,
	{
		name: "fast/ignores",
		ignores: ["src/icons", "src/components/index.ts"],
	},
	{
		name: "fast/import/lodash",
		files: ["src/**"],
		rules: {
			...importUseLodashRules,
		},
	},);
