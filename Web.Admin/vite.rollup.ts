export const rollupManualChunks = (id: string): string => {
	if (/[/\\]node_modules[/\\]/.test(id)) {
		const parts = id.replace(/\\/g, "/").split("/node_modules/");

		if (parts.length < 2) return null;

		// 最后一个 node_modules 后面的
		const tail = parts.at(-1);
		const sage = tail.split("/");

		// 作用域包
		if (sage[0].startsWith("@") && sage.length > 1) {
			return `_node_modules_${sage[0]}_${sage[1]}`;
		}

		// 普通包
		return `_node_modules_${sage[0]}`;
	}

	for (const { path, file } of [
		{
			path: "/src/api/enums",
			file: "_enums",
		},
		{
			path: "/src/api/services",
			file: "_services",
		},
		{
			path: "/src/components",
			file: "_components",
		},
		{
			path: "/src/directives",
			file: "_directives",
		},
		{
			path: "/src/icons",
			file: "_icons",
		},
		{
			path: "/src/layouts",
			file: "_layouts",
		},
		{
			path: "/src/router",
			file: "_router",
		},
		{
			path: "/src/stores",
			file: "_stores",
		},
		{
			path: "/src/utils",
			file: "_utils",
		},
		{
			path: "/src/views/common",
			file: "_view_common",
		},
		{
			path: "/src/views/{0}/{1}",
			file: "_view_{0}_{1}",
		},
		{
			path: "/src/views/{0}",
			file: "_view_{0}",
		},
		{
			path: ["/src/App.vue", "/src/main.ts", "/src/plugins/"],
			file: "_main",
		},
	]) {
		const paths = Array.isArray(path) ? path : [path];
		for (const pattern of paths) {
			// 去掉查询参数 ?xxx，去掉 index.vue
			const normalizedId = id.split("?")[0].replace(/\/index\.vue$/, "");
			// 将配置中的占位符 {n} 转为正则捕获组 ([^/]+)
			const regexStr = pattern.replace(/\{(\d+)\}/g, "([^/]+)");
			const regex = new RegExp(regexStr);
			const match = normalizedId.match(regex);
			if (match) {
				// 用捕获到的组替换文件名模板中的 {n}
				const fileName = file.replace(/\{(\d+)\}/g, (_, index) => {
					return match[Number(index) + 1] || "";
				});
				return fileName;
			}
		}
	}
};
