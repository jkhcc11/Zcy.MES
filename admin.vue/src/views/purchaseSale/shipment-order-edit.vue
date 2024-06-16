<template>
  <div class="main-container">
    <n-card :title="editData.title" style="margin-bottom: 16px">
      <n-skeleton v-if="editLoadingRef" text :repeat="10" />
      <DataForm
        ref="submitForm"
        :options="CreateShipmentOrderFormOptions"
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
        :data="orderDetailData"
        :pagination="false"
        :bordered="false"
        :max-height="280"
      />

      <template #action>
        <n-button type="info" size="small" @click="onSave" :loading="editLoadingRef">
          保存
        </n-button>

        <n-gradient-text type="error"> 商品总数：{{ editData.orderProductCount }} </n-gradient-text>
      </template>
    </n-card>

    <ModalDialog
      ref="modalDialogRef"
      @confirm="onDialogConfirm"
      :maskClosable="false"
      :isClosable="true"
      title="选择产品"
      contentHeight="70vh"
      contentWidth="60vh"
    >
      <template #content>
        <CommonTableSelector
          ref="vodTableSelectorRef"
          :getApiUrl="productApi.query"
          :searchFormOptions="ShipmentProductFilterForm"
          :baseTableColumns="editData.tableColumns"
          :tableHeight="300"
        />
      </template>
    </ModalDialog>
  </div>
</template>

<script lang="ts">
  import { ShipmentOrderApi, productApi } from '@/api/url'
  import { NButton, NInput, NInputNumber, useMessage } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref, watch } from 'vue'
  import { RouteRecordRaw, useRouter } from 'vue-router'
  import { post } from '@/api/http'
  import useVisitedRouteStore from '@/store/modules/visited-routes'
  import { FormItem, ModalDialogType } from '@/types/components'
  import { renderSelect, renderInput } from '@/hooks/form'
  import { ClientTypeEnum, recordNameLength } from '@/store/types'
  import {
    ProductFilterTableColumns,
    ShipmentProductFilterForm,
    CreateShipmentOrderDetailInput,
  } from './Data'
  import useClientCacheStore from '@/store/modules/client-cache'
  import useCompanyCacheStore from '@/store/modules/company'
  import { BaseCreateOrderFormOptions } from './BaseData'
  export default defineComponent({
    name: 'ShipmentOrderEdit',
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

      const orderDetailData = reactive<CreateShipmentOrderDetailInput[]>([])
      const editData = reactive<any>({
        detailData: null,
        title: null,
        orderProductCount: null,
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
                  orderDetailData[index].count = newVal
                },
              })
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
                  orderDetailData[index].remark = newVal
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

      //创建出货订单
      const CreateShipmentOrderFormOptions = [
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
          label: '提货人',
          key: 'pickUpUser',
          value: ref(null),
          span: 1,
          render: (formItem: any) =>
            renderInput(formItem.value, {
              placeholder: '提货人信息',
              maxlength: recordNameLength,
              showCount: true,
            }),
        },
        ...BaseCreateOrderFormOptions,
      ] as Array<FormItem>

      //移除产品
      function onRemoveItem(rowData: any) {
        const tempArray = orderDetailData.filter((item) => item.productId !== rowData.productId)
        orderDetailData.length = 0
        orderDetailData.push(...tempArray)
      }

      //加载详情
      function loadDetail() {
        //detailId.value = route.params.id
        editData.title = '新增出货订单'
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
          if (orderDetailData.length <= 0) {
            message.error('请添加产品')
            return
          }

          editLoadingRef.value = true
          post({
            url: ShipmentOrderApi.create,
            data: {
              ...formData,
              orderDetails: orderDetailData,
            },
            method: 'PUT',
          })
            .then((res: any) => {
              message.success(res.msg)

              useVisitedRoute
                .removeVisitedRoute({
                  name: 'ShipmentOrderEdit',
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

      //选择确认
      function onDialogConfirm() {
        var tempArray = vodTableSelectorRef.value?.checkedRows.map((item: any) => {
          return {
            productId: item.id,
            productName: item.productName,
            unit: item.unit,
            remark: '',
            count: 1,
          } as CreateShipmentOrderDetailInput
        })

        tempArray.forEach((newProduct: any) => {
          const exists = orderDetailData.some(
            (product) => product.productId === newProduct.productId
          )
          if (exists) {
            message.warning(`产品 ${newProduct.productName} 已存在，跳过！`)
          } else {
            orderDetailData.push(newProduct)
          }
        })

        modalDialogRef.value?.close()
      }

      // 计算订单信息
      const totalsOrderInfo = () => {
        editData.orderProductCount = orderDetailData.reduce(
          (acc, item) => acc + parseInt(item.count + ''),
          0
        )
      }

      // 监听 orderDetailData 的变化
      watch(
        () => orderDetailData,
        () => {
          totalsOrderInfo()
        },
        { deep: true }
      )

      onMounted(async () => {
        loadDetail()
        await useClientCache.init(ClientTypeEnum.客户)
        await useCompanyCache.init()
      })

      return {
        ShipmentProductFilterForm,
        submitForm,
        productApi,
        ShipmentOrderApi,
        CreateShipmentOrderFormOptions,
        editLoadingRef,
        editData,
        vodTableSelectorRef,
        modalDialogRef,
        orderDetailData,
        onCreateProcesses,
        onSave,
        onDialogConfirm,
      }
    },
  })
</script>
