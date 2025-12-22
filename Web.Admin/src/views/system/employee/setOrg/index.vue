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
			<el-form-item label="选择组织">
				<el-tree
					ref="orgTreeRef"
					:data="state.orgTree"
					:props="{ label: 'label', children: 'children' }"
					node-key="value"
					show-checkbox
					default-expand-all
					:default-checked-keys="state.checkedOrgs"
				/>
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
import { organizationApi } from "@/api/services/organization";

defineOptions({
	name: "SystemEmployeeSetOrg",
});

const faDialogRef = ref<FaDialogInstance>();
const orgTreeRef = ref();

const state = reactive({
	employeeId: 0,
	dialogTitle: "设置组织",
	orgTree: [] as any[],
	checkedOrgs: [] as number[],
});

const handleConfirm = () => {
	faDialogRef.value.close(async () => {
		const orgIds = orgTreeRef.value.getCheckedKeys();
		
		await employeeApi.setEmployeeOrganization({
			employeeId: state.employeeId,
			organizationIds: orgIds,
		});
		
		ElMessage.success("设置成功！");
	});
};

const open = async (employeeId: number, employeeName: string) => {
	faDialogRef.value.open(async () => {
		state.employeeId = employeeId;
		state.dialogTitle = `设置组织 - ${employeeName}`;
		
		// Load organization tree
		try {
			state.orgTree = await organizationApi.organizationSelector();
		} catch {
			state.orgTree = [];
		}
		
		// Load checked organizations
		try {
			const orgs = await employeeApi.queryEmployeeOrganizations(employeeId);
			state.checkedOrgs = orgs.map((org: any) => org.organizationId);
		} catch {
			state.checkedOrgs = [];
		}
	});
};

defineExpose({
	element: faDialogRef,
	open,
});
</script>
