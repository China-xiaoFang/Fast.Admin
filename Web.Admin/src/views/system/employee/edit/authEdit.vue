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
import type { ElSelectorOutput, FaDialogInstance } from "fast-element-plus";
import { roleApi } from "@/api/services/Admin/role";
import { EmployeeAuthInput } from "@/api/services/Admin/employee/models/EmployeeAuthInput";
import { employeeApi } from "@/api/services/Admin/employee";

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
