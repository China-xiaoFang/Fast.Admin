<template>
	<div>
		<el-tabs type="border-card" v-model="state.activeTab">
			<el-tab-pane label="应用" name="application" lazy>
				<FastTable
					ref="fastTableRef"
					tableKey="1D1F9HNVPQ"
					rowKey="appId"
					:requestApi="applicationApi.queryApplicationPaged"
					hideSearchTime
					@customCellClick="handleCustomCellClick1"
				>
					<!-- 表格按钮操作区域 -->
					<template #header>
						<el-button v-auth="'App:Add'" type="primary" :icon="Plus" @click="editFormRef.add()">新增</el-button>
					</template>

					<template #themeColor="{ row }: { row?: QueryApplicationPagedOutput }">
						<el-tag v-if="row?.themeColor" effect="dark" :color="row.themeColor" :style="{ borderColor: row.themeColor }">
							{{ row.themeColor }}
						</el-tag>
					</template>

					<!-- 表格操作 -->
					<template #operation="{ row }: { row: QueryApplicationPagedOutput }">
						<el-button v-auth="'App:Detail'" size="small" plain @click="editFormRef.detail(row.appId)">详情</el-button>
						<el-button v-auth="'App:Edit'" size="small" plain type="primary" @click="editFormRef.edit(row.appId)">编辑</el-button>
						<el-button v-auth="'App:Delete'" size="small" plain type="danger" @click="handleDelete(row)">删除</el-button>
					</template>
				</FastTable>
			</el-tab-pane>
			<el-tab-pane label="应用OpenId" name="applicationOpenId" lazy>
				<div class="fa__display_lr-r">
					<ApplicationTree @change="handleApplicationChange" />
					<FastTable
						ref="openIdFastTableRef"
						tableKey="1D1FCQZ5KT"
						rowKey="recordId"
						:requestApi="applicationOpenIdApi.queryApplicationOpenIdPaged"
						hideSearchTime
						@customCellClick="handleCustomCellClick2"
					>
						<!-- 表格按钮操作区域 -->
						<template #header>
							<el-button v-auth="'App:Add'" type="primary" :icon="Plus" @click="openIdEditFormRef.add()">新增</el-button>
						</template>

						<!-- 表格操作 -->
						<template #operation="{ row }: { row: QueryApplicationOpenIdPagedOutput }">
							<el-button v-auth="'App:Detail'" size="small" plain @click="openIdEditFormRef.detail(row.recordId)">详情</el-button>
							<el-button v-auth="'App:Edit'" size="small" plain type="primary" @click="openIdEditFormRef.edit(row.recordId)">
								编辑
							</el-button>
							<el-button v-auth="'App:Delete'" size="small" plain type="danger" @click="handleOpenIdDelete(row)">删除</el-button>
						</template>
					</FastTable>
				</div>
			</el-tab-pane>
		</el-tabs>
		<ApplicationEdit ref="editFormRef" @ok="fastTableRef.refresh()" />
		<ApplicationOpenIdEdit ref="openIdEditFormRef" @ok="openIdFastTableRef.refresh()" />
	</div>
</template>

<script lang="ts" setup>
import { reactive, ref } from "vue";
import { Plus } from "@element-plus/icons-vue";
import ApplicationEdit from "./edit/index.vue";
import ApplicationOpenIdEdit from "./edit/applicationOpenIdEdit.vue";
import type { QueryApplicationPagedOutput } from "@/api/services/application/models/QueryApplicationPagedOutput";
import type { FastTableInstance } from "@/components";
import { applicationApi } from "@/api/services/application";
import { ElMessage, ElMessageBox } from "element-plus";
import { QueryApplicationOpenIdPagedOutput } from "@/api/services/applicationOpenId/models/QueryApplicationOpenIdPagedOutput";
import { applicationOpenIdApi } from "@/api/services/applicationOpenId";
import { ElSelectorOutput } from "fast-element-plus";

defineOptions({
	name: "SystemApplication",
});

const fastTableRef = ref<FastTableInstance>();
const openIdFastTableRef = ref<FastTableInstance>();
const editFormRef = ref<InstanceType<typeof ApplicationEdit>>();
const openIdEditFormRef = ref<InstanceType<typeof ApplicationOpenIdEdit>>();

const state = reactive({
	activeTab: "application",
});

const handleCustomCellClick1 = (_, { row }: { row: QueryApplicationPagedOutput }) => {
	editFormRef.value.detail(row.appId);
};

const handleCustomCellClick2 = (_, { row }: { row: QueryApplicationOpenIdPagedOutput }) => {
	openIdEditFormRef.value.detail(row.recordId);
};

/** 应用更改 */
const handleApplicationChange = (data: ElSelectorOutput) => {
	openIdFastTableRef.value.searchParam.appId = data.value;
	openIdFastTableRef.value.refresh();
};

/** 处理删除 */
const handleDelete = (row: QueryApplicationPagedOutput) => {
	const { appId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除应用？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await applicationApi.deleteApplication({ appId, rowVersion });
			ElMessage.success("删除成功！");
			fastTableRef.value?.refresh();
		},
	});
};

/** 处理删除 */
const handleOpenIdDelete = (row: QueryApplicationOpenIdPagedOutput) => {
	const { recordId, rowVersion } = row;
	ElMessageBox.confirm("确定要删除应用OpenId？", {
		type: "warning",
		async beforeClose(action, instance, done) {
			await applicationOpenIdApi.deleteApplicationOpenId({ recordId, rowVersion });
			ElMessage.success("删除成功！");
			openIdFastTableRef.value?.refresh();
		},
	});
};
</script>
