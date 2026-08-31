<template>
	<div>
		<FastTable ref="fastTableRef" table-key="1D1F6WM7DG" row-key="serialRuleId" :request-api="serialApi.querySerialRulePaged" hide-search-time>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button v-auth="'Serial:Add'" type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>

			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QuerySerialRulePagedOutput }">
				<el-button v-auth="'Serial:Detail'" size="small" plain @click="editFormRef.detail(row.serialRuleId)">详情</el-button>
				<el-button v-auth="'Serial:Edit'" size="small" plain type="primary" @click="editFormRef.edit(row.serialRuleId)">编辑</el-button>
			</template>
		</FastTable>
		<SerialEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { useTemplateRef } from "vue";
import { Plus } from "@element-plus/icons-vue";
import { serialApi } from "@/api/services/Admin/serial";
import SerialEdit from "./edit/index.vue";
import type { QuerySerialRulePagedOutput } from "@/api/services/Admin/serial/models/QuerySerialRulePagedOutput";
import type { FastTableInstance } from "@/components";

defineOptions({
	name: "SystemSerial",
});

const fastTableRef = useTemplateRef<FastTableInstance>("fastTableRef");
const editFormRef = useTemplateRef<InstanceType<typeof SerialEdit>>("editFormRef");
</script>
