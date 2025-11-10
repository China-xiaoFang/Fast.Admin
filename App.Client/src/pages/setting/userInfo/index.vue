<template>
	<view class="page">
		<wd-cell-group border>
			<!-- #ifdef MP-WEIXIN -->
			<wd-cell title="头像" clickable isLink center>
				<button class="btn_avatar" openType="chooseAvatar" @chooseavatar="changeAvatar">
					<image class="avatar" :src="userInfo.avatarUrl ?? defaultAvatar" />
				</button>
			</wd-cell>
			<!-- #endif -->

			<!-- #ifndef MP-WEIXIN -->
			<wd-cell title="头像" clickable isLink center @click="changeAvatar">
				<image class="avatar" :src="userInfo.avatarUrl ?? defaultAvatar" />
			</wd-cell>
			<!-- #endif -->

			<wd-cell title="名称" :value="userInfo.clerkName" clickable isLink @click="changeClerkName" />
			<wd-cell title="手机号" :value="userInfo.mobile" clickable isLink @click="changeMobile" />
		</wd-cell-group>
	</view>
</template>

<script setup lang="ts">
import { onLoad } from "@dcloudio/uni-app";
import { reactive } from "vue";
import { clickUtil } from "@fast-china/utils";
import { fileApi } from "@/api/services/file";
import { useMessageBox, useToast } from "@/hooks";
import defaultAvatar from "@/static/images/avatar.jpg";
import { useUserInfo } from "@/stores";

definePage({
	name: "SettingUserInfo",
	layout: "layout",
	style: {
		navigationBarTitleText: "个人资料",
	},
});

const userInfoStore = useUserInfo();

const userInfo = reactive({});

const handleAvatarUpload = async (url: string) => {
	// const avatarUrl = await fileApi.uploadAvatar(url);
	// if (avatarUrl) {
	// 	await clerkApi.updateClerk({
	// 		...userInfo,
	// 		avatarUrl,
	// 	});
	// 	userInfo.avatarUrl = avatarUrl;
	// 	useToast.success("修改成功！");
	// }
};

const changeAvatar = async (event: UniHelper.ButtonOnChooseavatarEvent) => {
	await clickUtil.throttleAsync(async () => {
		// #ifdef MP-WEIXIN
		const { avatarUrl } = event.detail;
		if (!avatarUrl) {
			useToast.error("头像获取失败");
			return;
		}
		await handleAvatarUpload(avatarUrl);
		//#endif
		// #ifndef MP-WEIXIN
		uni.chooseImage({
			count: 1,
			sizeType: ["compressed"],
			success: async (res) => {
				await handleAvatarUpload(res.tempFilePaths[0]);
			},
		});
		//#endif
	});
};

const changeClerkName = () => {
	// useMessageBox
	// 	.prompt({
	// 		title: "请输入昵称",
	// 		inputType: "nickname",
	// 		inputPlaceholder: "请输入昵称",
	// 		inputValue: userInfo.clerkName,
	// 		inputPattern: /^(?!\s*$).+/,
	// 		inputError: "昵称不能为空",
	// 		confirmButtonText: "保存",
	// 	})
	// 	.then(async (res) => {
	// 		if (res.value !== userInfo.clerkName) {
	// 			const clerkName = res.value as string;
	// 			await clerkApi.updateClerk({
	// 				...userInfo,
	// 				clerkName,
	// 			});
	// 			userInfo.clerkName = clerkName;
	// 			useToast.success("修改成功！");
	// 		}
	// 	});
};

const changeMobile = () => {
	// useMessageBox
	// 	.prompt({
	// 		title: "请输入手机号",
	// 		inputPlaceholder: "请输入手机号",
	// 		inputValue: userInfo.mobile,
	// 		inputPattern: RegExps.Mobile,
	// 		inputError: "请输入正确的手机号",
	// 		confirmButtonText: "保存",
	// 	})
	// 	.then(async (res) => {
	// 		if (res.value !== userInfo.mobile) {
	// 			const mobile = res.value as string;
	// 			await clerkApi.updateClerk({
	// 				...userInfo,
	// 				mobile,
	// 			});
	// 			userInfo.mobile = mobile;
	// 			useToast.success("修改成功！");
	// 		}
	// 	});
};

onLoad(async () => {
	// const apiRes = await clerkApi.queryClerkDetail(userInfoStore.userInfo.clerkID);
	// Object.keys(apiRes).forEach((key) => {
	// 	userInfo[key] = apiRes[key];
	// });
});
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
