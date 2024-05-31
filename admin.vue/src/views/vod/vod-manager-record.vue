<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="vodApi.vodManagerRecordQuery"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchManagerRecordOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    @load-data-success="onLoadDataSuccess"
  >
    <template #tableToolbar>
      <n-button type="info" size="small" @click="onBtnClick.checkout"> 结算 </n-button>
    </template>

    <template #topSummary>
      <n-tag> 结算:{{ sumSummary.checkoutAmount.toFixed(2) }} </n-tag>
      <n-tag type="success"> 实际:{{ sumSummary.actualAmount.toFixed(2) }} </n-tag>
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { post } from '@/api/http'
  import { vodApi } from '@/api/url'
  import { useTable } from '@/hooks/table'
  import { useMessage, useDialog, NInputNumber } from 'naive-ui'
  import { defineComponent, h, reactive, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchManagerRecordOptions } from './Data'
  import { renderTag } from '@/hooks/form'
  export default defineComponent({
    name: 'VodManagerRecord',
    setup() {
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const sumSummary = reactive({
        checkoutAmount: 0.0,
        actualAmount: 0.0,
      })

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)

      //表格
      const table = useTable()
      const tableColumns = [
        {
          type: 'selection',
          disabled(row: any) {
            return row.isCheckout
          },
          fixed: 'left',
        },
        table.indexColumn,
        {
          title: '结算积分',
          key: 'checkoutAmount',
          fixed: 'left',
        },
        {
          title: '实际积分',
          key: 'actualAmount',
          render(rowData: any) {
            if (rowData.isCheckout) {
              return rowData.actualAmount
            }

            if (!rowData.actualAmount) {
              //初始化值
              rowData.actualAmount = rowData.checkoutAmount
            }

            return h(NInputNumber, {
              value: rowData.actualAmount,
              placeholder: '实际积分 回车',
              min: 0.1,
              max: 99,
              precision: 2,
              onUpdateValue(v: any) {
                rowData.actualAmount = v
              },
              onKeyup(e: any) {
                if (e.key === 'Enter') {
                  post({
                    url: vodApi.updateActualAmount,
                    data: {
                      keyId: rowData.id,
                      actualAmount: rowData.actualAmount,
                    },
                  })
                    .then((res) => {
                      message.success(res.msg)
                      doRefresh()
                    })
                    .catch(console.log)
                }
              },
            })
          },
        },
        {
          title: '记录类型',
          key: 'recordTypeStr',
        },
        {
          title: '是否结算',
          key: 'isCheckout',
          render(rowData: any) {
            return renderTag(rowData.isCheckout ? '是' : '否', {
              type: rowData.isCheckout ? 'success' : 'default',
            })
          },
        },
        {
          title: '是否有效',
          key: 'isValid',
          render(rowData: any) {
            return renderTag(rowData.isValid ? '是' : '否', {
              type: rowData.isValid ? 'success' : 'default',
            })
          },
        },
        {
          title: '备注',
          key: 'remark',
          width: 100,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '创建人',
          key: 'createdUserName',
          fixed: 'left',
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
      ]

      const onBtnClick = {
        //结算
        checkout: function () {
          const checkedRowInfo = commonQueryListRef.value?.getCheckedRow()
          if (checkedRowInfo.checkedRowKeys.length == 0) {
            message.warning('请至少选择一行后进行操作')
            return
          }

          naiveDialog.warning({
            title: '提示',
            content: `是否批量提交，数量为：【${checkedRowInfo.checkedRowKeys.length}】？`,
            positiveText: '确认',
            onPositiveClick: () => {
              post({
                url: vodApi.checkout,
                data: {
                  ids: checkedRowInfo.checkedRowKeys,
                },
              }).then((res) => {
                doRefresh()
                message.success(res.msg)
              })
            },
          })
        },
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      //数据加载成功后
      function onLoadDataSuccess(pageData: any[]) {
        sumSummary.actualAmount = pageData.reduce(
          (prevValue, row) => prevValue + (row.actualAmount ?? 0),
          0
        )
        sumSummary.checkoutAmount = pageData.reduce(
          (prevValue, row) => prevValue + (row.checkoutAmount ?? 0),
          0
        )
      }

      return {
        vodApi,
        commonQueryListRef,
        tableColumns,
        SearchManagerRecordOptions,
        submitLoading,
        submitForm,
        sumSummary,
        onBtnClick,
        onLoadDataSuccess,
      }
    },
  })
</script>
