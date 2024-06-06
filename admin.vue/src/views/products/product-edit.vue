<template>
  <div class="main-container">
    <n-card :title="editData.title" style="margin-bottom: 16px">
      <n-skeleton v-if="editLoadingRef" text :repeat="10" />
      <DataForm
        ref="submitForm"
        :options="CreateProductFormOptions"
        :form-config="{
          labelWidth: 100,
          size: 'medium',
          labelAlign: 'right',
        }"
        preset="grid-item"
      />

      <div v-if="editData.isProcesses">
        <n-divider title-placement="left"> 产品工序 </n-divider>

        <n-button type="success" size="small" @click="onCreateProcesses"> 新增工序 </n-button>
        <n-data-table
          :columns="editData.productProcessesTableColumns"
          :data="productProcessesData"
          :pagination="false"
          :bordered="false"
        />
      </div>

      <template #action>
        <n-button type="info" size="small" @click="onSave" :loading="editLoadingRef">
          保存
        </n-button>
      </template>
    </n-card>

    <ModalDialog
      ref="modalDialogRef"
      @confirm="onDialogConfirm"
      :maskClosable="false"
      :isClosable="true"
      title="选择产品工艺"
      contentHeight="70vh"
      contentWidth="60vh"
    >
      <template #content>
        <CommonTableSelector
          ref="vodTableSelectorRef"
          :getApiUrl="productCraftApi.query"
          :searchFormOptions="searchForm"
          :baseTableColumns="editData.tableColumns"
          :tableHeight="300"
        />
      </template>
    </ModalDialog>
  </div>
</template>

<script lang="ts">
  import { productApi, productCraftApi } from '@/api/url'
  import { NButton, NInputNumber, NSwitch, useMessage } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref } from 'vue'
  import { RouteRecordRaw, useRoute, useRouter } from 'vue-router'
  import { get, post } from '@/api/http'
  import useVisitedRouteStore from '@/store/modules/visited-routes'
  import useProductTypeCacheStore from '@/store/modules/product-type'
  import { FormItem, ModalDialogType } from '@/types/components'
  import { renderInput, renderNumberInput, renderSelect, renderTagByEnum } from '@/hooks/form'
  import { getListByEnum } from '@/utils'
  import {
    BillingTypeEnum,
    CraftTypeEnum,
    ProductTypeEnum,
    PublicStatusEnum,
    baseRemarkLength,
  } from '@/store/types'
  import { ProductProcesses } from './Data'
  export default defineComponent({
    name: 'ProductEdit',
    setup() {
      const useProductTypeCache = useProductTypeCacheStore()
      const message = useMessage()
      const useVisitedRoute = useVisitedRouteStore()
      const route = useRoute()
      const router = useRouter()

      const modalDialogRef = ref<ModalDialogType | null>(null)
      const vodTableSelectorRef = ref<any>(null)
      const submitForm = ref<any>(null)
      const editLoadingRef = ref(false)

      const productProcessesData = reactive<ProductProcesses[]>([])
      const editData = reactive<any>({
        productData: null,
        productProcessesTableColumns: [
          {
            title: '排序',
            key: 'orderBy',
            render(row: any, index: number) {
              return h(NInputNumber, {
                value: row.orderBy,
                min: 1,
                max: 9999,
                placeholder: '越大越靠前',
                onUpdateValue(newVal: any) {
                  productProcessesData[index].orderBy = newVal
                },
              })
            },
          },
          {
            title: '工艺名称',
            key: 'craftName',
          },
          {
            title: '计费类型',
            key: 'billingType',
            render: (rowData: any) =>
              renderTagByEnum(
                rowData.billingType,
                BillingTypeEnum,
                {
                  1: 'info',
                  2: 'warning',
                },
                {
                  size: 'small',
                }
              ),
          },
          {
            title: '工艺基础价格',
            key: 'craftPrice',
          },
          {
            title: '工序价格',
            key: 'processingPrice',
            render(row: any, index: number) {
              return h(NInputNumber, {
                value: row.processingPrice,
                min: 0,
                max: 9999,
                placeholder: '计算工资使用 除工艺价格以外',
                onUpdateValue(newVal: any) {
                  productProcessesData[index].processingPrice = newVal
                },
              })
            },
          },
          {
            title: '工序计费价',
            key: 'sumPrice',
            render: (rowData: any) => {
              return rowData.craftPrice + rowData.processingPrice
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
        isLoose: null,
        isProcesses: false,
        title: null,
        isShowCraft: false,
        tableColumns: [
          {
            type: 'selection',
          },
          {
            title: '工艺名',
            key: 'craftName',
            fixed: 'left',
          },
          {
            title: '工艺类型',
            key: 'craftType',
            render: (rowData: any) =>
              renderTagByEnum(
                rowData.craftType,
                CraftTypeEnum,
                {
                  1: 'warning',
                  2: 'info',
                },
                {
                  size: 'small',
                }
              ),
          },
          {
            title: '计费类型',
            key: 'billingType',
            render: (rowData: any) =>
              renderTagByEnum(
                rowData.billingType,
                BillingTypeEnum,
                {
                  1: 'info',
                  2: 'warning',
                },
                {
                  size: 'small',
                }
              ),
          },
          {
            title: '工艺基础价格',
            key: 'unitPrice',
          },
          {
            title: '创建时间',
            key: 'createdTime',
            width: 80,
            ellipsis: {
              tooltip: true,
            },
          },
          {
            title: '修改时间',
            key: 'modifyTime',
            width: 80,
            ellipsis: {
              tooltip: true,
            },
          },
        ],
      })

      const searchForm = [
        {
          label: '关键字',
          key: 'keyWord',
          value: ref(null),
          render: (formItem: any) =>
            renderInput(formItem.value, {
              placeholder: '关键字',
              clearable: true,
            }),
        },
        {
          label: '状态',
          key: 'craftStatus',
          value: ref(PublicStatusEnum.正常),
          hidden: true,
        },
        {
          label: '工艺类型',
          key: 'craftType',
          value: ref(CraftTypeEnum.产品工艺),
          render: (formItem) => renderSelect(formItem.value, getListByEnum(CraftTypeEnum)),
        },
      ] as Array<FormItem>

      //创建产品
      const CreateProductFormOptions = [
        {
          key: 'id',
          value: ref(null),
          hidden: true,
        },
        {
          label: '产品分类',
          key: 'productTypeId',
          value: ref(null),
          required: true,
          render: (formItem) => {
            const useProductTypeCache = useProductTypeCacheStore()
            return renderSelect(formItem.value, useProductTypeCache.cachedItems, {
              placeholder: '产品分类',
              filterable: true,
            })
          },
        },

        {
          label: '产品类型',
          key: 'productType',
          value: ref(null),
          required: true,
          render: (formItem) =>
            renderSelect(
              formItem.value,
              getListByEnum(ProductTypeEnum),
              {
                placeholder: '产品类型',
              },
              function (newVal: any) {
                editData.isProcesses = newVal === ProductTypeEnum.加工产品
              }
            ),
        },
        {
          label: '产品名',
          key: 'productName',
          value: ref(null),
          required: true,
          span: 3,
          render: (formItem: any) =>
            renderInput(formItem.value, {
              placeholder: '产品名',
              maxlength: 15,
              showCount: true,
            }),
        },
        {
          label: '是否散件',
          key: 'isLoose',
          value: ref(false),
          required: true,
          span: 1,
          validator(value, message) {
            if (value) {
              return true
            }

            message.error('请选择是否散件')
          },
          render: (formItem) => {
            return h(NSwitch, {
              value: formItem.value.value,
              onUpdateValue: (newVal: boolean) => {
                formItem.value.value = newVal
                editData.isLoose = newVal
              },
            })
          },
        },

        {
          label: '单位',
          key: 'unit',
          value: ref(null),
          required: true,
          span: 1,
          render: (formItem: any) =>
            renderInput(formItem.value, {
              placeholder: 'KG/个',
              maxlength: 5,
              showCount: true,
            }),
        },
        {
          label: '规格',
          key: 'spec',
          value: ref(null),
          span: 1,
          render: (formItem: any) =>
            renderInput(formItem.value, {
              placeholder: '箱/盒/袋',
              maxlength: 5,
              showCount: true,
              disabled: editData.isLoose,
            }),
        },
        {
          label: '规格数',
          key: 'specCount',
          value: ref(null),
          span: 1,
          render: (formItem: any) =>
            renderNumberInput(formItem.value, {
              placeholder: '规格数',
              min: 1,
              disabled: editData.isLoose,
            }),
        },

        {
          label: '备注',
          key: 'productRemark',
          value: ref(null),
          span: 3, //grid-item 生效
          render: (formItem: any) =>
            renderInput(formItem.value, {
              placeholder: '产品备注',
              maxlength: baseRemarkLength,
              type: 'textarea',
            }),
        },
      ] as Array<FormItem>

      //移除产品工序
      function onRemoveItem(rowData: any) {
        productProcessesData.length = 0
        productProcessesData.push(
          ...productProcessesData.filter((item) => item.craftId !== rowData.craftId)
        )
      }

      //加载详情
      function loadDetail() {
        //detailId.value = route.params.id
        editData.title = '新增产品'
        if (!route.params.id) {
          return
        }

        editLoadingRef.value = true
        get({
          url: productApi.detail + '/' + route.params.id,
        })
          .then((res) => {
            editData.title = `【${res.data?.productName ?? ''} 】编辑`
            editData.productData = res.data
            editData.isProcesses = res.data.productType === ProductTypeEnum.加工产品
            editData.isLoose = res.data.isLoose

            //加工产品
            if (res.data.productProcesses) {
              productProcessesData.push(
                ...res.data.productProcesses.map((item: any) => {
                  return {
                    orderBy: item.orderBy,
                    craftId: item.craftId,
                    craftName: item.productCraft.craftName,
                    billingType: item.productCraft.billingType,
                    craftPrice: item.productCraft.craftPrice,
                    processingPrice: item.processingPrice,
                  } as ProductProcesses
                })
              )
            }

            //遍历赋值
            CreateProductFormOptions.forEach((it: any) => {
              if (it.key == 'isLoose') {
                it.value.value = editData.productData[it.key] || false
                return
              }

              it.value.value = editData.productData[it.key] || null
            })
            //console.log('detail', res.data)
          })
          .finally(() => {
            editLoadingRef.value = false
          })
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
          if (
            formData.productType == ProductTypeEnum.加工产品 &&
            productProcessesData.length <= 0
          ) {
            message.error('加工商品，请添加产品工序')
            return
          }

          if (formData.productType != ProductTypeEnum.加工产品) {
            productProcessesData.length = 0
          }

          editLoadingRef.value = true

          post({
            url: productApi.createOrUpdate,
            data: {
              ...formData,
              productProcesses: productProcessesData,
            },
          })
            .then((res: any) => {
              message.success(res.msg)

              useVisitedRoute
                .removeVisitedRoute({
                  name: 'ProductEdit',
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
            orderBy: 1,
            craftId: item.id,
            craftName: item.craftName,
            billingType: item.billingType,
            craftPrice: item.unitPrice,
            processingPrice: 0,
          } as ProductProcesses
        })
        productProcessesData.push(...tempArray)
        modalDialogRef.value?.close()
        //console.log('data', vodTableSelectorRef.value?.checkedRows)
      }

      onMounted(async () => {
        loadDetail()
        await useProductTypeCache.init()
      })

      return {
        searchForm,
        submitForm,
        productApi,
        productCraftApi,
        CreateProductFormOptions,
        editLoadingRef,
        editData,
        vodTableSelectorRef,
        modalDialogRef,
        productProcessesData,
        onCreateProcesses,
        onSave,
        onDialogConfirm,
      }
    },
  })
</script>
