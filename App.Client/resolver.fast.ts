import { existsSync } from "fs";
import { relative, resolve } from "path";
import fg from "fast-glob";
import type { ComponentResolveResult, ComponentResolver } from "@uni-helper/vite-plugin-uni-components";

const rootDir = resolve(__dirname, "./src");

export const FastResolver = (): ComponentResolver => {
	return {
		type: "component",
		resolve: (name: string): ComponentResolveResult => {
			if (!name.match(/^Fa[A-Z]/)) return;

			// 删除第一个字符 Fa，并将剩余部分的首字母小写
			const compName = name.charAt(2).toLowerCase() + name.slice(3);

			// src/components
			const mainPath = resolve(__dirname, `src/components/${compName}/index.vue`);
			if (existsSync(mainPath)) {
				return {
					name,
					from: `@/components/${compName}/index.vue`,
				};
			}

			// 分包
			const subPath = fg.sync(`src/pages_**/components/${compName}/index.vue`, { absolute: true });
			if (subPath.length > 0) {
				const relativePath = relative(rootDir, subPath[0]).replace(/\\/g, "/");
				return {
					name,
					from: `@/${relativePath}`,
				};
			}
		},
	};
};
