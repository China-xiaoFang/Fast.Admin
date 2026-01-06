import { type App } from "vue";
import { vAuth } from "./modules/auth";

/** 挂载自定义指令 */
export function loadDirectives(app: App): void {
	const directivesList = [vAuth];

	directivesList.forEach((d) => app.use(d));
}
