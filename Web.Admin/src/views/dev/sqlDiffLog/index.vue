<template>
	<div>
		<FastTable tableKey="1D11BD21TV" rowKey="recordId" :requestApi="sqlDiffLogApi.querySqlDiffLogPaged" stripe>
			<template #mobile="{ row }: { row?: SqlDiffLogModel }">
				{{ row.nickName }}
				<br />
				手机：<span v-iconCopy="row.mobile">{{ row.mobile }}</span>
			</template>

			<template #os="{ row }: { row?: SqlDiffLogModel }">
				<span>设备：{{ row.device }}</span>
				<br />
				<span>操作系统：{{ row.os }}</span>
				<br />
				<span>浏览器：{{ row.browser }}</span>
			</template>

			<template #createdTime="{ row }: { row?: SqlDiffLogModel }">
				<span>地区：{{ row.province }} - {{ row.city }}</span>
				<br />
				<span>Ip：{{ row.ip }}</span>
				<br />
				<span>时间：{{ dayjs(row.createdTime).format("YYYY-MM-DD HH:mm:ss") }}</span>
				<el-tag v-if="row.createdTime" type="info" round effect="light" size="small" class="ml5">
					{{ dateUtil.dateTimeFix(String(row.createdTime)) }}
				</el-tag>
			</template>

			<template #beforeColumnList="{ row }: { row?: SqlDiffLogModel }">
				<el-tag
					v-if="row.beforeColumnList"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '旧的列信息';
							state.content = JSON.stringify(row.beforeColumnList) || '';
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>

			<template #afterColumnList="{ row }: { row?: SqlDiffLogModel }">
				<el-tag
					v-if="row.afterColumnList"
					type="info"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '新的列信息';
							state.content = JSON.stringify(row.afterColumnList) || '';
							state.visible = true;
						}
					"
				>
					查看
				</el-tag>
				<span v-else>--</span>
			</template>

			<template #pureSql="{ row }: { row?: SqlDiffLogModel }">
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
		<el-dialog v-model="state.visible" :title="state.title" width="1000px" alignCenter draggable destroyOnClose>
			<el-scrollbar>
				<div style="max-height: 500px; padding-bottom: 20px; padding-right: 10px">
					<VueJsonPretty
						:data="jsonContent"
						:deep="3"
						showLength
						showLineNumber
						showIcon
						virtual
						:height="500"
						:theme="configStore.layout.isDark ? 'dark' : 'light'"
					/>
				</div>
			</el-scrollbar>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { computed, reactive } from "vue";
import { dayjs } from "element-plus";
import { dateUtil } from "@fast-china/utils";
import VueJsonPretty from "vue-json-pretty";
import { sqlDiffLogApi } from "@/api/services/Center/sqlDiffLog";
import { SqlDiffLogModel } from "@/api/services/Center/sqlDiffLog/models/SqlDiffLogModel";
import { useConfig } from "@/stores";

defineOptions({
	name: "DevSqlDiffLog",
});

const configStore = useConfig();

const state = reactive({
	visible: false,
	title: "日志",
	content: "",
});

const jsonContent = computed(() => {
	try {
		return JSON.parse(state.content);
	} catch {
		return state.content;
	}
});
</script>
