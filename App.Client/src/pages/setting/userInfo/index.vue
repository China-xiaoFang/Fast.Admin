<template>
	<view class="page">
		<wd-text
			customClass="pl20 mb10"
			customStyle="display: block;"
			:text="`用户信息（${state.userInfo.updatedTime || state.userInfo.createdTime}）`"
			size="var(--wot-font-size-small)"
		/>

		<wd-cell-group border>
			<!-- #ifdef MP-WEIXIN -->
			<wd-cell title="头像" clickable isLink center>
				<button class="btn_avatar" :plain="true" openType="chooseAvatar" @chooseavatar="handleAvatar">
					<image class="avatar" :src="state.userInfo.avatar" />
				</button>
			</wd-cell>
			<!-- #endif -->

			<!-- #ifndef MP-WEIXIN -->
			<wd-cell title="头像" clickable isLink center @click="handleAvatar">
				<image class="avatar" :src="state.userInfo.avatar" />
			</wd-cell>
			<!-- #endif -->

			<wd-cell title="昵称" :value="state.userInfo.nickName" clickable isLink @click="handleNickName" />

			<!-- #ifdef MP-WEIXIN -->
			<wd-cell v-if="state.userInfo.allowModifyMobile" title="手机" clickable isLink center>
				<button v-if="appStore.isClient" class="btn_phone" size="mini" :plain="true" openType="getPhoneNumber" @getphonenumber="handlePhone">
					{{ state.userInfo.mobile || "暂未授权手机号" }}
				</button>
				<button v-else class="btn_phone" size="mini" :plain="true" openType="getRealtimePhoneNumber" @getrealtimephonenumber="handlePhone">
					{{ state.userInfo.mobile || "暂未授权手机号" }}
				</button>
			</wd-cell>
			<wd-cell
				v-else
				title="手机"
				:value="state.userInfo.mobile || '暂未授权手机号'"
				clickable
				isLink
				@click="useToast.warning('24 小时内不可再次修改手机号')"
			/>
			<!-- #endif -->

			<!-- #ifndef MP-WEIXIN -->
			<wd-cell title="手机" :value="state.userInfo.mobile || '暂未授权手机号'" clickable isLink @click="handlePhone" />
			<!-- #endif -->

			<wd-picker label="性别" alignRight :columns="genderEnum" v-model="state.userInfo.sex" @confirm="handleSex" />
		</wd-cell-group>
	</view>
</template>

<script setup lang="ts">
import { onLoad } from "@dcloudio/uni-app";
import { reactive } from "vue";
import { clickUtil, consoleLog, withDefineType } from "@fast-china/utils";
import { weChatApi } from "@/api/services/Center/weChat";
import { clientUserApi } from "@/api/services/Center/clientUser";
import { fileApi } from "@/api/services/File";
import { RegExps } from "@/constants";
import { useMessageBox, useToast } from "@/hooks";
import { useApp, useUserInfo } from "@/stores";
import type { QueryClientUserDetailOutput } from "@/api/services/Center/clientUser/models/QueryClientUserDetailOutput";
import type { MessageOptions } from "wot-design-uni/components/wd-message-box/types";

definePage({
	name: "SettingUserInfo",
	layout: "layout",
	style: {
		navigationBarTitleText: "个人资料",
	},
});

const appStore = useApp();
const userInfoStore = useUserInfo();

const genderEnum = appStore.getDictionary("GenderEnum");

const state = reactive({
	/** 用户信息 */
	userInfo: withDefineType<QueryClientUserDetailOutput>({}),
});

/** 页面刷新 */
const loadRefresh = async () => {
	state.userInfo = await clientUserApi.queryClientUserDetail();
};

onLoad(async () => {
	await loadRefresh();
});

/** 头像上传 */
const handleAvatarUpload = async (url: string) => {
	const avatar = await fileApi.uploadAvatar(url);
	if (avatar) {
		await clientUserApi.editClientUser({
			...state.userInfo,
			mobile: undefined,
			avatar,
		});
		userInfoStore.avatar = avatar;
		useToast.success("修改成功！");
		await loadRefresh();
	}
};

/** 处理头像 */
const handleAvatar = async (event: UniHelper.ButtonOnChooseavatarEvent) => {
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

/** 处理昵称 */
const handleNickName = () => {
	const options: MessageOptions = {
		title: "请输入昵称",
		inputPlaceholder: "请输入昵称",
		inputValue: state.userInfo.nickName,
		inputPattern: /^(?!\s*$).+/,
		inputError: "昵称不能为空",
		confirmButtonText: "保存",
	};

	// #ifdef MP-WEIXIN
	options.inputType = "nickname";
	//#endif

	// #ifndef MP-WEIXIN
	options.inputType = "text";
	//#endif

	useMessageBox.prompt(options).then(async (res) => {
		const nickName = res.value as string;
		if (nickName !== state.userInfo.nickName) {
			await clientUserApi.editClientUser({
				...state.userInfo,
				mobile: undefined,
				nickName,
			});
			userInfoStore.nickName = nickName;
			useToast.success("修改成功！");
			await loadRefresh();
		}
	});
};

/** 处理手机号 */
const handlePhone = async (event: UniHelper.ButtonOnGetrealtimephonenumberEvent | UniHelper.ButtonOnGetphonenumberEvent) => {
	// #ifdef MP-WEIXIN
	consoleLog("UserInfo", "PhoneNumber", event.detail);
	const { code } = event.detail;
	if (!code) return;
	// 解析微信手机号
	const apiRes = await weChatApi.weChatCode2PhoneNumber({
		code,
	});
	await clientUserApi.editClientUser({
		...state.userInfo,
		mobile: apiRes.purePhoneNumber,
	});
	userInfoStore.mobile = apiRes.purePhoneNumber;
	useToast.success("修改成功！");
	await loadRefresh();
	//#endif

	// #ifndef MP-WEIXIN
	useMessageBox
		.prompt({
			title: "请输入手机号",
			inputType: "number",
			inputPlaceholder: "请输入手机号",
			inputValue: state.userInfo.mobile,
			inputPattern: RegExps.Mobile,
			inputError: "请输入正确的手机号",
			confirmButtonText: "保存",
		})
		.then(async (res) => {
			const mobile = res.value as string;
			if (res.value !== state.userInfo.mobile) {
				await clientUserApi.editClientUser({
					...state.userInfo,
					mobile,
				});
				userInfoStore.mobile = mobile;
				useToast.success("修改成功！");
				await loadRefresh();
			}
		});
	//#endif
};

/** 处理性别 */
const handleSex = async (item: ElSelectorOutput) => {
	await clientUserApi.editClientUser({
		...state.userInfo,
		mobile: undefined,
		sex: item.value,
	});
	useToast.success("修改成功！");
	await loadRefresh();
};
</script>

<style scoped lang="scss">
@import "./index.scss";
</style>
