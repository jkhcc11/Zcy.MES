<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="vodApi.danmuQuery"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchDanMuOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    tableAlign="left"
  >
    <template #tableToolbar>
      <n-button-group>
        <n-button type="error" size="small" @click="onBtnClick.batchDel"> 批量删除 </n-button>
      </n-button-group>
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { sendDelete } from '@/api/http'
  import { vodApi, vodMainUrl } from '@/api/url'
  import { TableActionModel, useRenderAction } from '@/hooks/table'
  import { useMessage, useDialog, NButton } from 'naive-ui'
  import { defineComponent, h, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchDanMuOptions } from './Data'
  export default defineComponent({
    name: 'DanMuList',
    setup() {
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)

      //表格
      //const table = useTable()
      const tableColumns = [
        {
          type: 'selection',
        },
        {
          title: '弹幕',
          key: 'msg',
        },
        {
          title: '视频',
          key: 'epInfo',
          render(row: any) {
            return h(
              NButton,
              {
                type: 'info',
                href: `${vodMainUrl}/Movie/Vod/${row.epId}`,
                target: '_blank',
                tag: 'a',
                text: true,
              },
              {
                default: () => {
                  return row.epInfo
                },
              }
            )
          },
        },
        {
          title: '创建时间',
          key: 'createdTime',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '修改时间',
          key: 'modifyTime',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '操作',
          key: 'actions',
          fixed: 'right',
          width: 120,
          render: (rowData: any) => {
            const normalAction = useRenderAction([
              {
                label: '删除',
                type: 'error',
                onClick: onBtnClick.delete.bind(null, rowData),
              },
            ] as TableActionModel[])

            return normalAction
          },
        },
      ]

      const onBtnClick = {
        //删除
        delete: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: `是否删除【${rowData.msg}】?`,
            positiveText: '确认',
            onPositiveClick: () => {
              sendDelete({
                url: vodApi.danmuDel,
                data: {
                  ids: [rowData.id],
                },
              })
                .then((res) => {
                  doRefresh()
                  message.success(res.msg)
                })
                .catch(console.log)
            },
          })
        },
        //批量删除
        batchDel: function () {
          const checkedRowInfo = commonQueryListRef.value?.getCheckedRow()
          if (checkedRowInfo.checkedRowKeys.length == 0) {
            message.warning('请至少选择一行后进行操作')
            return
          }

          naiveDialog.error({
            title: '提示',
            content: `是否要批量删除，数量：${checkedRowInfo.checkedRowKeys.length}?`,
            positiveText: '删除',
            onPositiveClick: () => {
              sendDelete({
                url: vodApi.danmuDel,
                data: {
                  ids: checkedRowInfo.checkedRowKeys,
                },
              })
                .then((res) => {
                  doRefresh()
                  message.success(res.msg)
                })
                .catch(console.log)
            },
          })
        },
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      return {
        vodApi,
        commonQueryListRef,
        tableColumns,
        SearchDanMuOptions,
        submitLoading,
        submitForm,
        onBtnClick,
      }
    },
  })
</script>
