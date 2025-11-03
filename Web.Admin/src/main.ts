import { createApp } from "vue";
import "./styles/index.scss";
import App from "./App.vue";
import { loadDirectives } from "./directives";
import { loadPlugins } from "./plugins";
import router from "./router";
import { loadPinia, useApp } from "./stores";
import { checkVersionUpdate } from "./updateVersion";
if (import.meta.env.DEV) {
	await import("element-plus/dist/index.css");
	await import("element-plus/theme-chalk/dark/css-vars.css");
	await import("fast-element-plus/styles/index.scss");
}

let app = createApp(App);

async function start(): Promise<void> {
	// 注册持久化存储
	loadPinia(app);

	/** 加载插件 */
	loadPlugins(app);

	/** Launch */
	await useApp().launch();

	/** 加载路由 */
	app.use(router);

	/** 加载自定义指令 */
	loadDirectives(app);

	app.mount("#app");

	checkVersionUpdate(`v${import.meta.env.VITE_APP_VERSION}`);
}

start();

/** 刷新应用 */
export const refreshApp = (): void => {
	app.unmount();
	app = createApp(App);
	start();
};
