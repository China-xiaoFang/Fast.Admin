<template>
	<iframe :src="decodeURIComponent(route.query.url as string)" frameborder="0" style="width: 100%; height: 100%" />
</template>

<script lang="ts" setup>
import { useRoute } from "vue-router";

defineOptions({
	// eslint-disable-next-line vue/no-reserved-component-names
	name: "Iframe",
});

const route = useRoute();
</script>
