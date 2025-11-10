import { type App } from "vue";

/** 挂载自定义指令 */
export function loadDirectives(app: App): void {
	const directivesList: any = {};

	Object.keys(directivesList).forEach((key) => {
		// 注册所有自定义指令
		app.directive(key, directivesList[key]);
	});
}
