<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="companyApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchCompanyOptions"
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
        :options="CreateCompanyFormOptions"
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
  import { companyApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog } from 'naive-ui'
  import { defineComponent, reactive, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchCompanyOptions, CreateCompanyFormOptions } from './Data'
  import useUserStore from '@/store/modules/user'
  export default defineComponent({
    name: 'CompanyIndex',
    setup() {
      const useUser = useUserStore()
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const totalsRef = reactive({
        inMoney: null,
        outMoney: null,
        profitMoney: null,
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
          title: '公司名',
          key: 'companyName',
          fixed: 'left',
        },
        {
          title: '缩写',
          key: 'shortName',
          fixed: 'left',
        },
        {
          title: '公司抬头',
          key: 'companyShowName',
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
            tempArry.push({
              label: '编辑',
              type: 'info',
              onClick: onBtnClick.edit.bind(null, rowData),
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
          editTitle.value = '新增'
          submitForm.value?.reset()
          commonQueryListRef.value?.showDialog()
        },
        //编辑
        edit: function (rowData: any) {
          editTitle.value = `${rowData.companyName} 编辑`

          CreateCompanyFormOptions.forEach((it: any) => {
            it.value.value = rowData[it.key] || null

            if (it.key === 'companyName' || it.key === 'shortName') {
              it.disabled.value = true
            }
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
                url: companyApi.delete,
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
            let postUrl = companyApi.createAndUpdate
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
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      return {
        companyApi,
        commonQueryListRef,
        tableColumns,
        SearchCompanyOptions,
        CreateCompanyFormOptions,
        submitLoading,
        submitForm,
        editTitle,
        totalsRef,
        onBtnClick,
      }
    },
  })
</script>
