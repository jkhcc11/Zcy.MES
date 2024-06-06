<template>
  <div class="main-container">
    <n-card :title="editData.title" style="margin-bottom: 16px">
      <n-skeleton v-if="editLoadingRef" text :repeat="10" />
      <DataForm
        ref="submitForm"
        :options="CreateSaleOrderFormOptions"
        :form-config="{
          labelWidth: 100,
          size: 'medium',
          labelAlign: 'right',
        }"
        preset="grid-item"
      />

      <n-button type="success" size="small" @click="onCreateProcesses"> 添加产品 </n-button>
      <n-data-table
        :columns="editData.detailTableColumns"
        :data="saleOrderDetailData"
        :pagination="false"
        :bordered="false"
        :max-height="280"
      />

      <template #action>
        <n-button type="info" size="small" @click="onSave" :loading="editLoadingRef">
          保存
        </n-button>

        <n-gradient-text type="error">
          商品数量：{{ saleOrderDetailData.length }} 订单金额：{{ editData.orderPrice ?? '-' }}
        </n-gradient-text>
      </template>
    </n-card>

    <ModalDialog
      ref="modalDialogRef"
      @confirm="onDialogConfirm"
      :maskClosable="false"
      :isClosable="true"
      title="选择销售产品"
      contentHeight="70vh"
      contentWidth="60vh"
    >
      <template #content>
        <CommonTableSelector
          ref="vodTableSelectorRef"
          :getApiUrl="productApi.query"
          :searchFormOptions="ProductFilterForm"
          :baseTableColumns="editData.tableColumns"
          :tableHeight="300"
        />
      </template>
    </ModalDialog>
  </div>
</template>

<script lang="ts">
  import { saleOrderApi, productApi } from '@/api/url'
  import { NButton, NInput, NInputNumber, useMessage } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref, watch } from 'vue'
  import { RouteRecordRaw, useRouter } from 'vue-router'
  import { post } from '@/api/http'
  import useVisitedRouteStore from '@/store/modules/visited-routes'
  import { FormItem, ModalDialogType } from '@/types/components'
  import { renderNumberInput, renderSelect } from '@/hooks/form'
  import { AccountTypeEnum, ClientTypeEnum, SaleOrderStatusEnum } from '@/store/types'
  import { CreateSaleOrderDetailInput, ProductFilterTableColumns, ProductFilterForm } from './Data'
  import useClientCacheStore from '@/store/modules/client-cache'
  import useCompanyCacheStore from '@/store/modules/company'
  import { getListByEnum } from '@/utils'
  import { BaseCreateOrderFormOptions } from './BaseData'
  export default defineComponent({
    name: 'SaleOrderEdit',
    setup() {
      const useClientCache = useClientCacheStore()
      const useCompanyCache = useCompanyCacheStore()
      const message = useMessage()
      const useVisitedRoute = useVisitedRouteStore()
      const router = useRouter()

      const modalDialogRef = ref<ModalDialogType | null>(null)
      const vodTableSelectorRef = ref<any>(null)
      const submitForm = ref<any>(null)
      const editLoadingRef = ref(false)

      const saleOrderDetailData = reactive<CreateSaleOrderDetailInput[]>([])
      const editData = reactive<any>({
        detailData: null,
        title: null,
        orderPrice: null,
        freightPrice: null,
        detailTableColumns: [
          {
            title: '产品名',
            key: 'productName',
          },
          {
            title: '单位',
            key: 'unit',
          },
          {
            title: '数量',
            key: 'count',
            render(row: any, index: number) {
              return h(NInputNumber, {
                value: row.count,
                min: 1,
                max: 9999,
                precision: 0,
                placeholder: '数量',
                onUpdateValue(newVal: any) {
                  saleOrderDetailData[index].count = newVal
                },
              })
            },
          },
          {
            title: '单价（元）',
            key: 'unitPrice',
            render(row: any, index: number) {
              return h(NInputNumber, {
                value: row.unitPrice,
                min: 0.01,
                max: 9999,
                precision: 2,
                placeholder: '产品单价',
                onUpdateValue(newVal: any) {
                  saleOrderDetailData[index].unitPrice = newVal
                },
              })
            },
          },
          {
            title: '总价（单价*数量）',
            key: 'sumPrice',
            render: (rowData: any) => {
              rowData.sumPrice = (rowData.unitPrice * rowData.count).toFixed(2)
              return rowData.sumPrice
            },
          },
          {
            title: '产品备注',
            key: 'remark',
            render(row: any, index: number) {
              return h(NInput, {
                value: row.remark,
                placeholder: '产品备注',
                onUpdateValue(newVal: any) {
                  saleOrderDetailData[index].remark = newVal
                },
              })
            },
          },
          {
            title: '操作',
            key: 'actions',
            render(rowData: any) {
              return h(
                NButton,
                {
                  type: 'error',
                  size: 'small',
                  onClick: () => onRemoveItem(rowData),
                },
                { default: () => '移除' }
              )
            },
          },
        ],
        tableColumns: ProductFilterTableColumns,
      })

      //创建销售订单
      const CreateSaleOrderFormOptions = [
        {
          key: 'id',
          value: ref(null),
          hidden: true,
        },
        {
          label: '客户',
          key: 'supplierId',
          value: ref(null),
          required: true,
          render: (formItem) => {
            const useClientCache = useClientCacheStore()
            return renderSelect(formItem.value, useClientCache.cachedItems, {
              placeholder: '客户',
              filterable: true,
            })
          },
        },
        {
          label: '收款账户',
          key: 'accountType',
          value: ref(null),
          required: true,
          render: (formItem) =>
            renderSelect(formItem.value, getListByEnum(AccountTypeEnum), {
              placeholder: '收款账户',
            }),
        },
        {
          label: '收款状态',
          key: 'saleOrderStatus',
          value: ref(SaleOrderStatusEnum.已完成),
          required: true,
          reset(formItem) {
            formItem.value.value = SaleOrderStatusEnum.已完成
          },
          render: (formItem) =>
            renderSelect(formItem.value, getListByEnum(SaleOrderStatusEnum), {
              placeholder: '收款状态',
            }),
        },
        {
          label: '运费价格',
          key: 'freightPrice',
          value: ref(null),
          span: 1,
          render: (formItem: any) =>
            renderNumberInput(formItem.value, {
              placeholder: '运费价格',
              min: 0,
              onChange: function (newVal: any) {
                editData.freightPrice = newVal
              },
            }),
        },
        ...BaseCreateOrderFormOptions,
      ] as Array<FormItem>

      //移除产品
      function onRemoveItem(rowData: any) {
        saleOrderDetailData.length = 0
        saleOrderDetailData.push(
          ...saleOrderDetailData.filter((item) => item.productId !== rowData.productId)
        )
      }

      //加载详情
      function loadDetail() {
        //detailId.value = route.params.id
        editData.title = '新增销售订单'
      }

      //新增工序
      function onCreateProcesses() {
        vodTableSelectorRef.value?.clearSelected()
        modalDialogRef.value?.show()
      }

      //保存
      function onSave() {
        if (submitForm.value?.validator()) {
          const formData = submitForm.value?.generatorParams()
          if (saleOrderDetailData.length <= 0) {
            message.error('请添加产品')
            return
          }

          editLoadingRef.value = true
          post({
            url: saleOrderApi.create,
            data: {
              ...formData,
              orderDetails: saleOrderDetailData,
            },
            method: 'PUT',
          })
            .then((res: any) => {
              message.success(res.msg)

              useVisitedRoute
                .removeVisitedRoute({
                  name: 'SaleOrderEdit',
                } as RouteRecordRaw)
                .then((lastPath) => {
                  router.push(lastPath)
                })
            })
            .finally(() => {
              editLoadingRef.value = false
            })
        }
      }

      //选择工艺确认
      function onDialogConfirm() {
        var tempArray = vodTableSelectorRef.value?.checkedRows.map((item: any) => {
          return {
            productId: item.id,
            productName: item.productName,
            unit: item.unit,
            remark: '',
            unitPrice: 0.01,
            count: 1,
            sumPrice: 0,
          } as CreateSaleOrderDetailInput
        })

        tempArray.forEach((newProduct: any) => {
          const exists = saleOrderDetailData.some(
            (product) => product.productId === newProduct.productId
          )
          if (exists) {
            message.warning(`产品 ${newProduct.productName} 已存在，跳过！`)
          } else {
            saleOrderDetailData.push(newProduct)
          }
        })

        //saleOrderDetailData.push(...tempArray)
        // editData.orderPrice =
        //   (editData.freightPrice ?? 0) +
        //   saleOrderDetailData.reduce((acc, item) => acc + item.sumPrice, 0)

        modalDialogRef.value?.close()
        //console.log('data', vodTableSelectorRef.value?.checkedRows)
      }

      // 计算总价格
      const calculateSumPrice = () => {
        const tempPrice =
          (editData.freightPrice ?? 0) +
          saleOrderDetailData.reduce((acc, item) => acc + parseFloat(item.sumPrice + ''), 0)
        editData.orderPrice = tempPrice.toFixed(2)
      }

      // 监听 saleOrderDetailData 的变化
      watch(
        () => saleOrderDetailData,
        () => {
          calculateSumPrice()
        },
        { deep: true }
      )

      watch(
        () => editData.freightPrice,
        () => {
          calculateSumPrice()
        }
      )

      onMounted(async () => {
        loadDetail()
        await useClientCache.init(ClientTypeEnum.客户)
        await useCompanyCache.init()
      })

      return {
        ProductFilterForm,
        submitForm,
        productApi,
        saleOrderApi,
        CreateSaleOrderFormOptions,
        editLoadingRef,
        editData,
        vodTableSelectorRef,
        modalDialogRef,
        saleOrderDetailData,
        onCreateProcesses,
        onSave,
        onDialogConfirm,
      }
    },
  })
</script>
