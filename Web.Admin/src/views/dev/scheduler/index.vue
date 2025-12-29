<template>
	<el-container v-loading="state.loading" element-loading-text="加载中...">
		<el-aside width="450px">
			<el-card class="h100">
				<el-scrollbar>
					<div style="text-align: center">
						<el-text tag="b" size="large" type="primary"> {{ state.schedulerDetail.schedulerName }}</el-text>
					</div>

					<div v-if="state.tenantName" class="pb10" style="text-align: center">
						<el-text tag="b" size="large" type="primary"> {{ state.tenantName }}</el-text>
					</div>

					<div class="mb10" style="border-bottom: var(--el-border); text-align: right">
						<el-text type="info">{{ dayjs(state.lastUpdateTime).format("YYYY-MM-DD HH:mm:ss") }}</el-text>
					</div>

					<el-form labelWidth="auto" labelSuffix="：">
						<el-form-item label="版本">
							<el-text tag="b" type="danger">v{{ state.schedulerDetail.quartzVersion }}</el-text>
						</el-form-item>
						<el-form-item label="状态">
							<div>
								<el-text tag="b">{{ state.schedulerDetail.schedulerStatus }}</el-text>
								<el-button
									v-if="state.schedulerDetail.schedulerInStandbyMode"
									class="ml10"
									size="small"
									plain
									type="primary"
									@click="handleStart"
								>
									启动
								</el-button>
								<el-button v-else class="ml10" size="small" plain type="danger" @click="handleStop">待机</el-button>
							</div>
						</el-form-item>
						<el-form-item label="集群">
							<el-text tag="b">{{ state.schedulerDetail.clustered ? "True" : "False" }}</el-text>
						</el-form-item>
						<el-form-item label="实例Id">
							<el-text tag="b">{{ state.schedulerDetail.schedulerInstanceId }}</el-text>
						</el-form-item>
						<el-form-item label="运行时间">
							<el-text>{{ state.schedulerDetail.runTimes }}</el-text>
						</el-form-item>
						<el-form-item label="远程调度器">
							<el-text>{{ state.schedulerDetail.schedulerRemote ? "True" : "False" }}</el-text>
						</el-form-item>
						<el-form-item label="存储字符串">
							<el-text>{{ state.schedulerDetail.supportsPersistence ? "True" : "False" }}</el-text>
						</el-form-item>
						<el-form-item label="调度器类型">
							<el-text>{{ state.schedulerDetail.schedulerType }}</el-text>
						</el-form-item>
						<el-form-item label="持久化类型">
							<el-text>{{ state.schedulerDetail.jobStoreType }}</el-text>
						</el-form-item>
						<el-form-item label="线程池大小">
							<el-text>{{ state.schedulerDetail.threadPoolSize }}</el-text>
						</el-form-item>
						<el-form-item label="线程池类型">
							<el-text>{{ state.schedulerDetail.threadPoolType }}</el-text>
						</el-form-item>
						<el-form-item label="作业">
							<el-text type="info">{{ state.schedulerDetail.jobCountNumber }}个</el-text>
						</el-form-item>
						<el-form-item label="触发器">
							<el-text type="info">{{ state.schedulerDetail.triggerCountNumber }}个</el-text>
						</el-form-item>
						<el-form-item label="正在执行">
							<el-text tag="b" type="warning">{{ state.schedulerDetail.jobExecuteNumber }}个</el-text>
						</el-form-item>
					</el-form>
				</el-scrollbar>
			</el-card>
		</el-aside>
		<el-main>
			<FaTable rowKey="jobName" :columns="tableColumns" :data="state.schedulerJobList" :toolBtn="false">
				<!-- 表格顶部操作区域 -->
				<template #topHeader>
					<el-menu :defaultActive="`${state.activeJobGroup}`" mode="horizontal" :ellipsis="false">
						<el-menu-item
							v-for="(item, idx) in schedulerJobGroupEnum"
							:key="idx"
							:index="item.value.toString()"
							@click="handleJobGroupChange(item.value as SchedulerJobGroupEnum)"
						>
							{{ item.label }}
						</el-menu-item>
					</el-menu>
				</template>
				<!-- 表格按钮操作区域 -->
				<template #header>
					<el-button type="primary" :icon="Plus" @click="editFormRef.add(state.tenantId, state.activeJobGroup)">添加作业</el-button>
				</template>

				<template #requestUrl="{ row }: { row?: SchedulerJobInfoDto }">
					<Tag v-if="row.requestMethod" name="HttpRequestMethodEnum" :value="row.requestMethod" />
					<br v-if="row.requestUrl" />
					<span v-if="row.requestUrl">{{ row.requestUrl }}</span>
					<span v-else>--</span>
				</template>

				<template #exception="{ row }: { row?: SchedulerJobInfoDto }">
					<el-tag
						v-if="row.exception"
						type="danger"
						style="cursor: pointer"
						closable
						@click="handleShowException(row)"
						@close="handleDelException(row)"
					>
						查看
					</el-tag>
					<span v-else>--</span>
				</template>

				<template #fireTime="{ row }: { row?: SchedulerJobInfoDto }">
					<el-text v-if="row.previousFireTime" type="info">{{ row.previousFireTime }}</el-text>
					<span v-else>- -</span>
					<br />
					<el-text v-if="row.nextFireTime" type="primary">{{ row.nextFireTime }}</el-text>
					<span v-else>- -</span>
				</template>

				<template #time="{ row }: { row?: SchedulerJobInfoDto }">
					<el-text type="info">{{ row.beginTime }}</el-text>
					<br />
					<el-text v-if="row.endTime" type="primary">{{ row.endTime }}</el-text>
					<span v-else>- -</span>
				</template>

				<!-- 表格操作 -->
				<template #operation="{ row }: { row: SchedulerJobInfoDto }">
					<div class="mb5">
						<el-button size="small" @click="editFormRef.edit(state.tenantId, row.jobName, state.activeJobGroup)">编辑</el-button>
						<el-button size="small" @click="editFormRef.copy(state.tenantId, row.jobName, state.activeJobGroup)">复制</el-button>
						<el-button v-if="row.triggerState == TriggerState.Paused" size="small" plain type="primary" @click="handleResumeJob(row)">
							恢复
						</el-button>
						<el-button v-else size="small" plain type="warning" @click="handleStopJob(row)">暂停</el-button>
					</div>
					<el-button size="small" plain type="info" @click="handleLogs(row)">日志</el-button>
					<el-button size="small" plain type="warning" @click="handleTriggerJob(row)">执行</el-button>
					<el-button size="small" plain type="danger" @click="handleDelJob(row)">删除</el-button>
				</template>
			</FaTable>
			<el-dialog v-model="state.exp.visible" :title="state.exp.title" width="700px" alignCenter draggable destroyOnClose>
				<div class="log_content" v-html="state.exp.content" />
			</el-dialog>
			<el-dialog v-model="state.log.visible" :title="state.log.title" width="700px" alignCenter draggable destroyOnClose>
				<el-scrollbar v-loading="state.log.loading" element-loading-text="加载中..." height="500px">
					<div v-for="(item, index) in state.log.contents" :key="index" class="log_content" v-html="item" />
				</el-scrollbar>
			</el-dialog>
			<SchedulerEdit ref="editFormRef" @ok="handleTableRefresh" />
		</el-main>
	</el-container>
</template>

<script lang="ts" setup>
import { onActivated, onDeactivated, onMounted, onUnmounted, reactive, ref } from "vue";
import { ElMessage, ElMessageBox, dayjs } from "element-plus";
import { Plus } from "@element-plus/icons-vue";
import { withDefineType } from "@fast-china/utils";
import SchedulerEdit from "./edit/index.vue";
import type { QuerySchedulerDetailOutput } from "@/api/services/scheduler/models/QuerySchedulerDetailOutput";
import type { SchedulerJobInfoDto } from "@/api/services/scheduler/models/SchedulerJobInfoDto";
import type { FaTableColumnCtx, FaTableEnumColumnCtx } from "fast-element-plus";
import { SchedulerJobGroupEnum } from "@/api/enums/SchedulerJobGroupEnum";
import { TriggerState } from "@/api/enums/TriggerState";
import { schedulerApi } from "@/api/services/scheduler";
import { useApp } from "@/stores";

defineOptions({
	name: "DevScheduler",
});

const appStore = useApp();

const editFormRef = ref<InstanceType<typeof SchedulerEdit>>();

const schedulerJobGroupEnum = appStore.getDictionary("SchedulerJobGroupEnum");
const triggerState = withDefineType<FaTableEnumColumnCtx[]>([
	{
		label: "正常",
		value: TriggerState.Normal,
	},
	{
		label: "暂停",
		value: TriggerState.Paused,
	},
	{
		label: "完成",
		value: TriggerState.Complete,
	},
	{
		label: "异常",
		value: TriggerState.Error,
	},
	{
		label: "阻塞",
		value: TriggerState.Blocked,
	},
	{
		label: "不存在",
		value: TriggerState.None,
	},
]);

const state = reactive({
	/** 加载状态 */
	loading: false,
	/** 请求状态 */
	isRequesting: false,
	/** 定时器 */
	interval: withDefineType<NodeJS.Timeout>(),
	/** 租户Id */
	tenantId: withDefineType<number>(),
	/** 租户名称 */
	tenantName: "",
	/** 激活作业分组 */
	activeJobGroup: SchedulerJobGroupEnum.System,
	/** 调度器详情 */
	schedulerDetail: withDefineType<QuerySchedulerDetailOutput>({}),
	/** 调度作业 */
	schedulerJobList: withDefineType<SchedulerJobInfoDto[]>([]),
	/** 最后更新时间 */
	lastUpdateTime: new Date(),
	exp: {
		visible: false,
		title: "异常",
		content: "",
	},
	log: {
		loading: false,
		visible: false,
		title: "日志",
		contents: withDefineType<string[]>([]),
	},
});

/** 表格刷新 */
const handleTableRefresh = async () => {
	const apiRes = await schedulerApi.queryAllSchedulerJob(state.activeJobGroup, state.tenantId);
	state.schedulerJobList = apiRes[0]?.jobInfoList ?? [];
};

/** 停止定时器 */
const stopInterval = () => {
	if (state.interval) {
		clearInterval(state.interval);
		state.interval = null;
	}
};

/** 启动定时器 */
const startInterval = () => {
	if (!state.interval) {
		state.interval = setInterval(async () => {
			if (state.isRequesting) return;
			state.isRequesting = true;
			state.lastUpdateTime = new Date();
			[state.schedulerDetail] = await Promise.all([schedulerApi.querySchedulerDetail(state.tenantId), handleTableRefresh()]).finally(() => {
				state.isRequesting = false;
			});
		}, 5000);
	}
};

/** 作业分组改变 */
const handleJobGroupChange = (value: SchedulerJobGroupEnum) => {
	if (value === state.activeJobGroup) return;
	state.activeJobGroup = value;
	handleTableRefresh();
};

/** 调度程序启动 */
const handleStart = () => {
	ElMessageBox.confirm(`确定要启动【${state.schedulerDetail.schedulerName}】？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await schedulerApi.startScheduler(state.tenantId);
			ElMessage.success("启动成功！");
			state.schedulerDetail.schedulerInStandbyMode = false;
		},
	});
};

/** 调度程序停止 */
const handleStop = () => {
	ElMessageBox.confirm(`确定要待机【${state.schedulerDetail.schedulerName}】？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await schedulerApi.stopScheduler(state.tenantId);
			ElMessage.success("待机成功！");
			state.schedulerDetail.schedulerInStandbyMode = true;
		},
	});
};

/** 异常查看 */
const handleShowException = (row: SchedulerJobInfoDto) => {
	state.exp.title = `异常 - ${row.jobName}`;
	state.exp.content = row.exception;
	state.exp.visible = true;
};

/** 异常删除 */
const handleDelException = async (row: SchedulerJobInfoDto) => {
	ElMessageBox.confirm(`确定要删除【${row.jobName}】的异常信息？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await schedulerApi.deleteSchedulerJobException(state.tenantId, {
				jobName: row.jobName,
				jobGroup: state.activeJobGroup,
			});
			ElMessage.success("删除成功！");
			handleTableRefresh();
		},
	});
};

/** 处理日志查看 */
const handleLogs = async (row: SchedulerJobInfoDto) => {
	state.log.contents = [];
	state.log.title = `日志 - ${row.jobName}`;
	state.log.visible = true;
	state.log.loading = true;
	state.log.contents = await schedulerApi
		.querySchedulerJobLogs(state.tenantId, {
			jobName: row.jobName,
			jobGroup: state.activeJobGroup,
		})
		.finally(() => {
			state.log.loading = false;
		});
};

/** 暂停调度作业 */
const handleStopJob = async (row: SchedulerJobInfoDto) => {
	ElMessageBox.confirm(`确定要暂停【${row.jobName}】？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await schedulerApi.stopSchedulerJob(state.tenantId, {
				jobName: row.jobName,
				jobGroup: state.activeJobGroup,
			});
			ElMessage.success("暂停成功！");
			handleTableRefresh();
		},
	});
};

/** 恢复调度作业 */
const handleResumeJob = async (row: SchedulerJobInfoDto) => {
	ElMessageBox.confirm(`确定要恢复【${row.jobName}】？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await schedulerApi.resumeSchedulerJob(state.tenantId, {
				jobName: row.jobName,
				jobGroup: state.activeJobGroup,
			});
			ElMessage.success("恢复成功！");
			handleTableRefresh();
		},
	});
};

/** 执行调度作业 */
const handleTriggerJob = async (row: SchedulerJobInfoDto) => {
	ElMessageBox.confirm(`确定要立即执行【${row.jobName}】？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await schedulerApi.triggerSchedulerJob(state.tenantId, {
				jobName: row.jobName,
				jobGroup: state.activeJobGroup,
			});
			ElMessage.success("执行成功！");
			handleTableRefresh();
		},
	});
};

/** 删除调度作业 */
const handleDelJob = async (row: SchedulerJobInfoDto) => {
	ElMessageBox.confirm(`确定要删除【${row.jobName}】？`, {
		type: "warning",
		async beforeClose(action, instance, done) {
			await schedulerApi.deleteSchedulerJob(state.tenantId, {
				jobName: row.jobName,
				jobGroup: state.activeJobGroup,
			});
			ElMessage.success("删除成功！");
			handleTableRefresh();
		},
	});
};

const tableColumns = withDefineType<FaTableColumnCtx[]>([
	{
		prop: "jobName",
		label: "作业名称",
		fixed: "left",
		width: 300,
		minWidth: 280,
	},
	{
		prop: "triggerState",
		label: "状态",
		tag: true,
		enum: triggerState,
		width: 100,
		minWidth: 80,
	},
	{
		prop: "runNumber",
		label: "触发次数",
		width: 100,
		minWidth: 80,
	},
	{
		prop: "jobType",
		label: "任务类型",
		tag: true,
		enum: appStore.getDictionary("SchedulerJobTypeEnum"),
		width: 100,
		minWidth: 80,
	},
	{
		prop: "requestUrl",
		label: "请求地址",
		slot: "requestUrl",
		width: 300,
		minWidth: 280,
	},
	{
		prop: "exception",
		label: "异常信息",
		slot: "exception",
		width: 100,
		minWidth: 80,
	},
	{
		prop: "triggerType",
		label: "触发器类型",
		tag: true,
		enum: appStore.getDictionary("TriggerTypeEnum"),
		width: 120,
		minWidth: 100,
	},
	{
		prop: "fireTime",
		label: "执行时间",
		slot: "fireTime",
		width: 180,
		minWidth: 160,
	},
	{
		prop: "time",
		label: "开始结束时间",
		slot: "time",
		width: 180,
		minWidth: 160,
	},
	{
		prop: "interval",
		label: "执行计划",
		width: 150,
		minWidth: 130,
	},
	{
		prop: "description",
		label: "描述",
		width: 300,
		minWidth: 280,
	},
]);
onMounted(async () => {
	state.loading = true;
	[state.schedulerDetail] = await Promise.all([schedulerApi.querySchedulerDetail(state.tenantId), handleTableRefresh()])
		.finally(() => {
			state.isRequesting = false;
		})
		.finally(() => {
			state.loading = false;
		});
	startInterval();
});

onActivated(() => {
	startInterval();
});

onDeactivated(() => {
	stopInterval();
});

onUnmounted(() => {
	stopInterval();
});
</script>

<style lang="scss" scoped>
.el-card {
	padding: 0;
	:deep(.el-card__body) {
		height: 100%;
		padding-right: 10px;
		.el-scrollbar {
			padding-right: 10px;
		}
	}
}
.el-main {
	--el-main-padding: 0 0 0 5px;
}
.el-form {
	.el-form-item {
		margin-bottom: 0;
	}
	:deep() {
		.el-form-item__label {
			padding: 0 6px 0 0;
		}
	}
}
.fa-table {
	:deep() {
		.fa-table__header {
			background-color: var(--el-menu-bg-color);
			padding: 0 10px;
			.el-menu {
				--el-menu-horizontal-height: 50px;
				width: 100%;
				border: none;
			}
		}
	}
}
:deep(.el-dialog) {
	.log_content {
		padding-bottom: 20px;
		padding-right: 10px;
		.logList {
			padding-left: 10px;
			border-left: 3px solid var(--el-color-primary);
			&.error {
				border-left: 3px solid var(--el-color-danger);
			}
			.time {
				display: inline-block;
				padding-bottom: 3px;
				font-weight: 500;
				color: var(--el-color-success);
			}
			.execTime {
				font-weight: 500;
				color: var(--el-color-warning);
			}
			.url {
				color: var(--el-color-primary);
			}
			.params {
				font-weight: 500;
				color: var(--el-color-danger);
			}
			.headers {
				color: var(--el-color-success);
			}
			.result {
				color: var(--el-color-primary);
				font-weight: 500;
			}
			.error {
				color: var(--el-color-danger);
				font-weight: 500;
			}
		}
	}
}
</style>
