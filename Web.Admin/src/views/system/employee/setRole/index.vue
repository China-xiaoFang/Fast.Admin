<template>
	<FaDialog
		ref="faDialogRef"
		width="600"
		:title="state.dialogTitle"
		showConfirmButton
		confirmButtonText="保存"
		@confirm-click="handleConfirm"
	>
		<el-form label-width="100px">
			<el-form-item label="选择角色">
				<el-checkbox-group v-model="state.checkedRoles">
					<el-checkbox v-for="role in state.roleList" :key="role.value" :value="role.value">
						{{ role.label }}
					</el-checkbox>
				</el-checkbox-group>
			</el-form-item>
		</el-form>
	</FaDialog>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { ElMessage } from "element-plus";
import { FaDialog } from "fast-element-plus";
import { withDefineType } from "@fast-china/utils";
import type { FaDialogInstance } from "fast-element-plus";
import { employeeApi } from "@/api/services/employee";
import { roleApi } from "@/api/services/role";

defineOptions({
	name: "SystemEmployeeSetRole",
});

const faDialogRef = ref<FaDialogInstance>();

const state = reactive({
	employeeId: 0,
	dialogTitle: "设置角色",
	roleList: [] as any[],
	checkedRoles: [] as number[],
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		await employeeApi.setEmployeeRole({
			employeeId: state.employeeId,
			roleIds: state.checkedRoles,
		});
		
		ElMessage.success("设置成功！");
	});
};

const open = async (employeeId: number, employeeName: string) => {
	faDialogRef.value.open(async () => {
		state.employeeId = employeeId;
		state.dialogTitle = `设置角色 - ${employeeName}`;
		
		// Load role list
		try {
			state.roleList = await roleApi.roleSelector();
		} catch {
			state.roleList = [];
		}
		
		// Load checked roles
		try {
			state.checkedRoles = await employeeApi.queryEmployeeRoles(employeeId);
		} catch {
			state.checkedRoles = [];
		}
	});
};

defineExpose({
	element: faDialogRef,
	open,
});
</script>
