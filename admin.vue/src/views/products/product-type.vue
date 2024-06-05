<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="productTypeApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchProductTypeOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :submitLoading="submitLoading"
    :editDialogTilte="editTitle"
    :onDialogConfirm="onBtnClick.onSaveSubmit"
    editContentHeight="20vh"
  >
    <template #tableToolbar>
      <n-space>
        <n-button type="info" size="small" @click="onBtnClick.create"> 创建 </n-button>
      </n-space>
    </template>

    <template #showDialogContent>
      <DataForm
        ref="submitForm"
        :options="CreateProductTypeFormOptions"
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
  import { productTypeApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog } from 'naive-ui'
  import { defineComponent, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchProductTypeOptions, CreateProductTypeFormOptions } from './Data'
  import { renderTagByEnum } from '@/hooks/form'
  import { PublicStatusEnum } from '@/store/types'
  export default defineComponent({
    name: 'ProductType',
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
          title: '分类名称',
          key: 'typeName',
          fixed: 'left',
        },
        {
          title: '状态',
          key: 'typeStatus',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.typeStatus,
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
            tempArry.push({
              label: '编辑',
              type: 'info',
              onClick: onBtnClick.edit.bind(null, rowData),
            } as TableActionModel)

            if (rowData.typeStatus == PublicStatusEnum.正常) {
              tempArry.push({
                label: '下架',
                type: 'warning',
                onClick: onBtnClick.down.bind(null, rowData),
              } as TableActionModel)
            }

            if (rowData.typeStatus == PublicStatusEnum.禁用) {
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
        //编辑
        edit: function (rowData: any) {
          editTitle.value = `${rowData.typeName} 编辑`

          CreateProductTypeFormOptions.forEach((it: any) => {
            it.value.value = rowData[it.key] || null
          })

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
                url: productTypeApi.delete,
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
            let postUrl = productTypeApi.createAndUpdate
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
            url: productTypeApi.ban + '/' + rowData.id,
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
        productTypeApi,
        commonQueryListRef,
        tableColumns,
        SearchProductTypeOptions,
        CreateProductTypeFormOptions,
        submitLoading,
        submitForm,
        editTitle,
        onBtnClick,
      }
    },
  })
</script>
