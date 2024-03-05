import { defineComponent, ref, reactive, PropType, SetupContext, provide, onMounted, watch, toRefs } from "vue";
import type { FTableProps, FTableState, FTableEmits, FTableColumn, FTableBreakPoint } from "./interface";
import { ElTable } from "element-plus";
import { Refresh, Search, More } from "@element-plus/icons-vue";
import "./style/index.scss"
import { arrayDynamicSort, getRowspanMethod } from "./utils";
import SearchForm from "./modules/SearchForm"
import TableColumn from "./modules/Column"
import Pagination from "./modules/Pagination"
import { useI18n } from "vue-i18n";
import notDataImage from "@/assets/images/notData.png";

export default defineComponent({
    name: "FTable",
    props: {
        data: {
            type: Array as PropType<anyObj[]>,
            default: []
        },
        requestAuto: {
            type: Boolean,
            default: true
        },
        initParam: {
            type: Object as PropType<PagedInput & anyObj>,
            default: {},
        },
        requestApi: {
            type: Function as PropType<(params: PagedInput & anyObj) => ApiPromise<PagedResult<anyObj>> | ApiPromise<Array<anyObj>>>,
            require: false
        },
        dataCallback: {
            type: Function as PropType<(data: anyObj) => void>,
            require: false
        },
        columns: {
            type: Array as PropType<FTableColumn[]>,
            default: []
        },
        pagination: {
            type: Boolean,
            default: true,
        },
        searchFormColumns: {
            type: Object as PropType<number | Record<FTableBreakPoint, number>>,
            default: () => ({ xs: 1, sm: 2, md: 2, lg: 4, xl: 5 })
        },
        rowKey: {
            type: String,
            default: "id",
        },
        showSearchForm: {
            type: Boolean,
            default: true,
        },
        showHeaderCard: {
            type: Boolean,
            default: true,
        },
        showRefreshBtn: {
            type: Boolean,
            default: true,
        },
        showSearchBtn: {
            type: Boolean,
            default: true,
        },
        showColumnBtn: {
            type: Boolean,
            default: true,
        },
        toolButton: {
            type: Boolean,
            default: true,
        },
        treeData: {
            type: Boolean,
            default: false,
        },
        treeChildrenName: {
            type: String,
            default: "children",
        },
        singleChoice: {
            type: Boolean,
            default: false,
        },
    },
    setup(props: FTableProps, { attrs, slots, emit, expose }: SetupContext<FTableEmits>) {
        const { t } = useI18n();

        const state: FTableState = reactive({
            loading: false,
            orgColumns: props.columns,
            searchColumns: [],
            tableColumns: [],
            tableData: [],
            tablePagination: {
                pageIndex: 1,
                pageSize: 20,
                totalRows: 0
            },
            searchParam: {},
            showSearch: true,
            selected: false,
            selectedList: [],
            selectedListIds: []
        })

        // el-table Dom 元素
        const tableRef = ref<InstanceType<typeof ElTable>>();

        /**
         * 处理Table数据
         * @param data 
         */
        const handleTableData = (data: any[]) => {
            if (props.treeData) {
                let result: any[] = [];
                data.forEach(row => {
                    let rowList = row[props.treeChildrenName] as unknown[];
                    if (rowList && rowList.length > 0) {
                        rowList.forEach(tRow => {
                            result.push({ ...(row as any), ...(tRow as any) });
                        });
                    } else {
                        result.push(row as any);
                    }
                });
                return result;
            } else {
                return data;
            }
        }

        // 处理表格列
        state.tableColumns = props.columns.filter(f => !f.pureSearch).sort((a, b) => {
            if (a.fixed) {
                return 1;
            } else {
                const _a = (a.order || 1000) as number;
                const _b = (b.order || 1000) as number;
                return _a - _b;
            }
        })

        // 处理表格数据
        state.tableData = handleTableData(props.data);

        /**
         * 更新查询参数
         * */
        const updatedTotalParam = () => {
            // searchParam.value = {}
            // 处理查询参数，可以给查询参数加自定义前缀操作
            const newSearchParam: { [key: string]: any } = {};
            // 防止手动清空输入框携带参数（这里可以自定义查询参数前缀）
            for (const key in state.searchParam) {
                // * 某些情况下参数为 false/0 也应该携带参数
                if (state.searchParam[key] || state.searchParam[key] === false || state.searchParam[key] === 0) {
                    newSearchParam[key] = state.searchParam[key];
                }
                // 处理某些情况下如果为空字符串，其实是不需要传到后端的
                else if (!state.searchParam[key]) {
                    delete state.searchParam[key];
                }
            }
            Object.assign(state.searchParam, newSearchParam);
        };

        /**
         * 表格数据查询
         */
        const tableSearch = () => {
            // 重置到第一页
            state.tablePagination.pageIndex = 1;
            updatedTotalParam();
            loadData();
        };

        /**
         * 表格数据重置
         */
        const tableReset = () => {
            // 重置到第一页
            state.tablePagination.pageIndex = 1;
            // 清除搜索条件
            state.searchParam = {};
            // 重置搜索表单的时候，如果有默认搜索参数，则重置默认的搜索参数
            Object.keys(props.initParam).forEach(key => {
                state.searchParam[key] = props.initParam[key];
            });
            updatedTotalParam();
            emit("tableReset");
            loadData();
        };

        /**
         * 定义 enumMap 存储 enum 值（避免异步请求无法格式化单元格内容 || 无法填充搜索下拉选择）
         */
        const enumMap = ref(new Map<string, { [key: string]: any }[]>());
        provide("enumMap", enumMap);
        const setEnumMap = (column: FTableColumn) => {
            if (!column.enum) return;
            // 如果当前 enum 为后台数据需要请求数据，则调用该请求接口，并存储到 enumMap
            if (typeof column.enum !== "function") return enumMap.value.set(column.prop!, column.enum!);
            column.enum().then(res => {
                enumMap.value.set(column.prop!, res.data);
            });
        };

        /**
         * 扁平化 columns
         */
        const flatColumnsFunc = (columns: FTableColumn[], flatArr: FTableColumn[] = []) => {
            columns.forEach(col => {
                if (col._children?.length) flatArr.push(...flatColumnsFunc(col._children));
                flatArr.push(col);

                // 给每一项 column 添加 show && filterEnum 默认属性
                col.show = col.show ?? true;
                col.filterEnum = col.filterEnum ?? col.tag ?? false;

                // 设置 enumMap
                setEnumMap(col);
            });
            return flatArr.filter(item => !item._children?.length);
        };

        // 表格搜素列
        state.searchColumns = flatColumnsFunc(props.columns.filter(f => f.search?.el || f.pureSearch))
            .sort((a, b) => {
                const _a = (a.search?.order || 1000) as number;
                const _b = (b.search?.order || 1000) as number;
                return _a - _b;
            });

        /**
         * 自定义索引
         * @param index
         */
        const indexMethod = (index: number) => {
            return index + (state.tablePagination.pageIndex - 1) * state.tablePagination.pageSize + 1;
        };

        /**
         * 选中改变
         * @param selection 多选的数据
         * @param row 单行数据
         */
        const handleSelectClick = (selection: any[], row: any) => {
            // 判断是否开启了单选
            if (props.singleChoice) {
                tableRef.value?.clearSelection();
                if (row && selection.length) {
                    tableRef.value?.toggleRowSelection(row, true);
                }
            }
        };

        /**
         * 多选操作
         * @param rowArr 当前选择的所有的数据
         */
        const handleSelectionChange = (rowArr: any) => {
            rowArr.length === 0 ? (state.selected = false) : (state.selected = true);
            // 判断是否为单选
            if (props.singleChoice && rowArr.length > 0) {
                // 这里获取最后一个是因为选中改变的事件会触发多次，会带入旧的数据
                state.selectedList = [rowArr[rowArr.length - 1]];
            } else {
                state.selectedList = rowArr;
            }
            emit("selectionChange", state.selectedList);
        };

        /**
         * 设置列的排序为我们自定义的排序
         * @param param0
         */
        const handleHeaderClass = ({ column }) => {
            column.order = column.multiOrder;
        };

        /**
         * 当某个表头排序改变时会触发该事件
         * @param column
         */
        const handleSortChange = ({ column }) => {
            if (!column.multiOrder) {
                column.multiOrder = "descending";
            } else if (column.multiOrder === "descending") {
                column.multiOrder = "ascending";
            } else {
                column.multiOrder = null;
            }
            // 排序集合非空判断
            state.searchParam.sortList = Object.assign(props.initParam?.sortList ?? [], state.searchParam?.sortList ?? []);
            // 去原来的列中查找表格的列数据
            let orgColumn = state.orgColumns.find(f => f.prop == column.property);
            if (orgColumn == null) {
                orgColumn = column;
            }
            const enField = orgColumn?.sortableField ?? orgColumn?.prop ?? orgColumn?.property;
            const fieldIndex = state.searchParam.sortList.findIndex((f: PagedSortInput) => f.enField === enField);
            if (!column.multiOrder) {
                // 如果是空的，删除排序
                state.searchParam.sortList.splice(fieldIndex, 1);
            } else if (fieldIndex === -1) {
                state.searchParam.sortList.push({
                    enField: enField,
                    cnField: orgColumn?.label,
                    mode: column.multiOrder
                });
            } else {
                state.searchParam.sortList[fieldIndex].mode = column.multiOrder;
            }
            // 判断最后的排序集合中是否还存在数据，如果不存在，则删除排序集合
            if (state.searchParam.sortList.length === 0) {
                delete state.searchParam.sortList;
            }
            // 判断请求接口的方法是否为空
            if (props.requestApi) {
                // 刷新表格数据
                loadData();
            } else {
                state.tableData = state.tableData.sort(arrayDynamicSort(state.searchParam.sortList ?? []));
            }

            emit("sortChange", { column, prop: enField, order: column.multiOrder });
        };

        /**本地匹配搜索 */
        const filterByLocal = () => {
            const _value = handleTableData(props.data);
            state.tableData = _value.filter(f => {
                const _tagAry = [] as boolean[];
                Object.keys(state.searchParam).forEach(e => {
                    const param = state.searchParam[e]?.toLowerCase();
                    if (e == "searchValue") {
                        state.tableColumns.map(col => {
                            _tagAry.push(!param || f[col.prop]?.toLowerCase().includes(param))
                        })
                    } else {
                        _tagAry.push(!param || f[e]?.toLowerCase().includes(param))
                    }
                });
                return _tagAry.every(e => e);
            });
        };

        /**
         * 清空选中数据列表
         */
        const clearSelection = () => tableRef.value!.clearSelection();

        /**
         * 选中某一行
         * @param row
         * @param selected
         */
        const toggleRowSelection = (row: any, selected: boolean) => tableRef.value!.toggleRowSelection(row, selected);

        /**
         * 合并行列
         */
        const handleSpanMethod = () => {
            return getRowspanMethod(state.tableData,
                (props.columns.filter(f => f.span == true) ?? []).map(col => {
                    return {
                        prop: col.prop,
                        spanProp: col.spanProp ?? col.prop
                    };
                })
            )
        };

        /**
         * 获取请求参数
         */
        const getRequestParam = () => {
            const param = { ...props.initParam, ...state.searchParam, ...(props.pagination ? state.tablePagination : {}) };
            // 删除总条数
            delete param.totalRows;
            return param;
        };

        /**
         * 加载数据
         */
        const loadData = () => {
            // 判断是否需要自动请求
            if (!props.requestAuto) return;

            if (props.requestApi === undefined) {
                throw new Error("如果需要加载FTable数据，那么参数 requestApi 是必须的！");
            }

            state.loading = true;
            const param = getRequestParam();
            props.requestApi(param).then(res => {
                if (res.success) {
                    props.dataCallback && props.dataCallback(res.data);
                    let pageData: anyObj[];
                    // 解析 API 接口返回的分页数据（如果有分页更新分页信息）
                    if (props.pagination) {
                        let pagedResult = res.data as PagedResult<anyObj[]>;
                        pageData = pagedResult.rows;
                        // 更新分页信息
                        Object.assign(state.tablePagination, {
                            pageIndex: pagedResult.pageIndex,
                            pageSize: pagedResult.pageSize,
                            totalRows: pagedResult.totalRows
                        });
                    } else {
                        pageData = res.data as anyObj[];
                    }
                    state.tableData = handleTableData(pageData);
                }
            }).catch(() => {
                state.tableData = [];
            }).finally(() => {
                state.loading = false;
            })
        };

        /**
         * 每页条数改变
         * @param val 当前条数
         */
        const handleSizeChange = (val: number) => {
            state.tablePagination.pageIndex = 1;
            state.tablePagination.pageSize = val;
            loadData();
            emit("sizeChange", val);
            emit("paginationChange", 1, val);
        };

        /**
         * 当前页改变
         * @param val 当前页
         */
        const handleCurrentChange = (val: number) => {
            state.tablePagination.pageIndex = val;
            loadData();
            emit("sizeChange", val);
            emit("paginationChange", val, state.tablePagination.pageSize);
        };

        /**
         * 刷新表格
         */
        const refreshTable = () => {
            if (!props.requestAuto) {
                filterByLocal();
            } else {
                loadData();
            }
        };

        /**
         * 页面渲染
         * 初始化请求
         */
        onMounted(() => {
            loadData();
        });

        /**
         * 监听事件
         * 监听非 API 请求的 Table Data
         */
        watch(
            () => props.data,
            () => {
                state.tableData = handleTableData(props.data);
            },
            { deep: true, immediate: true }
        );

        // 暴漏出去的方法
        expose({
            element: tableRef,
            ...toRefs(state),
            loadData,
            tableReset,
            clearSelection,
            toggleRowSelection,
            refreshTable,
        });

        return () => (
            <div class="f-table">
                {
                    props.showSearchForm ? (
                        <SearchForm
                            loading={state.loading}
                            columns={state.searchColumns}
                            searchParam={state.searchParam}
                            searchFormColumns={props.searchFormColumns}
                            search={tableSearch}
                            reset={tableReset}
                        >
                            {
                                Object.keys(slots).map((slot: string) => (
                                    {
                                        [slot]: (scope) => (
                                            <>
                                                {slots[slot](scope)}
                                            </>
                                        )
                                    }
                                ))
                            }
                        </SearchForm>
                    ) : (null)
                }
                <div>
                    {slots.tableTopHeader && slots.tableTopHeader()}
                </div>
                <div class="f-table-main el-card">
                    {
                        props.showHeaderCard ? (
                            <div class="f-table-main-header">
                                <div class="f-table-main-header-left">
                                    {slots.tableHeader && slots.tableHeader(state)}
                                </div>
                                {
                                    props.toolButton ? (
                                        <>
                                            <div class="f-table-main-header-right">
                                                <el-input
                                                    loading={state.loading}
                                                    prefix-icon={Search}
                                                    placeholder={t("components.FTable.关键字搜索")}
                                                    v-model={[state.searchParam.searchValue, ['trim']]}
                                                    clearable
                                                    style="margin-right: 12px; width: 235px; margin-bottom: 5px"
                                                    onChange={refreshTable()}
                                                />
                                                {slots.toolButton && slots.toolButton(state)}
                                                {
                                                    props.showRefreshBtn ? (
                                                        <el-button
                                                            loading={state.loading}
                                                            title={t("components.FTable.刷新")}
                                                            circle
                                                            icon={Refresh}
                                                            onClick={refreshTable}
                                                        ></el-button>
                                                    ) : (null)
                                                }
                                                {
                                                    props.showSearchBtn && state.searchColumns.length ? (
                                                        <el-button
                                                            loading={state.loading}
                                                            title={state.showSearch ? t("components.FTable.隐藏搜索栏") : t("components.FTable.显示搜索栏")}
                                                            circle
                                                            icon={Search}
                                                            onClick={() => state.showSearch = !state.showSearch}
                                                        ></el-button>
                                                    ) : (null)
                                                }
                                                {
                                                    slots.toolButtonAdv ? (
                                                        <el-dropdown
                                                            loading={state.loading}
                                                            title={t("components.FTable.高级操作")}
                                                            trigger="click"
                                                            style="margin-left: 12px"
                                                        >
                                                            {{
                                                                default: () => (
                                                                    <el-button
                                                                        loading={state.loading}
                                                                        circle
                                                                        icon={More}
                                                                    ></el-button>
                                                                ),
                                                                dropdown: () =>
                                                                (
                                                                    <el-dropdown-menu>
                                                                        {slots.toolButtonAdv()}
                                                                    </el-dropdown-menu>
                                                                )
                                                            }}
                                                        </el-dropdown>
                                                    ) : (null)
                                                }
                                            </div>
                                        </>
                                    ) : (null)
                                }
                            </div>
                        ) : (null)
                    }
                    <el-table
                        {...attrs}
                        scrollbar-always-on
                        ref={tableRef}
                        size="small"
                        table-layout="fixed"
                        v-loading={state.loading}
                        element-loading-text={t("components.FTable.加载中")}
                        data={state.tableData}
                        border={props.border}
                        row-key={props.rowKey}
                        span-method={handleSpanMethod}
                        headerCellClassName={handleHeaderClass}
                        onSelectionChange={handleSelectionChange}
                        onSortChange={handleSortChange}
                        onSelect={handleSelectClick}
                        onSelectAll={handleSelectClick}
                    >
                        {{
                            append: () => (
                                <>
                                    {slots.append && slots.append()}
                                </>
                            ),
                            empty: () => (
                                <div class="table-empty">
                                    {
                                        slots.empty ? (
                                            slots.empty()
                                        ) : (
                                            <>
                                                <img src={notDataImage} alt="notData" />
                                                <div>{t("components.FTable.暂无数据")}</div>
                                            </>
                                        )
                                    }
                                </div>
                            ),
                            default: () => (
                                <>
                                    <el-table-column
                                        type="selection"
                                        fixed="left"
                                        width={35}
                                        align="left"
                                        reserve-selection
                                    />
                                    {
                                        state.tableColumns?.length === 0 ? (
                                            <>
                                                {slots.default && slots.default()}
                                            </>
                                        ) : (
                                            state.tableColumns.map((col) => (
                                                <>
                                                    {
                                                        col.type === "index" ? (
                                                            <el-table-column
                                                                {...col}
                                                                fixed={col.fixed ?? 'left'}
                                                                width={col.width ?? 50}
                                                                align={col.align ?? 'center'}
                                                                index={indexMethod}
                                                            />
                                                        ) : (null)
                                                    }
                                                    {
                                                        col.type === "expand" ? (
                                                            <el-table-column
                                                                {...col}
                                                                fixed={col.fixed ?? 'left'}
                                                            >
                                                                {{
                                                                    default: ({ row, column, $index }: { row: any, column: FTableColumn; $index: number }) => (
                                                                        <>
                                                                            <component is={col.render} row={row} column={column} $index={$index} />
                                                                            {slots[col.slot] && slots[col.slot](row, column, $index)}
                                                                        </>
                                                                    )
                                                                }}
                                                            </el-table-column>
                                                        ) : (null)
                                                    }
                                                    {
                                                        !col.prop && col.show == false ? (null) :
                                                            (
                                                                <TableColumn column={col}>
                                                                    {
                                                                        Object.keys(slots).map((slot: string) => (
                                                                            {
                                                                                [slot]: (scope) => (
                                                                                    <>
                                                                                        {slots[slot](scope)}
                                                                                    </>
                                                                                )
                                                                            }
                                                                        ))
                                                                    }
                                                                </TableColumn>
                                                            )
                                                    }
                                                </>
                                            ))
                                        )
                                    }
                                </>
                            )
                        }}
                    </el-table>
                    {
                        slots.pagination ? (
                            slots.pagination()
                        ) : (
                            <>
                                {
                                    props.pagination ? (
                                        <Pagination
                                            pageIndex={state.tablePagination.pageIndex}
                                            pageSize={state.tablePagination.pageSize}
                                            totalRows={state.tablePagination.totalRows}
                                            onHandleSizeChange={handleSizeChange}
                                            onHandleCurrentChange={handleCurrentChange}
                                        />
                                    ) : (
                                        <el-pagination
                                            style="margin-top: 5px"
                                            layout="total"
                                            total={state.tableData.length}
                                        />
                                    )
                                }
                            </>
                        )
                    }
                    {slots.tableFooter && slots.tableFooter()}
                </div>
            </div >
        )
    },
});
