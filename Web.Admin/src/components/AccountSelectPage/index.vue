<template>
	<FaSelectPage
		v-bind="$attrs"
		:requestApi="accountApi.accountSelector"
		v-model="modelValue"
		v-model:label="mobile"
		placeholder="请选择账号"
		clearable
		moreDetail
		@change="handleChange"
	>
		<template #default="data">
			<div style="display: flex; justify-content: space-between; align-items: center; gap: 8px; width: 100%">
				<FaAvatar :src="data.data.avatar" thumb size="small" />
				<div style="flex: 1">
					<span>{{ data.label }}</span>
					<span style="display: flex; justify-content: space-between; width: 100%">
						<span style="font-size: var(--el-font-size-extra-small); padding-right: 8px">{{ data.data?.nickName }}</span>
						<span style="font-size: var(--el-font-size-extra-small)">{{ data.data?.email }}</span>
					</span>
				</div>
			</div>
		</template>
	</FaSelectPage>
</template>

<script lang="ts" setup>
import { useVModel } from "@vueuse/core";
import type { ElSelectorOutput } from "fast-element-plus";
import { accountApi } from "@/api/services/account";

defineOptions({
	name: "AccountSelectPage",
});

const props = withDefaults(
	defineProps<{
		modelValue?: number | string;
		mobile?: string;
		email?: string;
		accountKey?: string;
	}>(),
	{}
);

const emit = defineEmits({
	"update:modelValue": (value: number | string) => true,
	"update:mobile": (value: string) => true,
	"update:email": (value: string) => true,
	"update:accountKey": (value: string) => true,
	change: (value: ElSelectorOutput<number | string>) => true,
});

const modelValue = useVModel(props, "modelValue", emit, { passive: true });
const mobile = useVModel(props, "mobile", emit, { passive: true });
const email = useVModel(props, "email", emit, { passive: true });
const accountKey = useVModel(props, "accountKey", emit, { passive: true });

const handleChange = (value: ElSelectorOutput<number | string>) => {
	if (value) {
		email.value = value.data.email;
		accountKey.value = value.data.accountKey;
		emit("change", value);
	} else {
		email.value = undefined;
		accountKey.value = undefined;
		emit("change", undefined);
	}
};
</script>
