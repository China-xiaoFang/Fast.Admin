import type { ComponentResolveResult, ComponentResolver } from "@uni-helper/vite-plugin-uni-components";

export const FastResolver = (): ComponentResolver => {
	return {
		type: "component",
		resolve: (name: string): ComponentResolveResult => {
			// src/components
			if (name.match(/^Fa[A-Z]/)) {
				// 删除第一个字符 Fa，并将剩余部分的首字母小写
				const compName = name.charAt(2).toLowerCase() + name.slice(3);
				return {
					name,
					from: `@/components/${compName}/index.vue`,
				};
			}
		},
	};
};
