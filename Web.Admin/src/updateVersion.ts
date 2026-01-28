import { ElMessage, ElMessageBox } from "element-plus";
import { consoleLog, consoleWarn } from "@fast-china/utils";

/**
 * 是否存在版本更新实例
 */
let existsVersionUpdateInstance = false;

/**
 * 检测版本更新
 */
const versionUpdate = async (version: string): Promise<void> => {
	consoleLog("App", `当前版本 ${version}`);
	const response = await fetch(`/version.json?_=${Date.now()}`);
	if (!response.ok) {
		consoleWarn("App", "更新检测失败");
		ElMessage.error("更新检测失败！");
		return Promise.reject();
	}
	const apiRes = await response.json();
	if (version !== apiRes.version) {
		// 判断是否存在版本更新实例弹窗
		if (existsVersionUpdateInstance) return;
		existsVersionUpdateInstance = true;
		consoleLog("App", `发现新版本 ${apiRes.version}`);
		ElMessageBox.confirm(`发现新版本 ${apiRes.version}，是否立即更新？`, {
			type: "warning",
			confirmButtonText: "更新",
			closeOnClickModal: false,
		})
			.then(() => {
				consoleLog("App", `更新版本 ${apiRes.version}`);
				// 强制刷新浏览器
				window.location.reload();
			})
			.catch(() => {
				existsVersionUpdateInstance = false;
				consoleWarn("App", `取消更新版本 ${apiRes.version}`);
				ElMessage.warning({
					message: "您取消了更新，将在十分钟后再次进行提示！",
				});
			});
	}
};

/**
 * 检测版本更新
 * @default 默认10分钟一次
 */
export const checkVersionUpdate = (version: string, delay = 10 * 60 * 1000): void => {
	versionUpdate(version);
	setInterval(() => {
		versionUpdate(version);
	}, delay);
};
