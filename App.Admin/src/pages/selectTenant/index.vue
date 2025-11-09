<template>
	<view class="page">
		<wd-divider contentPosition="left">
			<text>{{ userInfoStore.nickName }}</text>
			请选择您要登录的租户账号
		</wd-divider>
		<view class="card">
			<wd-card v-for="item in tenantList" :key="item.userKey" @tap="handleTenantLogin(item.userKey)">
				<image v-if="item.logoUrl" class="logo_img" :src="item.logoUrl" mode="aspectFill" />
				<FaIcon v-else name="tenant" />
				<view class="tenant__warp">
					<view class="tenant__top">
						<text class="top__name fa-text__overflow1">{{ item.tenantName }}</text>
						<FaTag name="EditionEnum" :value="item.edition" />
					</view>
					<view class="tenant__center">
						<text class="center__name fa-text__overflow1">{{ item.deptName || "无部门..." }}</text>
						<text>{{ item.employeeNo || "无工号..." }}</text>
					</view>
					<view class="tenant__bottom">
						<FaTag name="UserTypeEnum" :value="item.userType" />
						<view class="bottom__name">
							<text>{{ item.employeeName }}</text>
							<image v-if="item.idPhoto" class="idPhoto_img" :src="item.idPhoto" mode="aspectFill" />
						</view>
					</view>
				</view>
			</wd-card>
		</view>
	</view>
</template>

<script setup lang="ts">
import { onLoad, onPullDownRefresh } from "@dcloudio/uni-app";
import { ref } from "vue";
import { clickUtil } from "@fast-china/utils";
import { useRouter } from "uni-mini-router";
import type { LoginTenantOutput } from "@/api/services/login/models/LoginTenantOutput";
import { LoginStatusEnum } from "@/api/enums/LoginStatusEnum";
import { loginApi } from "@/api/services/login";
import { CommonRoute } from "@/common";
import { useLoading, useMessageBox } from "@/hooks";
import { useUserInfo } from "@/stores";

definePage({
	name: "SelectTenant",
	layout: "layout",
	pageScroll: false,
	authForbidView: true,
	noLogin: true,
	style: {
		navigationBarTitleText: "选择租户",
		enablePullDownRefresh: true,
	},
});

const userInfoStore = useUserInfo();
const router = useRouter();

const tenantList = ref<LoginTenantOutput[]>([]);

/** 处理租户登录 */
const handleTenantLogin = async (userKey: string) => {
	await clickUtil.throttleAsync(async () => {
		const { accountKey } = userInfoStore;
		const loginRes = await loginApi.tenantLogin({
			accountKey,
			userKey,
		});
		if (loginRes.status === LoginStatusEnum.Success) {
			userInfoStore.login(loginRes);
		} else {
			useMessageBox.alert(loginRes.message);
		}
	});
};

/** 获取租户 */
const queryTenant = async () => {
	const { accountKey } = userInfoStore;
	if (!accountKey) {
		useMessageBox
			.alert({
				msg: "登录信息有误，无法获取您的信息。请重新授权以继续使用我们的服务。",
				confirmButtonText: "重新登录",
			})
			.then(() => {
				router.replace({
					path: CommonRoute.Login,
				});
			});
	} else {
		useLoading.show("获取租户信息中...");
		tenantList.value = await loginApi.queryLoginUserByAccount(accountKey).finally(() => useLoading.hide());
		if (tenantList.value.length === 0) {
			useMessageBox
				.alert({
					msg: "该账号暂无关联的租户信息，请切换账号后重新登录。",
					confirmButtonText: "重新登录",
				})
				.then(() => {
					router.replace({
						path: CommonRoute.Login,
					});
				});
		}
	}
};

onLoad(() => {
	if (userInfoStore.nickName) {
		uni.setNavigationBarTitle({
			title: `选择租户 - ${userInfoStore.nickName}`,
		});
	}
	queryTenant();
});

onPullDownRefresh(async () => {
	await queryTenant();
	uni.stopPullDownRefresh();
});
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
