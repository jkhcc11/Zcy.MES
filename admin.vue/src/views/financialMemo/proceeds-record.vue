<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="proceedsRecordApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchProceedsRecordOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :submitLoading="submitLoading"
    :editDialogTilte="editTitle"
    :onDialogConfirm="onBtnClick.onSaveSubmit"
    @load-data-success="onBtnClick.onLoadDataSuccess"
    editContentHeight="50vh"
  >
    <template #tableToolbar>
      <n-space>
        <n-button type="info" size="small" @click="onBtnClick.create"> 创建 </n-button>
        <n-tag :bordered="false" type="success"> 总额： {{ totalsRef.totalMoney ?? '-' }}</n-tag>

        <n-gradient-text type="warning">
          注：默认数据为近30天数据，更多数据请点击搜索
        </n-gradient-text>
      </n-space>
    </template>

    <template #showDialogContent>
      <DataForm
        ref="submitForm"
        :options="CreateProceedsRecordFormOptions"
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
  import { get, post, sendDelete } from '@/api/http'
  import { proceedsRecordApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog, NTag } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchProceedsRecordOptions, CreateProceedsRecordFormOptions } from './Data'
  import { renderTagByEnum } from '@/hooks/form'
  import { AccountTypeEnum, ClientTypeEnum } from '@/store/types'
  import useClientCacheStore from '@/store/modules/client-cache'
  export default defineComponent({
    name: 'ProceedsRecordList',
    setup() {
      const useClientCache = useClientCacheStore()
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const totalsRef = reactive({
        totalMoney: null,
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
              innerHTML: `<strong>备注：</strong>${rowData.remark ?? ''} <br/>`,
            })
          },
        },
        {
          title: '客户名',
          key: 'supplierClientName',
        },
        {
          title: '账户',
          key: 'accountType',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.accountType,
              AccountTypeEnum,
              {
                1: 'info',
                2: 'info',
                3: 'info',
                4: 'info',
                100: 'tertiary',
              },
              {
                size: 'small',
              }
            ),
        },
        {
          title: '收款名称',
          key: 'proceedsRecordName',
          fixed: 'left',
        },
        {
          title: '金额',
          key: 'money',
          render: (rowData: any) => {
            return h(
              NTag,
              {
                type: 'success',
                size: 'small',
              },
              {
                default: () => '+' + rowData.money,
              }
            )
          },
        },
        {
          title: '记录日期',
          key: 'recordDate',
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
            const normalAction = useRenderAction([
              {
                label: '删除',
                type: 'error',
                onClick: onBtnClick.delete.bind(null, rowData),
              },
            ] as TableActionModel[])

            return normalAction
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
                url: proceedsRecordApi.delete,
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
            let postUrl = proceedsRecordApi.create
            submitLoading.value = true
            post({
              url: postUrl,
              data: pd,
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
        //加载完成
        onLoadDataSuccess: function () {
          get({
            url: proceedsRecordApi.getTotals,
            data: {},
          })
            .then((res) => {
              totalsRef.totalMoney = res.data.totalMoney
            })
            .finally(() => {})
        },
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      onMounted(async () => {
        await useClientCache.init(ClientTypeEnum.客户)
      })

      return {
        proceedsRecordApi,
        commonQueryListRef,
        tableColumns,
        SearchProceedsRecordOptions,
        CreateProceedsRecordFormOptions,
        submitLoading,
        submitForm,
        editTitle,
        totalsRef,
        onBtnClick,
      }
    },
  })
</script>
