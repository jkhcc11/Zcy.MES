<template>
  <div class="main-container">
    <n-card title="批量创建报工" style="margin-bottom: 16px">
      <n-skeleton v-if="editData.saveLoading" text :repeat="10" />
      <DataForm
        ref="submitForm"
        :options="BatchCreateReportWorkFormOptions"
        :form-config="{
          labelWidth: 100,
          size: 'medium',
          labelAlign: 'right',
        }"
        preset="grid-item"
      />

      <n-divider title-placement="left"> 报工列表 </n-divider>

      <n-button
        type="success"
        size="small"
        @click="onCreateProcesses"
        :loading="editData.loadingProductCraft"
      >
        选择工艺
      </n-button>

      <n-gradient-text type="warning"> 注：工作时长为0，需要移除后提交 </n-gradient-text>
      <n-data-table
        :columns="editData.reportWorkItemsTableColumns"
        :data="productProcessesData"
        :pagination="false"
        :bordered="false"
        :row-key="tableRowKey"
        :max-height="250"
      />

      <template #action>
        <n-button type="info" size="small" @click="onSave" :loading="editData.saveLoading">
          保存
        </n-button>
        <n-gradient-text type="info"> 报工数量：{{ productProcessesData.length }} </n-gradient-text>
      </template>
    </n-card>

    <ModalDialog
      ref="modalDialogRef"
      @confirm="onDialogConfirm"
      :maskClosable="false"
      :isClosable="true"
      :title="editData.showProcessesTilte"
      contentHeight="70vh"
      contentWidth="60vh"
    >
      <template #content>
        <n-data-table
          :columns="editData.productCraftTableColumns"
          :data="editData.productCraftData"
          :pagination="false"
          :bordered="false"
          :loading="editData.loadingProductCraft"
          :row-key="tableRowKey"
          :checked-row-keys="checkedRowKeys"
          @update:checked-row-keys="onRowCheck"
        />
      </template>
    </ModalDialog>
  </div>
</template>

<script lang="ts">
  import { productApi, reportWorkApi } from '@/api/url'
  import { NButton, NDatePicker, NInput, NInputNumber, useDialog, useMessage } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref } from 'vue'
  import { RouteRecordRaw, useRouter } from 'vue-router'
  import { get, post } from '@/api/http'
  import useVisitedRouteStore from '@/store/modules/visited-routes'
  import { FormItem, ModalDialogType } from '@/types/components'
  import { renderSelect, renderTagByEnum } from '@/hooks/form'
  import { BillingTypeEnum, ProductTypeEnum, baseRemarkLength } from '@/store/types'
  import { BatchCreateReportWorkItem } from './Data'
  import useProductCacheStore from '@/store/modules/product'
  import { useRowKey } from '@/hooks/table'
  import useCompanyCacheStore from '@/store/modules/company'
  export default defineComponent({
    name: 'BatchReportWork',
    setup() {
      const useCompanyCache = useCompanyCacheStore()
      const useProductCache = useProductCacheStore()
      const message = useMessage()
      const naiveDialog = useDialog()
      const useVisitedRoute = useVisitedRouteStore()
      const router = useRouter()

      const modalDialogRef = ref<ModalDialogType | null>(null)
      const submitForm = ref<any>(null)

      const tableRowKey = useRowKey('productProcessId')
      const checkedRowKeys = ref<Array<string | number>>([])
      const checkedRows = ref<object[]>([])

      const productProcessesData = reactive<BatchCreateReportWorkItem[]>([])
      const editData = reactive<any>({
        productId: null,
        loadingProductCraft: false,
        saveLoading: false,
        showProcessesTilte: null,
        reportWorkItemsTableColumns: [
          {
            title: '产品名',
            key: 'productName',
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
            title: '工作时长',
            key: 'wordDuration',
            render(row: any, index: number) {
              let numberMax = 99999
              let precision = 0
              if (row.billingType == BillingTypeEnum.计时) {
                numberMax = 24
                precision = 1
              }

              return h(
                NInputNumber,
                {
                  value: row.wordDuration,
                  min: 0.1,
                  max: numberMax,
                  precision: precision,
                  placeholder: '工作时长',
                  onUpdateValue(newVal: any) {
                    productProcessesData[index].wordDuration = newVal
                  },
                },
                {
                  suffix: () => {
                    let unit = '个'
                    if (row.billingType == BillingTypeEnum.计时) {
                      unit = '小时'
                    }

                    return `${unit}`
                  },
                }
              )
            },
          },
          {
            title: '备注',
            key: 'remark',
            render(row: any, index: number) {
              return h(NInput, {
                value: row.remark,
                length: baseRemarkLength,
                placeholder: '报工备注',
                onUpdateValue(newVal: any) {
                  productProcessesData[index].remark = newVal
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
        productCraftData: [],
        productCraftTableColumns: [
          {
            type: 'selection',
          },
          {
            title: '工艺名',
            key: 'craftName',
            fixed: 'left',
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
        ],
      })

      const BatchCreateReportWorkFormOptions = [
        {
          label: '员工',
          key: 'employeeId',
          value: ref(null),
          required: true,
          span: 2,
          render: (formItem) => {
            return renderSelect(formItem.value, useCompanyCache.cachedItems, {
              placeholder: '员工',
              filterable: true,
            })
          },
        },
        {
          label: '报工日期',
          key: 'reportWorkDate',
          value: ref(null),
          span: 1, //grid-item 生效
          render: (formItem: any) => {
            return h(NDatePicker, {
              type: 'date',
              valueFormat: 'yyyy-MM-dd 00:00:00',
              defaultFormattedValue: formItem.value.value,
              placeholder: '报工日期，不填为当天',
              isDateDisabled: function (ts: number) {
                return ts > Date.now()
              },
              onUpdateFormattedValue: function (newVal: any) {
                formItem.value.value = newVal
              },
            })
          },
        },
        {
          label: '产品',
          key: 'productId',
          value: ref(null),
          required: true,
          span: 2,
          render: (formItem) => {
            return renderSelect(
              formItem.value,
              useProductCache.cachedItems,
              {
                placeholder: '产品',
                filterable: true,
              },
              function (newVal: any) {
                editData.productId = newVal
                loadProductDetail()
              }
            )
          },
        },
      ] as Array<FormItem>

      //移除工艺
      function onRemoveItem(rowData: any) {
        const tempArray = productProcessesData.filter(
          (item) => item.productProcessId !== rowData.productProcessId
        )
        productProcessesData.length = 0
        productProcessesData.push(...tempArray)
      }

      //加载产品工艺
      function loadProductDetail() {
        editData.loadingProductCraft = true
        get({
          url: productApi.detailCascade + '/' + editData.productId,
        })
          .then((res: any) => {
            editData.showProcessesTilte = `【${res.data.productName} 】工艺列表`
            const productName = res.data.productName
            editData.productCraftData = res.data.productProcesses.map((item: any) => {
              return {
                productProcessId: item.productProcessId,
                craftName: item.productCraft.craftName,
                productName: productName,
                billingType: item.productCraft.billingType,
              }
            })
          })
          .finally(() => {
            editData.loadingProductCraft = false
          })
      }

      //新增工序
      function onCreateProcesses() {
        if (editData.productCraftData.length <= 0) {
          message.warning('请先选择产品')
          return
        }

        checkedRowKeys.value = []
        checkedRows.value = []
        modalDialogRef.value?.show()
      }

      //保存
      function onSave() {
        if (submitForm.value?.validator()) {
          const formData = submitForm.value?.generatorParams()
          if (productProcessesData.length <= 0) {
            message.error('请添加报工列表')
            return
          }

          editData.saveLoading = true
          post({
            url: reportWorkApi.batchCreate,
            data: {
              ...formData,
              reportWorkItems: productProcessesData,
            },
            method: 'PUT',
          })
            .then((res: any) => {
              message.success(res.msg)

              submitForm.value?.reset()
              productProcessesData.length = 0

              naiveDialog.warning({
                title: '提示',
                content: '是否退出添加？',
                positiveText: '退出',
                onPositiveClick: () => {
                  useVisitedRoute
                    .removeVisitedRoute({
                      name: 'BatchCreateReportWork',
                    } as RouteRecordRaw)
                    .then((lastPath) => {
                      router.push(lastPath)
                    })
                },
              })
            })
            .finally(() => {
              editData.saveLoading = false
            })
        }
      }

      //选择工艺确认
      function onDialogConfirm() {
        if (checkedRows.value == null || checkedRows.value.length <= 0) {
          message.warning('请先选择产品工艺')
          return
        }

        var tempArray = checkedRows.value.map((item: any) => {
          return {
            productProcessId: item.productProcessId,
            productName: item.productName,
            craftName: item.craftName,
            billingType: item.billingType,
            wordDuration: 0,
            remark: '',
          } as BatchCreateReportWorkItem
        })

        tempArray.forEach((newItem: BatchCreateReportWorkItem) => {
          const exists = productProcessesData.some(
            (item) => item.productProcessId === newItem.productProcessId
          )
          if (exists) {
            message.warning(`【 ${newItem.productName}/${newItem.craftName} 】已存在，跳过！`)
          } else {
            productProcessesData.push(newItem)
          }
        })

        modalDialogRef.value?.close()
        //console.log('data', vodTableSelectorRef.value?.checkedRows)
      }

      //选中
      function onRowCheck(rowKeys: Array<any>, rows: object[]) {
        //checkedRowKeys.length = 0
        checkedRowKeys.value = rowKeys
        checkedRows.value = rows
      }

      onMounted(async () => {
        await useProductCache.init(ProductTypeEnum.加工产品)
        await useCompanyCache.init()
      })

      return {
        submitForm,
        productApi,
        BatchCreateReportWorkFormOptions,
        editData,
        modalDialogRef,
        productProcessesData,
        checkedRowKeys,
        tableRowKey,
        onCreateProcesses,
        onSave,
        onDialogConfirm,
        onRowCheck,
      }
    },
  })
</script>
