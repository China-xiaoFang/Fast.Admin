<template>
	<div>
		<FaTable
			ref="faTableRef"
			rowKey="positionId"
			:columns="tableColumns"
			:requestApi="positionApi.queryPositionPaged"
			hideSearchTime
		>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }">
				<el-button size="small" plain @click="editFormRef.detail(row.positionId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.positionId)">编辑</el-button>
				<el-button size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
			</template>
		</FaTable>
		<PositionEdit ref="editFormRef" @ok="faTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, ElMessageBox } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import { withDefineType } from "@fast-china/utils";
import PositionEdit from "./edit/index.vue";
import type { FaTableColumnCtx, FaTableInstance } from "fast-element-plus";
import { positionApi } from "@/api/services/position";

defineOptions({
	name: "SystemPosition",
});

const faTableRef = ref<FaTableInstance>();
const editFormRef = ref<InstanceType<typeof PositionEdit>>();

/** 处理删除 */
const handleDelete = (row: any) => {
	const { positionId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除该职位？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			if (action === "confirm") {
				await positionApi.deletePosition({ positionId, rowVersion });
				ElMessage.success("删除成功！");
				faTableRef.value?.refresh();
				done();
			} else {
				done();
			}
		},
	});
};

const tableColumns = withDefineType<FaTableColumnCtx[]>([
	{
		prop: "positionName",
		label: "职位名称",
		sortable: true,
		link: true,
		click({ row }) {
			editFormRef.value.detail(row.positionId);
		},
		fixed: "left",
		width: 200,
	},
	{
		prop: "positionCode",
		label: "职位编码",
		sortable: true,
		copy: true,
		width: 200,
	},
	{
		prop: "status",
		label: "状态",
		sortable: true,
		tag: true,
		enum: [
			{ label: "正常", value: 1, type: "primary" },
			{ label: "禁用", value: 2, type: "danger" },
		],
		width: 100,
	},
	{
		prop: "remark",
		label: "备注",
		sortable: true,
		width: 280,
	},
	{
		type: "submitInfo",
		prop: "createdTime",
		label: "创建时间",
		sortable: true,
		width: 180,
	},
	{
		type: "submitInfo",
		prop: "updatedTime",
		label: "更新时间",
		sortable: true,
		submitInfoField: {
			submitClerkName: "updatedUserName",
			submitTime: "updatedTime",
		},
		width: 180,
	},
]);
</script>
