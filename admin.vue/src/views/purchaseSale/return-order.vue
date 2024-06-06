<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="ReturnOrderApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchShipmentOrderOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :submitLoading="submitLoading"
    :editDialogTilte="editTitle"
    editContentHeight="50vh"
  >
    <template #tableToolbar>
      <n-space>
        <n-button type="info" size="small" @click="onBtnClick.create"> 创建 </n-button>
      </n-space>
    </template>

    <template #showDialogContent>
      <n-data-table
        :columns="detailData.orderDetailTableColumns"
        :data="detailData.orderDetailData"
        :pagination="false"
        :bordered="false"
        :max-height="280"
      />
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { get, sendDelete } from '@/api/http'
  import { ReturnOrderApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchShipmentOrderOptions } from './Data'
  import { ClientTypeEnum } from '@/store/types'
  import { useRouter } from 'vue-router'
  import useCompanyCacheStore from '@/store/modules/company'
  import useClientCacheStore from '@/store/modules/client-cache'
  export default defineComponent({
    name: 'ReturnOrderIndex',
    setup() {
      const useCompanyCache = useCompanyCacheStore()
      const useClientCache = useClientCacheStore()
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const router = useRouter()

      const detailData = reactive<any>({
        orderDetailData: [],
        orderDetailTableColumns: [
          {
            title: '产品名',
            key: 'productName',
          },
          {
            title: '数量',
            key: 'count',
          },
          {
            title: '单位',
            key: 'unit',
          },
          {
            title: '产品备注',
            key: 'remark',
          },
        ],
      })

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)
      const editTitle = ref('新增')

      //表格
      const table = useTable()
      const tableColumns = [
        table.indexColumn,
        {
          type: 'expand',
          fixed: 'left',
          renderExpand: (rowData: any) => {
            return h('div', {
              innerHTML: `<strong>备注：</strong>${rowData.orderRemark ?? ''} <br/>`,
            })
          },
        },
        {
          title: '订单号',
          key: 'orderNo',
          width: 220,
          fixed: 'left',
        },
        {
          title: '订单日期',
          key: 'orderDate',
          width: 98,
          fixed: 'left',
        },
        {
          title: '订单退货量',
          key: 'orderProductCount',
        },
        {
          title: '客户名',
          key: 'supplierClientName',
        },
        {
          title: '送货人',
          key: 'shipmentUser',
        },
        {
          title: '经办人',
          key: 'managerUser',
        },
        {
          title: '备注',
          key: 'orderRemark',
          width: 50,
          ellipsis: true,
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
          title: '操作',
          key: 'actions',
          fixed: 'right',
          width: 120,
          render: (rowData: any) => {
            var tempArry: TableActionModel[] = []

            tempArry.push({
              label: '详情',
              type: 'info',
              onClick: onBtnClick.detail.bind(null, rowData),
            } as TableActionModel)

            tempArry.push({
              label: '删除',
              type: 'error',
              onClick: onBtnClick.delete.bind(null, rowData),
            } as TableActionModel)
            return useRenderAction(tempArry)
          },
        },
      ]

      const onBtnClick = {
        //创建
        create: function () {
          router.push('/order/return-order-edit')
        },
        //详情
        detail: function (rowData: any) {
          commonQueryListRef.value?.showDialog()
          submitLoading.value = true
          get({
            url: ReturnOrderApi.detail + '/' + rowData.id,
          })
            .then((res) => {
              editTitle.value = `【${res.data?.orderNo ?? ''} 】订单详情`
              detailData.orderDetailData.length = 0
              detailData.orderDetailData.push(...res.data.orderDetails)
            })
            .finally(() => {
              submitLoading.value = false
            })
        },
        //删除
        delete: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: '是否要删除此数据？',
            positiveText: '删除',
            onPositiveClick: () => {
              sendDelete({
                url: ReturnOrderApi.delete + '/' + rowData.id,
                data: {},
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

      onMounted(async () => {
        await useCompanyCache.init()
        await useClientCache.init(ClientTypeEnum.客户)
      })

      return {
        ReturnOrderApi,
        commonQueryListRef,
        tableColumns,
        SearchShipmentOrderOptions,
        submitLoading,
        submitForm,
        detailData,
        editTitle,
        onBtnClick,
      }
    },
  })
</script>
