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
          :scroll-x="500"
          @update:checked-row-keys="onRowCheck"
        />
      </template>
      <template #footer>
        <TableFooter :pagination="pagination" :showRefresh="false" />
      </template>
    </TableBody>

    <ModalDialog ref="modalDialog" :submitLoading="submitLoading" @confirm="onConfirm">
      <template #content>
        <DataForm ref="editForm" :options="EditFormOptions" :form-config="formConfig" />
      </template>
    </ModalDialog>
  </div>
</template>

<script lang="ts">
  import { get, post, sendDelete } from '@/api/http'
  import { cookieTypeApi } from '@/api/url'
  import {
    TableActionModel,
    usePagination,
    useRenderAction,
    useRowKey,
    useTable,
    useTableColumn,
    useTableHeight,
  } from '@/hooks/table'
  import { DataFormType, ModalDialogType } from '@/types/components'
  import { DataTableColumn, FormProps, useDialog, useMessage } from 'naive-ui'
  import { defineComponent, h, onMounted, ref } from 'vue'
  import { EditFormOptions, SearchFormOptions } from './Data'
  export default defineComponent({
    name: 'CookieType',
    setup() {
      const formParams = ref<any>({})
      const submitLoading = ref(false)
      const naiveDialog = useDialog()

      //新增或编辑
      const modalDialog = ref<ModalDialogType | null>(null)
      const editForm = ref<DataFormType | null>(null)
      const formConfig = {
        labelWidth: 100,
        size: 'medium',
        labelAlign: 'right',
      } as FormProps

      //表格
      const searchForm = ref<DataFormType | null>(null)
      const pagination = usePagination(doRefresh)
      //pagination.pageSize = 10
      const table = useTable()
      const message = useMessage()
      const rowKey = useRowKey('id')
      const checkedRowKeys = [] as Array<any>
      const tableColumns = useTableColumn(
        [
          //table.selectionColumn,
          table.indexColumn,
          {
            title: '显示文案',
            key: 'showText',
            fixed: 'left',
          },
          {
            title: '网盘标识',
            key: 'businessFlag',
          },
          {
            title: '创建时间',
            key: 'createdTime',
          },
          {
            title: '修改时间',
            key: 'modifyTime',
          },
          {
            title: '操作',
            key: 'actions',
            fixed: 'right',
            render: (rowData) => {
              const normalAction = useRenderAction([
                {
                  label: '编辑',
                  onClick: onUpdateItem.bind(null, rowData),
                },
                {
                  label: '删除',
                  type: 'error',
                  onClick: onDeleteItem.bind(null, rowData),
                },
              ] as TableActionModel[])

              return normalAction
            },
          },
        ],
        {
          align: 'center',
        } as DataTableColumn
      )
      //刷新
      function doRefresh() {
        const params = searchForm.value?.generatorParams()
        if (params) {
          formParams.value = params
        }

        get({
          url: cookieTypeApi.query,
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
      }

      //选中
      function onRowCheck(rowKeys: Array<any>) {
        checkedRowKeys.length = 0
        checkedRowKeys.push(...rowKeys)
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

      //添加
      function onAddItem() {
        modalDialog.value?.show().then(() => {
          editForm.value?.reset()
        })
      }

      //编辑
      function onUpdateItem(item: any) {
        EditFormOptions.forEach((it: any) => {
          it.value.value = item[it.key] || null
        })
        modalDialog.value?.show()
      }

      //删除
      function onDeleteItem(item: any) {
        const pd = {
          ids: [item.id],
        }
        naiveDialog.warning({
          title: '提示',
          content: `是否确认删除：${item.showText} 此条数据？`,
          positiveText: '删除',
          onPositiveClick: () => {
            sendDelete({
              url: cookieTypeApi.delete,
              data: pd,
            })
              .then((res) => {
                if (res.isSuccess) {
                  doRefresh()
                  message.success(res.msg)
                  modalDialog.value?.close()
                } else {
                  message.error(res.msg)
                }
              })
              .catch(console.log)
          },
        })
      }

      function onConfirm() {
        if (editForm.value?.validator()) {
          const pd = editForm.value?.generatorParams()
          submitLoading.value = true
          post({
            url: cookieTypeApi.createAndUpdate,
            data: pd,
          })
            .then((res) => {
              if (res.isSuccess) {
                doRefresh()
                message.success(res.msg)
                modalDialog.value?.close()
              } else {
                message.error(res.msg)
              }
            })
            .finally(() => {
              submitLoading.value = false
            })
        }
      }

      onMounted(async () => {
        table.tableHeight.value = await useTableHeight()
        doRefresh()
      })
      return {
        ...table,
        rowKey,
        submitLoading,
        pagination,
        searchForm,
        tableColumns,
        SearchFormOptions,
        EditFormOptions,
        onSearch,
        onResetSearch,
        onAddItem,
        doRefresh,
        onConfirm,
        modalDialog,
        editForm,
        formConfig,
        onRowCheck,
      }
    },
  })
</script>
