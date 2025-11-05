import { kebabCase } from "@uni-helper/vite-plugin-uni-components";
import type { ComponentResolveResult, ComponentResolver } from "@uni-helper/vite-plugin-uni-components";

const excludedComponents = ["z-paging-refresh", "z-paging-load-more"];

export const ZPagingResolver = (): ComponentResolver => {
	return {
		type: "component",
		resolve: (name: string): ComponentResolveResult => {
			// z-paging
			if (name.match(/^Z[A-Z]/)) {
				const compName = kebabCase(name);
				if (excludedComponents.includes(compName)) return;
				// 排除 组件
				return {
					name,
					from: `z-paging/components/${compName}/${compName}.vue`,
				};
			}
		},
	};
};
