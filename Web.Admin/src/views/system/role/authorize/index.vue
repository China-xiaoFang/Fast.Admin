<template>
	<FaDialog
		ref="faDialogRef"
		width="800"
		:title="state.dialogTitle"
		showConfirmButton
		confirmButtonText="保存"
		@confirm-click="handleConfirm"
	>
		<el-tabs v-model="state.activeTab">
			<el-tab-pane label="菜单权限" name="menu">
				<el-tree
					ref="menuTreeRef"
					:data="state.menuTree"
					:props="{ label: 'title', children: 'children' }"
					node-key="menuId"
					show-checkbox
					default-expand-all
					:default-checked-keys="state.checkedMenus"
				/>
			</el-tab-pane>
			<el-tab-pane label="按钮权限" name="button">
				<el-tree
					ref="buttonTreeRef"
					:data="state.buttonTree"
					:props="{ label: 'title', children: 'children' }"
					node-key="buttonId"
					show-checkbox
					default-expand-all
					:default-checked-keys="state.checkedButtons"
				/>
			</el-tab-pane>
		</el-tabs>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElMessage } from "element-plus";
import { FaDialog } from "fast-element-plus";
import type { FaDialogInstance } from "fast-element-plus";
import { roleApi } from "@/api/services/role";

defineOptions({
	name: "SystemRoleAuthorize",
});

const faDialogRef = ref<FaDialogInstance>();
const menuTreeRef = ref();
const buttonTreeRef = ref();

const state = reactive({
	roleId: 0,
	dialogTitle: "角色授权",
	activeTab: "menu",
	menuTree: [] as any[],
	buttonTree: [] as any[],
	checkedMenus: [] as number[],
	checkedButtons: [] as number[],
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		const menuIds = menuTreeRef.value.getCheckedKeys();
		const buttonIds = buttonTreeRef.value.getCheckedKeys();
		
		await roleApi.authorizeMenus({
			roleId: state.roleId,
			menuIds,
		});
		
		await roleApi.authorizeButtons({
			roleId: state.roleId,
			buttonIds,
		});
		
		ElMessage.success("授权成功！");
	});
};

const open = async (roleId: number, roleName: string) => {
	faDialogRef.value.open(async () => {
		state.roleId = roleId;
		state.dialogTitle = `角色授权 - ${roleName}`;
		state.activeTab = "menu";
		
		// Load menu tree (假设有菜单API)
		try {
			// state.menuTree = await menuApi.queryMenuTree();
			state.menuTree = []; // 临时为空，等待实际API
		} catch {
			state.menuTree = [];
		}
		
		// Load button tree (假设有按钮API)
		try {
			// state.buttonTree = await menuApi.queryButtonTree();
			state.buttonTree = []; // 临时为空，等待实际API
		} catch {
			state.buttonTree = [];
		}
		
		// Load checked menus
		state.checkedMenus = await roleApi.queryRoleMenus(roleId);
		
		// Load checked buttons
		state.checkedButtons = await roleApi.queryRoleButtons(roleId);
	});
};

defineExpose({
	element: faDialogRef,
	open,
});
</script>
