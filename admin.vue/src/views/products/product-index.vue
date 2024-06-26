<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="productApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchProductOptions"
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
        :columns="detailData.productProcessesTableColumns"
        :data="detailData.productProcessesData"
        :pagination="false"
        :bordered="false"
      />
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { get, post, sendDelete } from '@/api/http'
  import { productApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchProductOptions } from './Data'
  import { renderTagByEnum } from '@/hooks/form'
  import { PublicStatusEnum, ProductTypeEnum, BillingTypeEnum } from '@/store/types'
  import useProductTypeCacheStore from '@/store/modules/product-type'
  import { useRouter } from 'vue-router'
  export default defineComponent({
    name: 'ProductIndex',
    setup() {
      const useProductTypeCache = useProductTypeCacheStore()
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const router = useRouter()

      const detailData = reactive<any>({
        productProcessesData: [],
        productProcessesTableColumns: [
          {
            title: '排序',
            key: 'orderBy',
          },
          {
            title: '工艺名称',
            key: 'productCraft.craftName',
          },
          {
            title: '计费类型',
            key: 'productCraft.billingType',
            render: (rowData: any) =>
              renderTagByEnum(
                rowData.productCraft.billingType,
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
            key: 'productCraft.craftPrice',
          },
          {
            title: '工序价格',
            key: 'processingPrice',
          },
          {
            title: '工序计费价',
            key: 'sumPrice',
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
              innerHTML: `<strong>备注：</strong>${rowData.productRemark ?? ''} <br/>`,
            })
          },
        },
        {
          title: '产品分类',
          key: 'productTypeName',
          fixed: 'left',
        },
        {
          title: '产品名',
          key: 'productName',
          fixed: 'left',
        },
        {
          title: '状态',
          key: 'productStatus',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.productStatus,
              PublicStatusEnum,
              {
                1: 'success',
                5: 'error',
              },
              {
                size: 'small',
              }
            ),
        },
        {
          title: '产品类型',
          key: 'productType',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.productType,
              ProductTypeEnum,
              {
                1: 'success',
                2: 'warning',
                3: 'info',
                4: 'error',
              },
              {
                size: 'small',
              }
            ),
        },
        {
          title: '是否散件',
          key: 'isLoose',
          render: (rowData: any) => {
            return rowData.isLoose ? '是' : '否'
          },
        },
        {
          title: '单位',
          key: 'unit',
        },
        {
          title: '规格',
          key: 'spec',
        },
        {
          title: '规格数',
          key: 'specCount',
        },
        {
          title: '备注',
          key: 'productRemark',
          width: 50,
          ellipsis: {
            tooltip: true,
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
          width: 125,
          render: (rowData: any) => {
            var tempArry: TableActionModel[] = []

            tempArry.push({
              label: '复制',
              type: 'info',
              onClick: onBtnClick.copy.bind(null, rowData),
            } as TableActionModel)

            if (rowData.productStatus == PublicStatusEnum.正常) {
              if (rowData.productType == ProductTypeEnum.加工产品) {
                tempArry.push({
                  label: '详情',
                  type: 'info',
                  onClick: onBtnClick.detail.bind(null, rowData),
                } as TableActionModel)
              }

              tempArry.push({
                label: '下架',
                type: 'warning',
                onClick: onBtnClick.down.bind(null, rowData),
              } as TableActionModel)
            }

            if (rowData.productStatus == PublicStatusEnum.禁用) {
              tempArry.push({
                label: '编辑',
                type: 'info',
                onClick: onBtnClick.edit.bind(null, rowData),
              } as TableActionModel)

              tempArry.push({
                label: '上架',
                type: 'success',
                onClick: onBtnClick.down.bind(null, rowData),
              } as TableActionModel)

              tempArry.push({
                label: '删除',
                type: 'error',
                onClick: onBtnClick.delete.bind(null, rowData),
              } as TableActionModel)
            }

            return useRenderAction(tempArry)
          },
        },
      ]

      const onBtnClick = {
        //创建
        create: function () {
          router.push('/products/product-edit')
        },
        //编辑
        edit: function (rowData: any) {
          router.push('/products/product-edit/' + rowData.id)
        },
        //详情
        detail: function (rowData: any) {
          commonQueryListRef.value?.showDialog()
          submitLoading.value = true
          get({
            url: productApi.detail + '/' + rowData.id,
          })
            .then((res) => {
              editTitle.value = `【${res.data?.productName ?? ''} 】工序详情`
              //加工产品
              if (res.data.productType == ProductTypeEnum.加工产品) {
                detailData.productProcessesData.length = 0
                detailData.productProcessesData.push(...res.data.productProcesses)
              }
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
                url: productApi.delete + '/' + rowData.id,
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
        //提交保存
        onSaveSubmit: function () {
          if (submitForm.value?.validator()) {
            const pd = submitForm.value?.generatorParams()
            let postUrl = productApi.createOrUpdate
            submitLoading.value = true
            post({
              url: postUrl,
              data: {
                ...pd,
              },
            })
              .then((res) => {
                message.success(res.msg)

                doRefresh()
                commonQueryListRef.value?.closeDialog()
              })
              .finally(() => {
                submitLoading.value = false
              })
          }
        },
        //下架|上架
        down: function (rowData: any) {
          sendDelete({
            url: productApi.banOrEnable + '/' + rowData.id,
            data: {},
          })
            .then((res) => {
              message.success(res.msg)
            })
            .finally(() => {
              doRefresh()
            })
        },
        //复制
        copy: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: `是否确认复制【${rowData.productName}】这个产品？`,
            positiveText: '确认',
            onPositiveClick: () => {
              post({
                url: productApi.copy + '/' + rowData.id,
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
        await useProductTypeCache.init()
      })

      return {
        productApi,
        commonQueryListRef,
        tableColumns,
        SearchProductOptions,
        submitLoading,
        submitForm,
        detailData,
        editTitle,
        onBtnClick,
      }
    },
  })
</script>
