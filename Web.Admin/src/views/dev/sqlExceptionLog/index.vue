<template>
	<div>
		<FastTable tableKey="15UVTXHNBW" rowKey="recordId" :requestApi="sqlExceptionLogApi.querySqlExceptionLogPaged" stripe>
			<template #stackTrace="{ row }: { row?: SqlExceptionLogModel }">
				<el-tag
					v-if="row.stackTrace"
					type="danger"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '堆栈信息';
							state.content = row.stackTrace || '';
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>
			<template #rawSql="{ row }: { row?: SqlExceptionLogModel }">
				<el-tag
					v-if="row.rawSql"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '原始Sql';
							state.content = row.rawSql || '';
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>
			<template #parameters="{ row }: { row?: SqlExceptionLogModel }">
				<el-tag
					v-if="row.parameters"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = 'Sql参数';
							state.content = row.parameters || '';
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>
			<template #pureSql="{ row }: { row?: SqlExceptionLogModel }">
				<el-tag
					v-if="row.pureSql"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '纯Sql';
							state.content = row.pureSql || '';
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>
		</FastTable>
		<el-dialog v-model="state.visible" :title="state.title" width="700px" alignCenter draggable destroyOnClose>
			<el-scrollbar>
				<div style="max-height: 500px; padding-bottom: 20px; padding-right: 10px" v-html="state.content" />
			</el-scrollbar>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { sqlExceptionLogApi } from "@/api/services/sqlExceptionLog";
import { SqlExceptionLogModel } from "@/api/services/sqlExceptionLog/models/SqlExceptionLogModel";
import { reactive } from "vue";

defineOptions({
	name: "DevSqlExceptionLog",
});

const state = reactive({
	visible: false,
	title: "日志",
	content: "",
});
</script>
