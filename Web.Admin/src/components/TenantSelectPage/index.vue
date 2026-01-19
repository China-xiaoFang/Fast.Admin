<template>
	<FaSelectPage
		v-bind="$attrs"
		:requestApi="tenantApi.tenantSelector"
		v-model="modelValue"
		v-model:label="tenantName"
		placeholder="请选择租户"
		clearable
		moreDetail
		@change="handleChange"
	>
		<template #default="data">
			<div style="display: flex; justify-content: space-between; align-items: center; gap: 8px; width: 100%">
				<FaAvatar v-if="data.data?.logoUrl" :src="data.data.logoUrl" thumb size="small" />
				<div style="flex: 1">
					<span>{{ data.label }}</span>
					<span style="display: flex; justify-content: space-between; width: 100%">
						<span style="font-size: var(--el-font-size-extra-small); padding-right: 8px">{{ data.data?.tenantNo }}</span>
						<Tag name="EditionEnum" :value="data.data.edition" size="small" />
					</span>
				</div>
			</div>
		</template>
	</FaSelectPage>
</template>

<script lang="ts" setup>
import { useVModel } from "@vueuse/core";
import Tag from "../Tag/index.vue";
import type { ElSelectorOutput } from "fast-element-plus";
import { tenantApi } from "@/api/services/tenant";

defineOptions({
	name: "TenantSelectPage",
});

const props = withDefaults(
	defineProps<{
		modelValue?: number | string;
		tenantName?: string;
		tenantNo?: string;
		tenantCode?: string;
	}>(),
	{}
);

const emit = defineEmits({
	"update:modelValue": (value: number | string) => true,
	"update:tenantName": (value: string) => true,
	"update:tenantNo": (value: string) => true,
	"update:tenantCode": (value: string) => true,
	change: (value: ElSelectorOutput<number | string>) => true,
});

const modelValue = useVModel(props, "modelValue", emit, { passive: false });
const tenantName = useVModel(props, "tenantName", emit, { passive: true });
const tenantNo = useVModel(props, "tenantNo", emit, { passive: true });
const tenantCode = useVModel(props, "tenantCode", emit, { passive: true });

const handleChange = (value: ElSelectorOutput<number | string>) => {
	if (value) {
		tenantNo.value = value.data.tenantNo;
		tenantCode.value = value.data.tenantCode;
		emit("change", value);
	} else {
		tenantNo.value = undefined;
		tenantCode.value = undefined;
		emit("change", undefined);
	}
};
</script>
