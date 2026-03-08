<template>
	<div class="about-page">
		<el-card shadow="never">
			<template #header>
				<div class="about-header">
					<img :src="logo" alt="Logo" class="about-logo" @error="setFallBack" />
					<div class="about-title">
						<h1>{{ appStore.appName }}</h1>
						<el-text type="info">v{{ appVersion }}</el-text>
					</div>
				</div>
			</template>
			<el-descriptions :column="2" border>
				<el-descriptions-item label="应用名称">{{ appStore.appName }}</el-descriptions-item>
				<el-descriptions-item label="应用版本">{{ appVersion }}</el-descriptions-item>
				<el-descriptions-item label="当前环境">{{ environmentName }}</el-descriptions-item>
				<el-descriptions-item label="应用版次">{{ editionName }}</el-descriptions-item>
				<el-descriptions-item label="当前租户" :span="2">{{ userInfoStore.tenantName || "无" }}</el-descriptions-item>
			</el-descriptions>
		</el-card>
		<el-card shadow="never" style="margin-top: 16px">
			<template #header>
				<span>技术栈</span>
			</template>
			<el-descriptions :column="2" border>
				<el-descriptions-item v-for="(item, idx) in techStack" :key="idx" :label="item.name">
					{{ item.version }}
				</el-descriptions-item>
			</el-descriptions>
		</el-card>
		<el-card shadow="never" style="margin-top: 16px">
			<template #header>
				<span>相关链接</span>
			</template>
			<div class="about-links">
				<el-link v-for="(item, idx) in links" :key="idx" :href="item.url" target="_blank" type="primary" :underline="false">
					<el-icon><Link /></el-icon>
					{{ item.name }}
				</el-link>
			</div>
		</el-card>
	</div>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { Link } from "@element-plus/icons-vue";
import { AppEnvironmentEnum } from "@/api/enums/AppEnvironmentEnum";
import { EditionEnum } from "@/api/enums/EditionEnum";
import LogoImg from "@/assets/logo.png";
import { useApp, useUserInfo } from "@/stores";

defineOptions({
	name: "About",
});

const appStore = useApp();
const userInfoStore = useUserInfo();

const appVersion = import.meta.env.VITE_APP_VERSION;

const logo = computed(() => userInfoStore.logoUrl || appStore.logoUrl || LogoImg);

const setFallBack = (event: Event) => {
	const target = event.target as HTMLImageElement;
	target.src = LogoImg;
};

const environmentName = computed(() => {
	const map: Record<number, string> = {
		[AppEnvironmentEnum.Web]: "Web",
	};
	return map[appStore.appType] || "未知";
});

const editionName = computed(() => {
	const map: Record<number, string> = {
		[EditionEnum.None]: "无",
		[EditionEnum.Trial]: "试用版",
		[EditionEnum.Basic]: "基础版",
		[EditionEnum.Standard]: "标准版",
		[EditionEnum.Professional]: "专业版",
		[EditionEnum.Enterprise]: "企业版",
	};
	return map[appStore.edition] || "未知";
});

const techStack = [
	{ name: "Vue", version: "3.5" },
	{ name: "TypeScript", version: "5.9" },
	{ name: "Vite", version: "7.x" },
	{ name: "Element Plus", version: "2.13" },
	{ name: "Pinia", version: "3.x" },
	{ name: "Vue Router", version: "4.5" },
	{ name: "Axios", version: "1.x" },
	{ name: "ECharts", version: "6.x" },
];

const links = [
	{ name: "Fast.Admin 文档", url: "http://fastdotnet.com" },
	{ name: "Element Plus", url: "https://element-plus.org" },
	{ name: "Vue 3", url: "https://vuejs.org" },
	{ name: "Vite", url: "https://vitejs.dev" },
];
</script>

<style scoped lang="scss">
.about-page {
	padding: 20px;
	.about-header {
		display: flex;
		align-items: center;
		gap: 16px;
		.about-logo {
			width: 60px;
			height: 60px;
			border-radius: 8px;
		}
		.about-title {
			h1 {
				margin: 0 0 4px;
				font-size: 22px;
				font-weight: var(--el-font-weight-primary);
			}
		}
	}
	.about-links {
		display: flex;
		flex-wrap: wrap;
		gap: 16px;
		.el-link {
			display: flex;
			align-items: center;
			gap: 4px;
		}
	}
}
</style>
