<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="DEV_WECHAT_USER" rowKey="weChatId" :requestApi="weChatApi.queryWeChatUserPaged"> </FastTable>
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import type { FastTableInstance } from "@/components";
import { weChatApi } from "@/api/services/weChat";

defineOptions({
	name: "DevWeChatUser",
});

const fastTableRef = ref<FastTableInstance>();
</script>
