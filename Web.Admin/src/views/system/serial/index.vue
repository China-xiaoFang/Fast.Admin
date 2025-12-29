<template>
	<div>
		<FastTable ref="fastTableRef" tableKey="1HDRYP6NFH" rowKey="serialRuleId" :requestApi="serialApi.querySerialRulePaged" hideSearchTime>
			<!-- 表格按钮操作区域 -->
			<template #header>
				<el-button type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
			</template>
			<!-- 表格操作 -->
			<template #operation="{ row }: { row: QuerySerialRulePagedOutput }">
				<el-button size="small" plain @click="editFormRef.detail(row.serialRuleId)">详情</el-button>
				<el-button size="small" plain type="primary" @click="editFormRef.edit(row.serialRuleId)">编辑</el-button>
			</template>
		</FastTable>
		<SerialEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { ref } from "vue";
import { Plus } from "@element-plus/icons-vue";
import SerialEdit from "./edit/index.vue";
import type { QuerySerialRulePagedOutput } from "@/api/services/serial/models/QuerySerialRulePagedOutput";
import type { FastTableInstance } from "@/components";
import { serialApi } from "@/api/services/serial";

defineOptions({
	name: "SystemSerial",
});

const fastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof SerialEdit>>();
</script>
