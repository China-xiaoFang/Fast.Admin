<template>
	<div class="captcha-verify">
		<el-input
			v-model="modelValue"
			:placeholder="placeholder"
			maxlength="4"
			clearable
			@update:modelValue="(val) => emit('update:modelValue', val)"
		>
			<template #append>
				<div class="captcha-verify__image" @click="handleRefresh" title="点击刷新验证码">
					<img v-if="state.captchaUrl" :src="state.captchaUrl" alt="验证码" />
					<el-icon v-else :size="20"><Refresh /></el-icon>
				</div>
			</template>
		</el-input>
	</div>
</template>

<script setup lang="ts">
import { onMounted, reactive } from "vue";
import { Refresh } from "@element-plus/icons-vue";

defineOptions({
	name: "CaptchaVerify",
});

const props = defineProps({
	/** 验证码值 */
	modelValue: {
		type: String,
		default: "",
	},
	/** 占位符 */
	placeholder: {
		type: String,
		default: "请输入验证码",
	},
	/** 验证码图片接口地址 */
	captchaApi: {
		type: String,
		default: "/api/captcha",
	},
});

const emit = defineEmits<{
	(e: "update:modelValue", value: string): void;
	(e: "refresh", captchaKey: string): void;
}>();

const state = reactive({
	captchaUrl: "",
	captchaKey: "",
});

/** 生成本地SVG验证码 */
const generateLocalCaptcha = () => {
	const chars = "0123456789";
	let code = "";
	for (let i = 0; i < 4; i++) {
		code += chars[Math.floor(Math.random() * chars.length)];
	}
	state.captchaKey = code;

	// 生成 SVG 验证码图片
	const width = 120;
	const height = 40;
	let svg = `<svg xmlns="http://www.w3.org/2000/svg" width="${width}" height="${height}">`;
	svg += `<rect width="${width}" height="${height}" fill="#f0f0f0"/>`;

	// 干扰线
	for (let i = 0; i < 4; i++) {
		const x1 = Math.random() * width;
		const y1 = Math.random() * height;
		const x2 = Math.random() * width;
		const y2 = Math.random() * height;
		const color = `rgb(${Math.random() * 200},${Math.random() * 200},${Math.random() * 200})`;
		svg += `<line x1="${x1}" y1="${y1}" x2="${x2}" y2="${y2}" stroke="${color}" stroke-width="1"/>`;
	}

	// 干扰点
	for (let i = 0; i < 20; i++) {
		const cx = Math.random() * width;
		const cy = Math.random() * height;
		const color = `rgb(${Math.random() * 200},${Math.random() * 200},${Math.random() * 200})`;
		svg += `<circle cx="${cx}" cy="${cy}" r="1" fill="${color}"/>`;
	}

	// 文字
	for (let i = 0; i < code.length; i++) {
		const x = 15 + i * 25;
		const y = 28 + (Math.random() * 8 - 4);
		const rotate = Math.random() * 30 - 15;
		const color = `rgb(${Math.floor(Math.random() * 100)},${Math.floor(Math.random() * 100)},${Math.floor(Math.random() * 100)})`;
		svg += `<text x="${x}" y="${y}" fill="${color}" font-size="24" font-weight="bold" transform="rotate(${rotate},${x},${y})">${code[i]}</text>`;
	}

	svg += "</svg>";
	state.captchaUrl = `data:image/svg+xml;base64,${btoa(svg)}`;
	emit("refresh", state.captchaKey);
};

const handleRefresh = () => {
	generateLocalCaptcha();
};

/** 获取当前验证码key */
const getCaptchaKey = () => state.captchaKey;

onMounted(() => {
	generateLocalCaptcha();
});

defineExpose({ refresh: handleRefresh, getCaptchaKey });
</script>

<style scoped lang="scss">
.captcha-verify {
	width: 100%;
	:deep(.el-input-group__append) {
		padding: 0;
	}
	.captcha-verify__image {
		width: 120px;
		height: 100%;
		display: flex;
		align-items: center;
		justify-content: center;
		cursor: pointer;
		img {
			width: 100%;
			height: 100%;
			object-fit: contain;
		}
		.el-icon {
			color: var(--el-text-color-secondary);
		}
		&:hover {
			opacity: 0.8;
		}
	}
}
</style>
