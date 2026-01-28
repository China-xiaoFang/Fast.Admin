import * as Pinia from "pinia";
import { createSSRApp } from "vue";
import App from "./App.vue";
import { loadPlugins } from "./plugins";
import router from "./router";
import { loadPinia } from "./stores";
import "./styles/index.scss";

export function createApp(): any {
	const app = createSSRApp(App);

	// 注册持久化存储
	loadPinia(app);

	loadPlugins(app);

	app.use(router);

	return {
		app,
		Pinia,
	};
}
