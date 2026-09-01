<template>
	<section class="login-form-panel" :class="`login-form-panel--${props.variant}`">
		<header class="login-form-panel__header">
			<div class="login-form-panel__eyebrow">
				<span class="eyebrow-dot"></span>
				{{ variantEyebrow }}
			</div>
			<h2>{{ stepContent.title }}</h2>
			<p>{{ stepContent.description }}</p>
		</header>

		<transition mode="out-in" name="login-step">
			<div v-if="formStep !== 'SelectTenant'" :key="formStep" class="login-form-panel__step">
				<div v-if="formStep === 'NewAccount'" class="login-form-panel__back">
					<el-button type="primary" link :icon="ArrowLeftBold" @click="handleNewAccountBack">返回已保存账号</el-button>
				</div>

				<el-form
					ref="elFormRef"
					label-position="top"
					:model="formData"
					:rules="props.formRules"
					size="large"
					@keyup.enter.prevent="handleKeyupEnter"
				>
					<el-form-item v-if="formStep === 'TenantAccount' && tenantList.length > 0" prop="userKey" label="工作空间">
						<el-select
							v-model="formData.userKey"
							fit-input-width
							popper-class="login-tenant-popper"
							placeholder="选择已保存的租户"
							@change="handleTenantChange"
						>
							<el-option
								v-for="(item, index) in tenantList"
								:key="item.tenant.userKey"
								:label="item.tenant.tenantName"
								:value="item.tenant.userKey"
							>
								<div class="tenant-option">
									<img :src="item.tenant.logoUrl" :alt="item.tenant.tenantName" />
									<div class="tenant-option__content">
										<strong>{{ item.tenant.tenantName }}</strong>
										<span>{{ item.tenant.departmentName || "未设置部门" }} · {{ item.tenant.employeeName }}</span>
									</div>
									<Tag size="small" name="EditionEnum" :value="item.tenant.edition" />
									<el-button
										class="tenant-option__remove"
										text
										circle
										size="small"
										:icon="Close"
										aria-label="移除已保存账号"
										@click.stop="handleTenantRemove(index, item)"
									/>
								</div>
							</el-option>
							<template #footer>
								<el-button text type="primary" :icon="Plus" @click="handleNewAccount">绑定新的租户账号</el-button>
							</template>
						</el-select>
					</el-form-item>

					<el-form-item prop="account">
						<template #label>
							<FaFormItemTip
								v-if="formStep === 'TenantAccount'"
								label="账号"
								tips="已授权租户不能修改账号，如需使用其他账号，请绑定新的租户账号"
							/>
							<span v-else>账号</span>
						</template>
						<el-input
							v-model.trim="formData.account"
							:disabled="formStep === 'TenantAccount'"
							placeholder="请输入登录账号"
							type="text"
							autocomplete="username"
							autocapitalize="off"
							spellcheck="false"
							tabindex="1"
							:prefix-icon="User"
							@change="handleAccountChange"
						/>
					</el-form-item>

					<el-form-item prop="password" label="密码">
						<el-input
							v-model.trim="formData.password"
							placeholder="请输入登录密码"
							type="password"
							autocomplete="current-password"
							tabindex="2"
							minlength="6"
							maxlength="20"
							show-word-limit
							show-password
							:prefix-icon="Lock"
						/>
					</el-form-item>

					<div class="login-form-panel__meta">
						<el-checkbox v-model="formData.rememberMe" size="default">记住登录信息</el-checkbox>
						<span class="secure-label">
							<el-icon><CircleCheck /></el-icon>
							安全连接
						</span>
					</div>

					<FaButton ref="faButtonRef" class="login-submit" type="primary" size="large" @click="handleFormLogin">
						<span>进入工作台</span>
						<el-icon><Right /></el-icon>
					</FaButton>
				</el-form>
			</div>

			<div v-else key="SelectTenant" class="login-form-panel__step login-form-panel__step--tenant">
				<div class="login-form-panel__back">
					<el-button type="primary" link :icon="ArrowLeftBold" @click="handleTenantSelectionBack">返回账号登录</el-button>
				</div>
				<el-scrollbar class="tenant-list">
					<button v-for="item in tenantSelector" :key="item.userKey" class="tenant-card" type="button" @click="handleTenantLogin(item)">
						<img :src="item.logoUrl" :alt="item.tenantName" />
						<span class="tenant-card__body">
							<strong>{{ item.tenantName }}</strong>
							<small>{{ item.departmentName || "未设置部门" }} · {{ item.employeeName }}</small>
						</span>
						<Tag size="small" name="EditionEnum" :value="item.edition" />
						<el-icon class="tenant-card__arrow"><Right /></el-icon>
					</button>
				</el-scrollbar>
			</div>
		</transition>

		<footer class="login-form-panel__footer">
			<span></span>
			Powered by FastDotNet
		</footer>
	</section>
</template>

<script lang="ts" setup>
import { computed, useTemplateRef } from "vue";
import { ArrowLeftBold, CircleCheck, Close, Lock, Plus, Right, User } from "@element-plus/icons-vue";
import { useApp } from "@/stores";
import { useLogin } from "../useLogin";
import type { FormInstance, FormRules } from "element-plus";
import type { FaButtonInstance } from "fast-element-plus";
import type { LoginTenantOutput } from "@/api/services/Auth/login/models/LoginTenantOutput";

type LoginVariant = "classic" | "modern" | "simple" | "split";

const props = defineProps<{
	/** 登录页视觉类型。 */
	variant: LoginVariant;
	/** 登录表单校验规则。 */
	formRules?: FormRules;
}>();

const appStore = useApp();
const elFormRef = useTemplateRef<FormInstance>("elFormRef");
const faButtonRef = useTemplateRef<FaButtonInstance>("faButtonRef");

const {
	formData,
	tenantList,
	formStep,
	tenantSelector,
	currentTenant,
	handleTenantChange,
	handleTenantRemove,
	handleNewAccount,
	handleNewAccountBack,
	handleAccountChange,
	handleLogin,
	handleFormLogin,
	handleKeyupEnter,
} = useLogin(elFormRef, faButtonRef);

const variantEyebrow = computed(() => {
	switch (props.variant) {
		case "modern":
			return "SECURE DIGITAL WORKSPACE";
		case "split":
			return "UNIFIED MANAGEMENT ACCESS";
		case "simple":
			return "FOCUS · CREATE · DELIVER";
		default:
			return "ENTERPRISE MANAGEMENT";
	}
});

const stepContent = computed(() => {
	switch (formStep.value) {
		case "TenantAccount":
			return {
				title: "欢迎回来",
				description: currentTenant.value
					? `${currentTenant.value.tenantName} · ${currentTenant.value.employeeName}`
					: "选择工作空间后继续登录",
			};
		case "NewAccount":
			return { title: "绑定新账号", description: "使用新的租户账号进入工作空间" };
		case "SelectTenant":
			return { title: "选择工作空间", description: "此账号关联了多个租户，请选择本次登录入口" };
		default:
			return { title: "欢迎登录", description: `进入 ${appStore.appName}，开启高效工作` };
	}
});

const handleTenantSelectionBack = () => {
	formStep.value = tenantList.value.length > 0 ? "NewAccount" : "Account";
};

const handleTenantLogin = async (tenant: LoginTenantOutput) => {
	formData.value.userKey = tenant.userKey;
	await handleLogin();
};
</script>

<style scoped lang="scss">
.login-form-panel {
	width: 100%;
	color: var(--login-text, #162033);

	&__header {
		margin-bottom: 28px;

		h2 {
			margin: 10px 0 8px;
			font-size: clamp(28px, 3vw, 36px);
			font-weight: 720;
			line-height: 1.16;
			letter-spacing: -1.2px;
			color: var(--login-heading, #101828);
		}

		p {
			margin: 0;
			font-size: 14px;
			line-height: 1.65;
			color: var(--login-muted, #667085);
		}
	}

	&__eyebrow {
		display: flex;
		align-items: center;
		gap: 8px;
		font-size: 11px;
		font-weight: 700;
		letter-spacing: 1.8px;
		color: var(--el-color-primary);
	}

	.eyebrow-dot {
		width: 7px;
		height: 7px;
		border-radius: 50%;
		background: #22c55e;
		box-shadow: 0 0 0 5px rgb(34 197 94 / 12%);
		animation: loginStatusPulse 2.4s ease-in-out infinite;
	}

	&__step {
		min-height: 334px;
	}

	&__step--tenant {
		display: flex;
		min-height: 360px;
		flex-direction: column;
	}

	&__back {
		margin: -8px 0 14px;

		:deep(.el-button > span) {
			margin-left: 0;
		}
	}

	&__meta {
		display: flex;
		align-items: center;
		justify-content: space-between;
		margin: -2px 0 20px;

		.secure-label {
			display: inline-flex;
			align-items: center;
			gap: 5px;
			font-size: 12px;
			color: var(--login-muted, #667085);

			.el-icon {
				color: #22c55e;
			}
		}
	}

	&__footer {
		display: flex;
		align-items: center;
		justify-content: center;
		gap: 9px;
		margin-top: 18px;
		font-size: 11px;
		letter-spacing: 0.6px;
		color: var(--login-faint, #98a2b3);

		span {
			width: 18px;
			height: 1px;
			background: currentcolor;
		}
	}

	:deep(.el-form-item) {
		margin-bottom: 20px;
	}

	:deep(.el-form-item__label) {
		padding-bottom: 7px;
		font-size: 13px;
		font-weight: 600;
		color: var(--login-label, #344054);
	}

	:deep(.el-input__wrapper),
	:deep(.el-select__wrapper) {
		min-height: 48px;
		padding-inline: 15px;
		border: 1px solid var(--login-input-border, rgb(16 24 40 / 10%));
		border-radius: 13px;
		background: var(--login-input-bg, rgb(255 255 255 / 78%));
		box-shadow: 0 1px 2px rgb(16 24 40 / 3%);
		transition:
			border-color 180ms ease,
			box-shadow 180ms ease,
			background-color 180ms ease,
			transform 180ms ease;

		&:hover {
			border-color: color-mix(in srgb, var(--el-color-primary) 45%, transparent);
		}
	}

	:deep(.el-input__wrapper.is-focus),
	:deep(.el-select__wrapper.is-focused) {
		border-color: color-mix(in srgb, var(--el-color-primary) 70%, white);
		background: var(--login-input-focus-bg, #fff);
		box-shadow: 0 0 0 4px color-mix(in srgb, var(--el-color-primary) 12%, transparent);
		transform: translateY(-1px);
	}

	:deep(.el-input__inner) {
		color: var(--login-heading, #101828);

		&::placeholder {
			color: var(--login-faint, #98a2b3);
		}
	}

	:deep(.el-input__prefix),
	:deep(.el-select__prefix) {
		color: var(--el-color-primary);
	}

	:deep(.el-checkbox__label) {
		font-size: 13px;
		color: var(--login-muted, #667085);
	}

	.login-submit {
		width: 100%;
		height: 50px;
		border: 0;
		border-radius: 13px;
		font-size: 15px;
		font-weight: 650;
		letter-spacing: 0.8px;
		background: linear-gradient(110deg, var(--el-color-primary), color-mix(in srgb, var(--el-color-primary) 68%, #7259ff));
		box-shadow: 0 12px 24px color-mix(in srgb, var(--el-color-primary) 25%, transparent);
		transition:
			transform 180ms ease,
			box-shadow 180ms ease,
			filter 180ms ease;

		:deep(> span) {
			display: flex;
			align-items: center;
			justify-content: center;
			gap: 10px;
		}

		&:hover {
			filter: saturate(1.08) brightness(1.04);
			box-shadow: 0 16px 30px color-mix(in srgb, var(--el-color-primary) 32%, transparent);
			transform: translateY(-2px);
		}

		&:active {
			transform: translateY(0);
		}
	}
}

.tenant-list {
	flex: 1;
	height: 300px;
	margin-right: -8px;
	padding-right: 8px;
}

.tenant-card {
	display: grid;
	width: 100%;
	grid-template-columns: 42px minmax(0, 1fr) auto 20px;
	align-items: center;
	gap: 12px;
	margin-bottom: 10px;
	padding: 12px;
	color: var(--login-text, #162033);
	text-align: left;
	cursor: pointer;
	border: 1px solid var(--login-input-border, rgb(16 24 40 / 10%));
	border-radius: 14px;
	background: var(--login-input-bg, rgb(255 255 255 / 78%));
	transition:
		border-color 180ms ease,
		background-color 180ms ease,
		transform 180ms ease,
		box-shadow 180ms ease;

	> img {
		width: 42px;
		height: 42px;
		object-fit: cover;
		border-radius: 12px;
		box-shadow: 0 5px 12px rgb(16 24 40 / 10%);
	}

	&__body {
		display: flex;
		min-width: 0;
		flex-direction: column;
		gap: 4px;

		strong,
		small {
			overflow: hidden;
			text-overflow: ellipsis;
			white-space: nowrap;
		}

		strong {
			font-size: 14px;
		}

		small {
			color: var(--login-muted, #667085);
		}
	}

	&__arrow {
		color: var(--login-faint, #98a2b3);
		transition: transform 180ms ease;
	}

	&:hover,
	&:focus-visible {
		outline: none;
		border-color: color-mix(in srgb, var(--el-color-primary) 50%, transparent);
		background: var(--login-input-focus-bg, #fff);
		box-shadow: 0 10px 24px color-mix(in srgb, var(--el-color-primary) 10%, transparent);
		transform: translateY(-2px);

		.tenant-card__arrow {
			color: var(--el-color-primary);
			transform: translateX(3px);
		}
	}
}

:global(.login-tenant-popper) {
	border: 1px solid var(--el-border-color-lighter);
	border-radius: 14px;
	box-shadow: 0 18px 50px rgb(15 23 42 / 18%);
}

:global(.login-tenant-popper .el-select-dropdown__item) {
	height: auto;
	padding: 8px 12px;
	line-height: 1.4;
}

:global(.login-tenant-popper .el-select-dropdown__footer) {
	padding: 6px;
}

:global(.login-tenant-popper .el-select-dropdown__footer .el-button) {
	width: 100%;
}

:global(.login-tenant-popper .tenant-option) {
	display: grid;
	width: 100%;
	grid-template-columns: 36px minmax(0, 1fr) auto 32px;
	align-items: center;
	gap: 10px;
}

:global(.login-tenant-popper .tenant-option > img) {
	width: 36px;
	height: 36px;
	object-fit: cover;
	border-radius: 10px;
}

:global(.login-tenant-popper .tenant-option__content) {
	display: flex;
	min-width: 0;
	flex-direction: column;
	gap: 2px;
}

:global(.login-tenant-popper .tenant-option__content strong),
:global(.login-tenant-popper .tenant-option__content span) {
	overflow: hidden;
	text-overflow: ellipsis;
	white-space: nowrap;
}

:global(.login-tenant-popper .tenant-option__content span) {
	font-size: 12px;
	color: var(--el-text-color-secondary);
}

:global(.login-tenant-popper .tenant-option__remove) {
	width: 28px;
	height: 28px;
	padding: 0;
	color: var(--el-text-color-placeholder);
	transition:
		color 180ms ease,
		background-color 180ms ease,
		transform 180ms ease;
}

:global(.login-tenant-popper .tenant-option__remove:hover) {
	color: var(--el-color-danger);
	background: var(--el-color-danger-light-9);
	transform: rotate(90deg) scale(1.06);
}

:global(.login-tenant-popper .tenant-option__remove:active) {
	transform: rotate(90deg) scale(0.9);
}

.login-step-enter-active,
.login-step-leave-active {
	transition:
		opacity 180ms ease,
		transform 240ms cubic-bezier(0.22, 1, 0.36, 1),
		filter 180ms ease;
}

.login-step-enter-from {
	opacity: 0;
	filter: blur(4px);
	transform: translateX(18px);
}

.login-step-leave-to {
	opacity: 0;
	filter: blur(4px);
	transform: translateX(-14px);
}

@keyframes loginStatusPulse {
	0%,
	100% {
		box-shadow: 0 0 0 4px rgb(34 197 94 / 10%);
	}

	50% {
		box-shadow: 0 0 0 7px rgb(34 197 94 / 4%);
	}
}

@media (max-width: 520px) {
	.login-form-panel {
		&__header {
			margin-bottom: 18px;

			h2 {
				margin-block: 8px 6px;
				font-size: 27px;
			}
		}

		&__step {
			min-height: 300px;
		}

		&__footer {
			margin-top: 14px;
		}

		&__meta {
			margin-bottom: 16px;

			.secure-label {
				display: none;
			}
		}

		:deep(.el-form-item) {
			margin-bottom: 16px;
		}
	}
}

@media (prefers-reduced-motion: reduce) {
	.login-form-panel *,
	.login-form-panel *::before,
	.login-form-panel *::after,
	:global(.login-tenant-popper .tenant-option__remove) {
		scroll-behavior: auto !important;
		animation-duration: 0.01ms !important;
		animation-iteration-count: 1 !important;
		transition-duration: 0.01ms !important;
	}
}
</style>
