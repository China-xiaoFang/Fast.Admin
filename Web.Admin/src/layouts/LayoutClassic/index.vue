<template>
	<el-container
		:class="[
			'layout',
			{
				contentFull: navTabsStore.state.contentFull,
				contentLarge: navTabsStore.state.contentLarge,
			},
		]"
	>
		<el-aside :style="{ '--el-aside-width': configStore.layout.menuWidth }">
			<LayoutLogo />
			<LayoutMenu />
		</el-aside>
		<el-container>
			<el-header>
				<div class="nav-bar" :style="{ '--height': configStore.layout.navBarHeight }">
					<LayoutBreadcrumb />
					<div class="right">
						<LayoutScreenFull />
						<div class="setting g__hover__twinkle" title="高级配置" @click="layoutConfigRef.open()">
							<el-icon>
								<Setting />
							</el-icon>
						</div>
						<el-dropdown class="avatar" placement="bottom" trigger="click" hideOnClick>
							<div class="user-info">
								<FaAvatar original :size="25" :src="userInfoStore.userInfo.avatar" :icon="UserFilled" />
								<span class="clerk-name">{{ userInfoStore.userInfo.nickName }}</span>
							</div>
							<template #dropdown>
								<el-dropdown-menu>
									<el-dropdown-item @click="changePasswordRef.open()">修改密码</el-dropdown-item>
									<el-dropdown-item @click="handleRefreshSystem">刷新系统</el-dropdown-item>
									<el-dropdown-item divided @click="handleLogout">退出系统</el-dropdown-item>
								</el-dropdown-menu>
							</template>
						</el-dropdown>
					</div>
				</div>
				<LayoutNavBarTab />
			</el-header>
			<el-main :style="{ '--el-main-padding': configStore.layout.mainPadding }">
				<el-scrollbar>
					<router-view v-slot="{ Component, route }">
						<transition mode="out-in" :name="configStore.layout.mainAnimation">
							<keep-alive :include="navTabsStore.state.keepAliveComponentNameList">
								<component :is="Component" :key="route.name" class="layout-main" />
							</keep-alive>
						</transition>
					</router-view>
				</el-scrollbar>
			</el-main>
			<el-footer :style="{ '--el-footer-height': configStore.layout.footer ? configStore.layout.footerHeight : 0 }">
				<Footer />
			</el-footer>
		</el-container>
	</el-container>
</template>

<script setup lang="ts">
import { inject } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Setting, UserFilled } from "@element-plus/icons-vue";
import { Local } from "@fast-china/utils";
import { changePasswordKey, layoutConfigKey } from "@/layouts";
import LayoutBreadcrumb from "@/layouts/components/Breadcrumb/index.vue";
import LayoutLogo from "@/layouts/components/Logo/index.vue";
import LayoutMenu from "@/layouts/components/Menu/index.vue";
import LayoutNavBarTab from "@/layouts/components/NavBarTab/index.vue";
import LayoutScreenFull from "@/layouts/components/ScreenFull/index.vue";
import { refreshApp } from "@/main";
import { useConfig, useNavTabs, useUserInfo } from "@/stores";

defineOptions({
	name: "LayoutClassic",
});

const configStore = useConfig();
const navTabsStore = useNavTabs();
const userInfoStore = useUserInfo();

const layoutConfigRef = inject(layoutConfigKey);
const changePasswordRef = inject(changePasswordKey);

const handleRefreshSystem = () => {
	ElMessageBox.confirm("此操作会强制刷新当前页面，是否继续操作？", {
		type: "warning",
	}).then(() => {
		Local.removeByPrefix("HTTP_CACHE_");
		refreshApp();
	});
};

const handleLogout = async () => {
	ElMessageBox.confirm(`确定要退出登录？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await userInfoStore.logout();
			ElMessage.success(`退出登录成功`);
		},
	});
};
</script>

<style scoped lang="scss">
@use "./index.scss";
</style>
