<template>
	<div class="fa-error-page">
		<Page403 />
		<router-link to="/">
			<el-button type="primary">回到首页</el-button>
		</router-link>
	</div>
</template>

<script lang="ts" setup>
import { Page403 } from "@/icons";

defineOptions({
	name: "403",
});
</script>
