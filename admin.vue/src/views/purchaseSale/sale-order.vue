<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="saleOrderApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchSaleOrderOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :submitLoading="submitLoading"
    :editDialogTilte="editTitle"
    @load-data-success="onBtnClick.onLoadDataSuccess"
    editContentHeight="50vh"
  >
    <template #tableToolbar>
      <n-space>
        <n-button type="info" size="small" @click="onBtnClick.create"> 创建 </n-button>

        <n-tag :bordered="false" type="success">
          总订单价： {{ totalsRef.sumOrderPrice ?? '-' }}</n-tag
        >
        <n-tag :bordered="false" type="info">
          总运费： {{ totalsRef.sumFreightPrice ?? '-' }}</n-tag
        >
        <n-tag :bordered="false" type="warning">
          总商品价： {{ totalsRef.sumSalePrice ?? '-' }}</n-tag
        >

        <n-gradient-text type="warning">
          注：默认数据为近30天数据，更多数据请点击搜索
        </n-gradient-text>
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
  import { saleOrderApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchSaleOrderOptions } from './Data'
  import { renderTagByEnum } from '@/hooks/form'
  import { AccountTypeEnum, SaleOrderStatusEnum, ClientTypeEnum } from '@/store/types'
  import { useRouter } from 'vue-router'
  import useCompanyCacheStore from '@/store/modules/company'
  import useClientCacheStore from '@/store/modules/client-cache'
  import { OrderDetailTableColumns } from './BaseData'
  export default defineComponent({
    name: 'SaleOrderIndex',
    setup() {
      const useCompanyCache = useCompanyCacheStore()
      const useClientCache = useClientCacheStore()
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const router = useRouter()

      const detailData = reactive<any>({
        orderDetailData: [],
        orderDetailTableColumns: OrderDetailTableColumns,
      })
      const totalsRef = reactive({
        sumFreightPrice: null,
        sumSalePrice: null,
        sumOrderPrice: null,
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
              innerHTML: `<strong>备注：</strong>${
                rowData.orderRemark ?? ''
              } <br/> <strong>摘要 ：</strong>${rowData.orderSummary ?? ''}`,
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
          title: '账户类型',
          key: 'accountType',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.accountType,
              AccountTypeEnum,
              {
                1: 'info',
                2: 'info',
                3: 'info',
                4: 'info',
                100: 'tertiary',
              },
              {
                size: 'small',
              }
            ),
        },
        {
          title: '客户名',
          key: 'supplierClientName',
        },
        {
          title: '订单状态',
          key: 'saleOrderStatus',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.saleOrderStatus,
              SaleOrderStatusEnum,
              {
                100: 'success',
                5: 'warning',
              },
              {
                size: 'small',
              }
            ),
        },
        {
          title: '运费',
          key: 'freightPrice',
        },
        {
          title: '销售总价',
          key: 'sumSalePrice',
        },
        {
          title: '订单价格',
          key: 'orderPrice',
        },
        {
          title: '经办人',
          key: 'managerUser',
        },
        {
          title: '数量',
          key: 'orderProductCount',
        },
        {
          title: '摘要',
          key: 'orderSummary',
          width: 50,
          ellipsis: true,
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
          router.push('/order/sale-order-edit')
        },
        //详情
        detail: function (rowData: any) {
          commonQueryListRef.value?.showDialog()
          submitLoading.value = true
          get({
            url: saleOrderApi.detail + '/' + rowData.id,
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
                url: saleOrderApi.delete + '/' + rowData.id,
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
        //加载完成
        onLoadDataSuccess: function () {
          const params = commonQueryListRef.value?.getQueryParams()
          get({
            url: saleOrderApi.getTotals,
            data: params,
          })
            .then((res) => {
              totalsRef.sumFreightPrice = res.data.sumFreightPrice
              totalsRef.sumSalePrice = res.data.sumSalePrice
              totalsRef.sumOrderPrice = res.data.sumOrderPrice
            })
            .finally(() => {})
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
        saleOrderApi,
        commonQueryListRef,
        tableColumns,
        SearchSaleOrderOptions,
        submitLoading,
        submitForm,
        detailData,
        editTitle,
        totalsRef,
        onBtnClick,
      }
    },
  })
</script>
