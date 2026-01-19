<template>
	<FaSelect
		v-bind="$attrs"
		:initParam="props.merchantType"
		:requestApi="merchantApi.merchantSelector"
		v-model="modelValue"
		v-model:label="merchantNo"
		placeholder="请选择商户号"
		clearable
		moreDetail
		@change="(value) => emit('change', value)"
	>
		<template #default="data">
			<span>{{ data.label }}</span>
			<Tag name="PaymentChannelEnum" :value="data.data?.merchantType" size="small" />
		</template>
	</FaSelect>
</template>

<script lang="ts" setup>
import { useVModel } from "@vueuse/core";
import type { ElSelectorOutput } from "fast-element-plus";
import { PaymentChannelEnum } from "@/api/enums/PaymentChannelEnum";
import { merchantApi } from "@/api/services/merchant";

defineOptions({
	name: "MerchantSelect",
});

const props = withDefaults(
	defineProps<{
		modelValue?: number | string;
		merchantNo?: string;
		merchantType?: PaymentChannelEnum;
	}>(),
	{}
);

const emit = defineEmits({
	"update:modelValue": (value: number | string) => true,
	"update:merchantNo": (value: string) => true,
	change: (value: ElSelectorOutput<number | string>) => true,
});

const modelValue = useVModel(props, "modelValue", emit, { passive: false });
const merchantNo = useVModel(props, "merchantNo", emit, { passive: true });
</script>
