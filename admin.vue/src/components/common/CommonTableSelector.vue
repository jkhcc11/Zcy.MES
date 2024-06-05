<template>
  <TableBody ref="tableBody">
    <template #header>
      <TableHeader
        :show-filter="true"
        title="查询条件"
        @search="onSearch"
        @reset-search="onResetSearch"
      >
        <template #search-content>
          <DataForm
            ref="searchForm"
            :form-config="{
              labelWidth: 60,
              size: 'small',
            }"
            :options="searchFormOptions"
            preset="grid-item"
          />
        </template>
      </TableHeader>
    </template>

    <template #default>
      <n-data-table
        size="small"
        :loading="tableLoading"
        :data="dataList"
        :columns="tableColumns"
        :row-key="tableRowKey"
        :style="{ height: `${tableHeight}px` }"
        :flex-height="true"
        :checked-row-keys="checkedRowKeys"
        @update:checked-row-keys="onRowCheck"
      />
    </template>
    <template #footer>
      <TableFooter :pagination="pagination" :showRefresh="false" />
    </template>
  </TableBody>
</template>

<script lang="ts">
  import { defineComponent, ref, h, onMounted, toRef, watch } from 'vue'
  import { DataTableColumn } from 'naive-ui'
  import { usePagination, useRowKey, useTable, useTableColumn, useTableHeight } from '@/hooks/table'
  import { DataFormType, FormItem } from '@/types/components'
  import { get } from '@/api/http'
  import { removeNullProperties } from '@/utils'
  export default defineComponent({
    //通用表格选择组件
    name: 'CommonTableSelector',
    props: {
      /**
       * 请求Api地址
       */
      getApiUrl: String,
      /**
       * 搜索配置
       */
      searchFormOptions: Array<FormItem>,
      /**
       * 表格列
       */
      baseTableColumns: Array<DataTableColumn>,
      /**
       * 表格列唯一标识 默认为id
       */
      rowKey: {
        type: String,
        defalut: 'id',
      },
      defaultFilter: Object,
      tableHeight: Number,
    },
    emits: ['selected'],
    setup(props, { emit }) {
      //把传入的变量解析出来
      const tableLoading = ref(false)
      const rowKeyRef = toRef(props, 'rowKey')
      const defaultFilterRef = toRef(props, 'defaultFilter')
      const baseTableColumnsRef = toRef(props, 'baseTableColumns')
      const apiUrlRef = toRef(props, 'getApiUrl')
      const postData = ref<any>({})
      if (!apiUrlRef.value) {
        throw new Error('请检查组件getApiUrl是否设置')
      }

      const searchForm = ref<DataFormType | null>(null)

      const pagination = usePagination(doRefresh)
      pagination.pageSize = 5
      const table = useTable()
      const tableRowKey = useRowKey(rowKeyRef.value ?? 'id')
      //const defaultRowKeys = ref<Array<string | number>>([])
      const checkedRowKeys = ref<Array<string | number>>([])
      const checkedRows = ref<object[]>([])
      const tableColumns = useTableColumn(baseTableColumnsRef.value ?? [], {
        align: 'center',
      } as DataTableColumn)

      //刷新
      function doRefresh() {
        const params = searchForm.value?.generatorParams()
        if (params) {
          Object.assign(postData.value, removeNullProperties(params))
        }

        if (defaultFilterRef.value) {
          Object.assign(postData.value, removeNullProperties(defaultFilterRef.value))
        }

        tableLoading.value = true
        //formParams.value = params
        get({
          url: apiUrlRef.value ?? '',
          data: () => {
            return {
              ...postData.value,
              page: pagination.page,
              pageSize: pagination.pageSize,
            }
          },
        })
          .then((res) => {
            //请求成功处理
            table.handleSuccess(res.data)
            pagination.setTotalSize(res.data.total)
            //console.log(res.result.totalCount, res.result)
          })
          .catch(console.log)
          .finally(() => {
            tableLoading.value = false
          })
      }

      //选中
      function onRowCheck(rowKeys: Array<any>, rows: object[]) {
        //checkedRowKeys.length = 0
        checkedRowKeys.value = rowKeys

        checkedRows.value = rows
      }

      //搜索
      function onSearch() {
        pagination.page = 1
        doRefresh()
      }

      //重置查询
      function onResetSearch() {
        pagination.page = 1
        searchForm.value?.reset()
        doRefresh()
      }

      //清除选中
      function clearSelected() {
        checkedRows.value = []
        checkedRowKeys.value = []
      }

      watch(
        () => props.defaultFilter,
        (newValue) => {
          //默认值变更时查询
          if (JSON.stringify(postData.value) !== JSON.stringify(newValue)) {
            postData.value = newValue
            onSearch()
          }
        }
      )

      onMounted(async () => {
        if (!props.tableHeight) {
          //没有高度自动计算
          table.tableHeight.value = await useTableHeight()
        } else {
          table.tableHeight.value = props.tableHeight
        }

        doRefresh()
      })
      return {
        ...table,
        tableRowKey,
        tableColumns,
        pagination,
        searchForm,
        checkedRowKeys,
        checkedRows,
        tableLoading,
        onRowCheck,
        onSearch,
        onResetSearch,
        clearSelected,
      }
    },
  })
</script>
