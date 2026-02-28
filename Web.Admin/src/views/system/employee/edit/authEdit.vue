<template>
	<FaDialog
		ref="faDialogRef"
		width="1000"
		fullHeight
		:title="state.dialogTitle"
		confirmButtonText="保存"
		@confirm-click="handleConfirm"
		@close="
			() => {
				state.formData = {
					menuIds: [],
					buttonIds: [],
				};
				state.menuList = [];
			}
		"
	>
		<div class="auth-toolbar">
			<span class="auth-toolbar__stats">
				已选 <strong>{{ state.formData.menuIds?.length ?? 0 }}</strong> 个菜单，<strong>{{ state.formData.buttonIds?.length ?? 0 }}</strong> 个权限
			</span>
			<el-button-group>
				<el-button size="small" @click="handleSelectAll">全选</el-button>
				<el-button size="small" @click="handleDeselectAll">取消全选</el-button>
			</el-button-group>
		</div>
		<el-collapse :modelValue="state.menuList.map((item) => item.value)">
			<el-collapse-item v-for="menu in state.menuList" :key="menu.value" :name="menu.value">
				<template #title>
					{{ menu.label }}
					<el-checkbox
						border
						:modelValue="isMenuChecked(menu.value)"
						:disabled="isMenuFromRole(menu.value)"
						@click.stop
						@change="handleMenuChange(menu.value, $event)"
					>
						<template #default>{{ getButtonCheckedCount(menu.value) }} / {{ menu.children.length }}</template>
					</el-checkbox>
					<el-checkbox
						class="checkbox__right"
						border
						:modelValue="isButtonAllChecked(menu.value)"
						:indeterminate="isButtonIndeterminate(menu.value)"
						:disabled="isMenuFromRole(menu.value)"
						@click.stop
						@change="handleButtonAllChange(menu.value, $event)"
					>
						<template #default>全选</template>
					</el-checkbox>
				</template>
				<div v-if="menu.children.length > 0" class="item__content__div">
					<span>操作</span>
					<el-checkbox-group v-model="state.formData.buttonIds">
						<el-checkbox
							v-for="button in menu.children"
							:key="button.value"
							border
							:value="button.value"
							:disabled="isButtonFromRole(button.value)"
						>
							{{ button.label }}
							<FaIcon v-if="button.data?.hasDesktop" name="el-icon-Monitor" />
							<FaIcon v-if="button.data?.hasWeb" name="el-icon-ChromeFilled" />
							<FaIcon v-if="button.data?.hasMobile" name="el-icon-Iphone" />
						</el-checkbox>
					</el-checkbox-group>
				</div>
			</el-collapse-item>
		</el-collapse>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { CheckboxValueType, ElMessage } from "element-plus";
import { withDefineType } from "@fast-china/utils";
import { employeeApi } from "@/api/services/Admin/employee";
import { EmployeeAuthInput } from "@/api/services/Admin/employee/models/EmployeeAuthInput";
import { roleApi } from "@/api/services/Admin/role";
import type { ElSelectorOutput, FaDialogInstance } from "fast-element-plus";

defineOptions({
	name: "SystemEmployeeAuthEdit",
});

const emit = defineEmits(["ok"]);

const faDialogRef = ref<FaDialogInstance>();

const state = reactive({
	formData: withDefineType<EmployeeAuthInput>({
		menuIds: [],
		buttonIds: [],
		roleMenuIds: [],
		roleButtonIds: [],
	}),
	dialogTitle: "职员授权",
	menuList: withDefineType<ElSelectorOutput<number>[]>([]),
});

const isMenuFromRole = (menuId: number) => {
	return state.formData.roleMenuIds?.includes(menuId) || false;
};

const isButtonFromRole = (buttonId: number) => {
	return state.formData.roleButtonIds?.includes(buttonId) || false;
};

const isMenuChecked = (menuId: number) => {
	return state.formData.menuIds.includes(menuId) || false;
};

const getButtonCheckedCount = (menuId: number) => {
	const menu = state.menuList.find((item) => item.value === menuId);
	if (!menu) {
		return 0;
	}
	const buttonIds = menu.children.map((item) => item.value);
	return buttonIds.filter((id) => state.formData.buttonIds?.includes(id)).length;
};

const getNonRoleButtonIds = (menuId: number) => {
	const menu = state.menuList.find((item) => item.value === menuId);
	if (!menu) {
		return [];
	}
	return menu.children.filter((item) => !isButtonFromRole(item.value)).map((item) => item.value);
};

const isButtonAllChecked = (menuId: number) => {
	const buttonIds = getNonRoleButtonIds(menuId);
	if (buttonIds.length === 0) {
		return false;
	}
	return buttonIds.every((id) => state.formData.buttonIds?.includes(id));
};

const isButtonIndeterminate = (menuId: number) => {
	const buttonIds = getNonRoleButtonIds(menuId);
	const checkedCount = buttonIds.filter((id) => state.formData.buttonIds?.includes(id)).length;
	return checkedCount > 0 && checkedCount < buttonIds.length;
};

const handleButtonAllChange = (menuId: number, val: CheckboxValueType) => {
	const allButtonIds = getNonRoleButtonIds(menuId);
	if (allButtonIds.length === 0) {
		return;
	}

	if (val) {
		// 全选：将该菜单下所有按钮 ID 添加到 buttonIds
		const newButtonIds = allButtonIds.filter((id) => !state.formData.buttonIds?.includes(id));
		state.formData.buttonIds = [...(state.formData.buttonIds || []), ...newButtonIds];

		// 同时选中菜单
		if (!state.formData.menuIds?.includes(menuId)) {
			state.formData.menuIds = [...(state.formData.menuIds || []), menuId];
		}
	} else {
		// 取消全选：移除该菜单下所有按钮 ID
		state.formData.buttonIds = state.formData.buttonIds?.filter((id) => !allButtonIds.includes(id)) || [];

		// 同时取消选中菜单
		state.formData.menuIds = state.formData.menuIds?.filter((id) => id !== menuId) || [];
	}
};

const handleMenuChange = (menuId: number, val: CheckboxValueType) => {
	if (val) {
		// 选中菜单
		if (!state.formData.menuIds?.includes(menuId)) {
			state.formData.menuIds = [...(state.formData.menuIds || []), menuId];
		}
	} else {
		// 取消选中菜单
		state.formData.menuIds = state.formData.menuIds?.filter((id) => id !== menuId) || [];
	}
};

const handleSelectAll = () => {
	state.formData.menuIds = state.menuList.map((item) => item.value);
	state.formData.buttonIds = state.menuList.flatMap((item) => item.children.map((child) => child.value));
};

const handleDeselectAll = () => {
	// 仅清除非角色继承的菜单和权限
	state.formData.menuIds = state.formData.roleMenuIds?.slice() || [];
	state.formData.buttonIds = state.formData.roleButtonIds?.slice() || [];
};

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await employeeApi.employeeAuth(state.formData);
		ElMessage.success("授权成功！");
		emit("ok");
	});
};

const open = (employeeId: number) => {
	faDialogRef.value.open(async () => {
		const apiRes = await employeeApi.queryEmployeeAuthMenu({ employeeId });
		state.formData = apiRes;
		state.menuList = await roleApi.queryAuthMenu();
		state.dialogTitle = `职员授权 - ${apiRes.employeeName}`;
	});
};

// 暴露给父组件的参数和方法(外部需要什么，都可以从这里暴露出去)
defineExpose({
	element: faDialogRef,
	open,
});
</script>

<style lang="scss" scoped>
.auth-toolbar {
	display: flex;
	align-items: center;
	justify-content: space-between;
	margin-bottom: 16px;
	padding: 8px 12px;
	background-color: var(--el-fill-color-light);
	border-radius: 4px;
	&__stats {
		font-size: 13px;
		color: var(--el-text-color-secondary);
	}
}
.el-collapse {
	border: none;
	.el-collapse-item {
		border: 1px solid var(--el-collapse-border-color);
		&:nth-child(n + 2) {
			margin-top: 20px;
		}
		:deep() {
			.el-collapse-item__header {
				border: none;
				padding-left: 20px;
				transition: none;
				&.is-active {
					border-bottom: 1px solid var(--el-collapse-border-color);
				}
				.el-checkbox {
					margin-left: 10px;
					transition:
						border-color 0.25s cubic-bezier(0.71, -0.46, 0.29, 1.46),
						background-color 0.25s cubic-bezier(0.71, -0.46, 0.29, 1.46),
						outline 0.25s cubic-bezier(0.71, -0.46, 0.29, 1.46);
					&:hover {
						border-color: var(--el-checkbox-checked-input-border-color);
					}
				}
				.el-collapse-item__title {
					display: flex;
					align-items: center;
					.checkbox__right {
						margin-left: auto;
						margin-right: 12px;
					}
				}
			}
			.el-collapse-item__wrap {
				border: none;
				.el-collapse-item__content {
					padding-bottom: 0;
					.item__content__div {
						display: grid;
						grid-template-columns: repeat(24, 1fr);
						&:nth-child(2) {
							border-top: 1px solid var(--el-collapse-border-color);
						}
						> span {
							border-right: 1px solid var(--el-collapse-border-color);
							grid-column: span 2;
							display: flex;
							justify-content: center;
							align-items: center;
						}
						> .el-checkbox-group {
							grid-column: span 22;
							padding: 5px 10px 5px;
							display: flex;
							flex-wrap: wrap;
							.el-checkbox {
								margin: 5px 30px 5px 0;
								&:hover {
									border-color: var(--el-checkbox-checked-input-border-color);
								}
							}
						}
					}
				}
			}
		}
	}
}
</style>
