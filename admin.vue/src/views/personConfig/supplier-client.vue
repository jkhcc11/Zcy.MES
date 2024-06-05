<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="supplierClientApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchSupplierClientOptions"
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
        :options="CreateSupplierClientFormOptions"
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
  import { supplierClientApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog } from 'naive-ui'
  import { defineComponent, h, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchSupplierClientOptions, CreateSupplierClientFormOptions } from './Data'
  import { renderTagByEnum } from '@/hooks/form'
  import { ClientTypeEnum, PublicStatusEnum } from '@/store/types'
  import useUserStore from '@/store/modules/user'
  export default defineComponent({
    name: 'SupplierClientList',
    setup() {
      const useUser = useUserStore()
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
          type: 'expand',
          fixed: 'left',
          renderExpand: (rowData: any) => {
            return h('div', {
              innerHTML: `<strong>备注：</strong>${rowData.remark ?? ''} <br/>`,
            })
          },
        },
        {
          title: '客户类型',
          key: 'clientType',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.clientType,
              ClientTypeEnum,
              {
                1: 'info',
                5: 'warning',
              },
              {
                size: 'small',
              }
            ),
        },
        {
          title: '客户名',
          key: 'clientName',
          fixed: 'left',
        },
        {
          title: '客户状态',
          key: 'clientStatus',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.clientStatus,
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
          title: '联系方式',
          key: 'phoneNumber',
        },
        {
          title: '备注',
          key: 'remark',
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

            if (rowData.clientStatus == PublicStatusEnum.正常) {
              tempArry.push({
                label: '下架',
                type: 'warning',
                onClick: onBtnClick.down.bind(null, rowData),
              } as TableActionModel)
            }

            if (rowData.clientStatus == PublicStatusEnum.禁用) {
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
          editTitle.value = `${rowData.clientName} 编辑`

          CreateSupplierClientFormOptions.forEach((it: any) => {
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
                url: supplierClientApi.delete,
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
            let postUrl = supplierClientApi.createAndUpdate
            submitLoading.value = true
            post({
              url: postUrl,
              data: {
                companyId: useUser.companyId,
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
        //下架
        down: function (rowData: any) {
          sendDelete({
            url: supplierClientApi.ban + '/' + rowData.id,
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
        supplierClientApi,
        commonQueryListRef,
        tableColumns,
        SearchSupplierClientOptions,
        CreateSupplierClientFormOptions,
        submitLoading,
        submitForm,
        editTitle,
        onBtnClick,
      }
    },
  })
</script>
