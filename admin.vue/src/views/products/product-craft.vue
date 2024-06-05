<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="productCraftApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchProductCraftOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :submitLoading="submitLoading"
    :editDialogTilte="editTitle"
    :onDialogConfirm="onBtnClick.onSaveSubmit"
    editContentHeight="50vh"
  >
    <template #tableToolbar>
      <n-space>
        <n-button type="info" size="small" @click="onBtnClick.create"> 创建 </n-button>
      </n-space>
    </template>

    <template #showDialogContent>
      <DataForm
        ref="submitForm"
        :options="CreateProductCraftFormOptions"
        :form-config="{
          labelWidth: 100,
          size: 'medium',
          labelAlign: 'right',
        }"
        preset="grid-item"
      />
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { post, sendDelete } from '@/api/http'
  import { productCraftApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog } from 'naive-ui'
  import { defineComponent, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchProductCraftOptions, CreateProductCraftFormOptions } from './Data'
  import { renderTagByEnum } from '@/hooks/form'
  import { PublicStatusEnum, BillingTypeEnum, CraftTypeEnum } from '@/store/types'
  export default defineComponent({
    name: 'ProductCraft',
    setup() {
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)
      const editTitle = ref('新增')

      //表格
      const table = useTable()
      const tableColumns = [
        table.indexColumn,
        {
          title: '工艺名',
          key: 'craftName',
          fixed: 'left',
        },
        {
          title: '状态',
          key: 'craftStatus',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.craftStatus,
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
          title: '备注',
          key: 'remark',
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
          width: 120,
          render: (rowData: any) => {
            var tempArry: TableActionModel[] = []

            if (rowData.craftStatus == PublicStatusEnum.正常) {
              tempArry.push({
                label: '下架',
                type: 'warning',
                onClick: onBtnClick.down.bind(null, rowData),
              } as TableActionModel)
            }

            if (rowData.craftStatus == PublicStatusEnum.禁用) {
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
          editTitle.value = '新增'
          submitForm.value?.reset()
          commonQueryListRef.value?.showDialog()
        },
        //删除
        delete: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: '是否要删除此数据？',
            positiveText: '删除',
            onPositiveClick: () => {
              sendDelete({
                url: productCraftApi.delete,
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
        //提交保存
        onSaveSubmit: function () {
          if (submitForm.value?.validator()) {
            const pd = submitForm.value?.generatorParams()
            let postUrl = productCraftApi.create
            submitLoading.value = true
            post({
              url: postUrl,
              data: {
                ...pd,
              },
              method: 'PUT',
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
            url: productCraftApi.ban + '/' + rowData.id,
            data: {},
          })
            .then((res) => {
              message.success(res.msg)
            })
            .finally(() => {
              doRefresh()
            })
        },
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      return {
        productCraftApi,
        commonQueryListRef,
        tableColumns,
        SearchProductCraftOptions,
        CreateProductCraftFormOptions,
        submitLoading,
        submitForm,
        editTitle,
        onBtnClick,
      }
    },
  })
</script>
