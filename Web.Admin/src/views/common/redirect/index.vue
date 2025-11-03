<template>
	<div />
</template>

<script lang="ts" setup>
import { useRoute, useRouter } from "vue-router";

defineOptions({
	name: "Redirect",
});

const route = useRoute();
const router = useRouter();

router.replace({ path: `/${route.params.path}`, query: route.query });
</script>
