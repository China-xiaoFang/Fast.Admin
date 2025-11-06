<script setup lang="ts">
import { onLaunch } from "@dcloudio/uni-app";
import { consoleLog } from "@fast-china/utils";
import { useApp, useConfig } from "@/stores";

onLaunch(() => {
	const appStore = useApp();
	const configStore = useConfig();

	consoleLog("App", `成功加载【${appStore.appBaseInfo.appName}】`, new Date());

	// #ifdef APP-PLUS
	// App端需要调用setUIStyle，否则无法使用深色模式
	plus.nativeUI.setUIStyle("auto");
	// #endif

	// 设置应用名称
	appStore.setAppName(appStore.appBaseInfo.appName);
	// 处理未经过 Launcher 页面导致 axios 配置不存在的问题
	appStore.setFastAxios();
	// 处理未经过 Launcher 页面导致字典不存在的问题
	appStore.setDictionary();

	// 初始化主题
	configStore.initTheme();
});
</script>
