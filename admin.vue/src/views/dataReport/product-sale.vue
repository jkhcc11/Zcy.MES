<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="DataReportApi.queryProductSale"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchProductSaleOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    @load-data-success="onBtnClick.onLoadDataSuccess"
  >
    <template #tableToolbar>
      <n-space>
        <n-tag :bordered="false" type="info">
          总采购价： {{ totalsRef.sumPurchasePrice ?? '-' }}</n-tag
        >
        <n-tag :bordered="false" type="success">
          总销售价： {{ totalsRef.sumSalePrice ?? '-' }}</n-tag
        >
        <n-tag :bordered="false" type="warning">
          总盈亏： {{ totalsRef.sumProfitPrice ?? '-' }}</n-tag
        >

        <n-gradient-text type="warning">
          注：默认数据为近30天数据，更多数据请点击搜索
        </n-gradient-text>
      </n-space>
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { DataReportApi } from '@/api/url'
  import { useTable } from '@/hooks/table'
  import { defineComponent, h, reactive, ref } from 'vue'
  import { SearchProductSaleOptions } from './Data'
  import { NTag } from 'naive-ui'
  export default defineComponent({
    name: 'ProductSaleReport',
    setup() {
      const commonQueryListRef = ref()
      const totalsRef = reactive({
        sumPurchasePrice: null,
        sumSalePrice: null,
        sumProfitPrice: null,
      })

      //表格
      const table = useTable()
      const tableColumns = [
        table.indexColumn,
        {
          title: '产品名',
          key: 'productName',
          fixed: 'left',
        },
        {
          title: '采购数量',
          key: 'purchaseCount',
        },
        {
          title: '采购均价',
          key: 'avgPurchaseUnitPrice',
        },
        {
          title: '采购总价',
          key: 'sumPurchasePrice',
        },

        {
          title: '销售数量',
          key: 'saleCount',
        },
        {
          title: '销售均价',
          key: 'avgSaleUnitPrice',
        },
        {
          title: '销售总价',
          key: 'sumSalePrice',
        },

        {
          title: '库存',
          key: 'stockCount',
          render: (rowData: any) => {
            let type = 'default'
            if (rowData.stockCount && rowData.stockCount <= 0) {
              type = 'warning'
            } else if (rowData.stockCount && rowData.stockCount > 0) {
              type = 'success'
            }

            return h(
              NTag,
              {
                type: type,
                size: 'small',
              },
              {
                default: () => rowData.stockCount ?? '-',
              }
            )
          },
        },
        {
          title: '盈亏',
          key: 'profitPrice',
          render: (rowData: any) => {
            let type = 'default'
            if (rowData.profitPrice && rowData.profitPrice <= 0) {
              type = 'warning'
            } else if (rowData.profitPrice && rowData.profitPrice > 0) {
              type = 'success'
            }

            return h(
              NTag,
              {
                type: type,
                size: 'small',
              },
              {
                default: () => rowData.profitPrice ?? '-',
              }
            )
          },
        },
      ]

      const onBtnClick = {
        //加载完成
        onLoadDataSuccess: function (data: any) {
          totalsRef.sumPurchasePrice = data.reduce((total: any, item: any) => {
            return total + (item.sumPurchasePrice || 0)
          }, 0)

          totalsRef.sumSalePrice = data.reduce((total: any, item: any) => {
            return total + (item.sumSalePrice || 0)
          }, 0)

          totalsRef.sumProfitPrice = data.reduce((total: any, item: any) => {
            return total + (item.sumProfitPrice || 0)
          }, 0)
        },
      }
      // onMounted(async () => {
      //   await useCompanyCache.init()
      //   await useClientCache.init(ClientTypeEnum.客户)
      // })

      return {
        DataReportApi,
        commonQueryListRef,
        tableColumns,
        SearchProductSaleOptions,
        totalsRef,
        onBtnClick,
      }
    },
  })
</script>
