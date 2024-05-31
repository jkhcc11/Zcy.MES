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
              :options="DaySumSearchFormOptions"
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
    </TableBody>
  </div>
</template>

<script lang="ts">
  import { get } from '@/api/http'
  import { recordHistoryApi } from '@/api/url'
  import { usePagination, useRowKey, useTable, useTableColumn, useTableHeight } from '@/hooks/table'
  import { DataFormType } from '@/types/components'
  import { DataTableColumn } from 'naive-ui'
  import { defineComponent, onMounted, ref } from 'vue'
  import { DaySumSearchFormOptions } from './Data'
  export default defineComponent({
    name: 'DateSumList',
    setup() {
      const formParams = ref<any>({})
      const tableLoading = ref(false)

      //表格
      const searchForm = ref<DataFormType | null>(null)
      const table = useTable()
      const rowKey = useRowKey('date')
      const tableColumns = useTableColumn(
        [
          {
            title: '日期',
            key: 'date',
            render: function (row: any) {
              return new Date(row.date).toISOString().split('T')[0]
            },
          },
          {
            title: '请求量',
            key: 'count',
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
          url: recordHistoryApi.queryDaySum,
          data: () => {
            return {
              ...formParams.value,
            }
          },
        })
          .then((res) => {
            //请求成功处理
            table.handleSuccess(res)
          })
          .catch(console.log)
          .finally(() => {
            tableLoading.value = false
          })
      }

      //查询
      function onSearch() {
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
        searchForm,
        tableColumns,
        DaySumSearchFormOptions,
        onSearch,
        onResetSearch,
        doRefresh,
      }
    },
  })
</script>
