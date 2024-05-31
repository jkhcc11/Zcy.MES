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

          <!-- <template #top-toolbar>
            <AddButton @add="onAddItem" />
          </template> -->
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
          :scroll-x="800"
          @update:checked-row-keys="onRowCheck"
        />
      </template>
      <template #footer>
        <TableFooter :pagination="pagination" :showRefresh="false" />
      </template>
    </TableBody>
  </div>
</template>

<script lang="ts">
  import { get, post } from '@/api/http'
  import { renderTagByEnum } from '@/hooks/form'
  import { baseUserInfoApi } from '@/api/url'
  import {
    TableActionModel,
    usePagination,
    useRenderAction,
    useRowKey,
    useTable,
    useTableColumn,
    useTableHeight,
  } from '@/hooks/table'
  import { DataFormType } from '@/types/components'
  import { DataTableColumn, useDialog, useMessage, NInput } from 'naive-ui'
  import { defineComponent, h, onMounted, ref } from 'vue'
  import { ServerCookieStatusEnum } from '@/store/types'
  import { SearchFormOptions } from './Data'
  import useUserParseConfigStore from '@/store/modules/user-parse-config'
  export default defineComponent({
    name: 'ParseUser',
    setup() {
      const formParams = ref<any>({})
      const submitLoading = ref(false)
      const naiveDialog = useDialog()
      const useUserParseConfig = useUserParseConfigStore()

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
          table.indexColumn,
          {
            title: '用户Id',
            fixed: 'left',
            key: 'userId',
          },
          {
            title: 'Api地址',
            key: 'selfApiUrl',
          },
          {
            title: '是否防盗',
            key: 'isHoldLink',
            render: (rowData) => {
              return rowData.isHoldLink ? '是' : '否'
            },
          },
          {
            title: '防盗域名',
            key: 'holdLinkHost',
            render: (rowData: any) => {
              return rowData.holdLinkHost.join(',')
            },
            width: 100,
            ellipsis: {
              tooltip: true,
            },
          },
          {
            title: '状态',
            key: 'userStatus',
            render: (rowData) =>
              renderTagByEnum(rowData.userStatus, ServerCookieStatusEnum, {
                5: 'success',
              }),
          },
          {
            title: '注册时间',
            key: 'createdTime',
          },
          {
            title: '过期时间',
            key: 'expirationDateTime',
          },
          {
            title: '备注',
            key: 'remark',
            render(row, index) {
              return h(NInput, {
                value: row.remark,
                placeholder: '用户备注 回车确认',
                onUpdateValue(v: any) {
                  row.remark = v
                },
                onKeyup(e: any) {
                  if (e.key === 'Enter') {
                    post({
                      url: baseUserInfoApi.updateUserRemark,
                      data: {
                        userId: row.userId,
                        remark: row.remark,
                      },
                    })
                      .then((res) => {
                        message.success(res.msg)
                      })
                      .catch(console.log)
                  }
                },
              })
            },
          },
          {
            title: '操作',
            key: 'actions',
            fixed: 'right',
            render: (rowData) => {
              const actionArray = [
                {
                  label: '延期',
                  type: 'info',
                  onClick: onDelayDateItem.bind(null, rowData),
                },
                {
                  label: '删除',
                  type: 'error',
                  onClick: onDeleteItem.bind(null, rowData),
                },
              ] as TableActionModel[]
              if (rowData.userStatus == ServerCookieStatusEnum.初始化) {
                actionArray.push({
                  label: '审批',
                  type: 'warning',
                  onClick: onAuditItem.bind(null, rowData),
                } as TableActionModel)
              }

              const normalAction = useRenderAction(actionArray)
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
          url: baseUserInfoApi.query,
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

      //审批
      function onAuditItem(item: any) {
        naiveDialog.warning({
          title: '提示',
          content: `是否确认审批，别名：${item.remark} 此条数据？`,
          positiveText: '确认',
          onPositiveClick: () => {
            post({
              url: baseUserInfoApi.audit + '/' + item.userId,
            })
              .then((res) => {
                doRefresh()
                message.success(res.msg)
              })
              .catch(console.log)
          },
        })
      }

      //延期
      function onDelayDateItem(item: any) {
        naiveDialog.warning({
          title: '提示',
          content: `是否确认延期一个月，别名：${item.remark} 此条数据？`,
          positiveText: '确认',
          onPositiveClick: () => {
            post({
              url: baseUserInfoApi.delayDate + '/' + item.id,
            })
              .then((res) => {
                doRefresh()
                message.success(res.msg)
                useUserParseConfig.reset()
              })
              .catch(console.log)
          },
        })
      }

      //删除
      function onDeleteItem(item: any) {
        message.warning('待实现')
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
        onSearch,
        onResetSearch,
        doRefresh,
        onRowCheck,
      }
    },
  })
</script>
