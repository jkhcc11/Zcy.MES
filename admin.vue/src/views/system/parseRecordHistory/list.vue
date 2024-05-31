<template>
  <div class="main-container">
    <TableBody ref="tableBody">
      <template #header>
        <TableHeader :show-filter="true" @search="onSearch" @reset-search="onResetSearch">
          <template #search-content>
            <DataForm
              ref="searchForm"
              :form-config="{
                labelWidth: 60,
              }"
              :options="SearchFormOptions"
              preset="grid-item"
            />
          </template>

          <template #top-toolbar>
            <AddButton @add="onAddItem" />
          </template>
        </TableHeader>
      </template>
      <template #default>
        <n-data-table
          size="small"
          :loading="tableLoading"
          :data="dataList"
          :columns="tableColumns"
          :row-key="rowKey"
          :style="{ height: `${tableHeight}px` }"
          :flex-height="true"
        />
      </template>
      <template #footer>
        <TableFooter :pagination="pagination" :showRefresh="false" />
      </template>
    </TableBody>
  </div>
</template>

<script lang="ts">
  import { get } from '@/api/http'
  import { recordHistoryApi } from '@/api/url'
  import { renderTagByEnum } from '@/hooks/form'
  import { usePagination, useRowKey, useTable, useTableColumn, useTableHeight } from '@/hooks/table'
  import { DataFormType } from '@/types/components'
  import { DataTableColumn } from 'naive-ui'
  import { defineComponent, onMounted, ref } from 'vue'
  import { SearchFormOptions } from './Data'
  import { RecordHistoryTypeEnum } from '@/store/types'
  export default defineComponent({
    name: 'RecordHistory',
    setup() {
      const formParams = ref<any>({})
      const tableLoading = ref(false)

      //表格
      const searchForm = ref<DataFormType | null>(null)
      const pagination = usePagination(doRefresh)
      const table = useTable()
      const rowKey = useRowKey('id')
      const tableColumns = useTableColumn(
        [
          {
            title: '别名',
            key: 'alias',
          },
          {
            title: '文件信息',
            key: 'fileIdOrFileName',
          },
          {
            title: '记录类型',
            key: 'recordHistoryType',
            render: (rowData) =>
              renderTagByEnum(rowData.recordHistoryType, RecordHistoryTypeEnum, {
                1: 'success',
              }),
          },
          {
            title: '访问时间',
            key: 'createdTime',
          },
        ],
        {
          align: 'center',
        } as DataTableColumn
      )
      //刷新
      function doRefresh() {
        tableLoading.value = true
        const params = searchForm.value?.generatorParams()
        if (params) {
          formParams.value = params
        }

        get({
          url: recordHistoryApi.query,
          data: () => {
            return {
              ...formParams.value,
              page: pagination.page,
              pageSize: pagination.pageSize,
            }
          },
        })
          .then((res) => {
            //请求成功处理
            table.handleSuccess(res.data)
            pagination.setTotalSize(res.data.dataCount)
          })
          .catch(console.log)
          .finally(() => {
            tableLoading.value = false
          })
      }

      //查询
      function onSearch() {
        pagination.page = 1
        doRefresh()
      }
      //重置查询
      function onResetSearch() {
        searchForm.value?.reset()
      }

      onMounted(async () => {
        table.tableHeight.value = await useTableHeight()
        doRefresh()
      })
      return {
        ...table,
        rowKey,
        tableLoading,
        pagination,
        searchForm,
        tableColumns,
        SearchFormOptions,
        onSearch,
        onResetSearch,
        doRefresh,
      }
    },
  })
</script>
