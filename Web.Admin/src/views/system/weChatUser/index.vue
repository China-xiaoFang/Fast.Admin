<template>
	<div>
		<FastTable tableKey="1DQUW3H5ZD" rowKey="weChatId" :requestApi="weChatApi.queryWeChatUserPaged" stripe />
	</div>
</template>

<script lang="ts" setup>
import { weChatApi } from "@/api/services/weChat";

defineOptions({
	name: "SystemWeChatUser",
});
</script>
