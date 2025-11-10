/// <reference types="vite/client" />
import { resolve } from "path";
import Uni from "@dcloudio/vite-plugin-uni";
import UniHelperComponents from "@uni-helper/vite-plugin-uni-components";
import { WotResolver } from "@uni-helper/vite-plugin-uni-components/resolvers";
import UniHelperLayouts from "@uni-helper/vite-plugin-uni-layouts";
import UniHelperPages from "@uni-helper/vite-plugin-uni-pages";
import { type ConfigEnv, type UserConfig, loadEnv } from "vite";
import { FastResolver } from "./resolver.fast";
import { ZPagingResolver } from "./resolver.zPaging";

const pathResolve = (dir: string): any => {
	return resolve(__dirname, ".", dir);
};

/** 配置项文档：https://cn.vitejs.dev/config */
const ViteConfig = ({ mode }: ConfigEnv): UserConfig => {
	const viteEnv = loadEnv(mode, process.cwd()) as ImportMetaEnv;
	const viteDev = viteEnv.DEV || viteEnv.VITE_ENV === "development";

	// 配置别名
	const alias: Record<string, string> = {
		"@": pathResolve("./src"),
		static: pathResolve("./src/static"),
	};

	return {
		root: process.cwd(),
		resolve: { alias },
		build: {
			/** Vite 2.6.x 以上需要配置 minify: "terser", terserOptions 才能生效 */
			/** 生产环境压缩代码，开发环境不压缩 */
			minify: viteDev ? false : "terser",
			/** 在打包代码时移除 console.log、debugger 和 注释 */
			terserOptions: {
				compress: {
					/** 移除所有 console.* 调用 */
					drop_console: !viteDev,
					/** 移除 debugger 语句 */
					drop_debugger: !viteDev,
					/** 仅移除 console.log */
					pure_funcs: viteDev ? [] : ["console.log"],
				},
				format: {
					/** 删除注释 */
					comments: viteDev,
					/** 格式化代码 */
					beautify: viteDev,
				},
				/** 混淆变量名 */
				mangle: !viteDev,
			},
			/** 启用/禁用 CSS 代码拆分 */
			cssCodeSplit: false,
			/** 关闭生成 map 文件，可以达到缩小打包体积 */
			sourcemap: false,
			/** 打包输出路径 */
			outDir: viteEnv.VITE_OUT_DIR,
			/** 打包的时候清空目录 */
			emptyOutDir: true,
			/** 0kb，小于此阈值的导入或引用资源将内联为 base64 编码 */
			assetsInlineLimit: 0,
		},
		/** Vite 插件 */
		plugins: [
			/** 基于文件的路由 */
			UniHelperPages({
				outDir: "src",
				dts: "types/pages.d.ts",
				homePage: "pages/tabBar/home/index",
				dir: "src/pages",
				subPackages: [],
			}),
			/** Layout 模式 */
			UniHelperLayouts(),
			/** 组件自动化加载 */
			UniHelperComponents({
				/** 避免自动引入，所以修改为未知文件夹 */
				dirs: "/src/xxx",
				resolvers: [WotResolver(), ZPagingResolver(), FastResolver()],
				dts: "types/components.d.ts",
				directoryAsNamespace: true,
			}),
			Uni(),
		],
	};
};

export default ViteConfig;
