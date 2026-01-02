<template>
	<div>
		<FastTable tableKey="1HDVHRLPGU" rowKey="recordId" :requestApi="exceptionLogApi.queryExceptionLogPaged" stripe>
			<template #stackTrace="{ row }: { row?: ExceptionLogModel }">
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
			<template #paramsObj="{ row }: { row?: ExceptionLogModel }">
				<el-tag
					v-if="row.paramsObj"
					type="danger"
					style="cursor: pointer"
					@click="
						() => {
							state.title = '参数对象';
							state.content = row.paramsObj || '';
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
import { exceptionLogApi } from "@/api/services/exceptionLog";
import { ExceptionLogModel } from "@/api/services/exceptionLog/models/ExceptionLogModel";
import { reactive } from "vue";

defineOptions({
	name: "DevExceptionLog",
});

const state = reactive({
	visible: false,
	title: "日志",
	content: "",
});
</script>
