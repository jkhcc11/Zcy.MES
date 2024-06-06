<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="incomeRecordApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchIncomeRecordOptions"
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
        <n-tag :bordered="false" type="success"> 进账： {{ totalsRef.inMoney ?? '-' }}</n-tag>
        <n-tag :bordered="false" type="warning"> 出账： {{ totalsRef.outMoney ?? '-' }} </n-tag>
        <n-tag :bordered="false"> 盈亏： {{ totalsRef.profitMoney ?? '-' }} </n-tag>

        <n-gradient-text type="warning">
          注：默认数据为近30天数据，更多数据请点击搜索
        </n-gradient-text>
      </n-space>
    </template>

    <template #showDialogContent>
      <DataForm
        ref="submitForm"
        :options="CreateIncomeRecordFormOptions"
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
  import { incomeRecordApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog, NTag } from 'naive-ui'
  import { defineComponent, h, reactive, ref, render } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchIncomeRecordOptions, CreateIncomeRecordFormOptions } from './Data'
  import { renderTagByEnum } from '@/hooks/form'
  import { AccountTypeEnum, IncomeTypeEnum } from '@/store/types'
  export default defineComponent({
    name: 'IncomeRecordList',
    setup() {
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
          type: 'expand',
          fixed: 'left',
          renderExpand: (rowData: any) => {
            return h('div', {
              innerHTML: `<strong>备注：</strong>${rowData.remark ?? ''} <br/>`,
            })
          },
        },
        {
          title: '收支',
          key: 'incomeType',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.incomeType,
              IncomeTypeEnum,
              {
                1: 'success',
                5: 'warning',
              },
              {
                size: 'small',
              }
            ),
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
          title: '款项名称',
          key: 'recordName',
          fixed: 'left',
        },
        {
          title: '金额',
          key: 'money',
          render: (rowData: any) => {
            if (rowData.incomeType == IncomeTypeEnum.出账) {
              return h(
                NTag,
                {
                  type: 'warning',
                  size: 'small',
                },
                {
                  default: () => rowData.money,
                }
              )
            }

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
          title: '经办人',
          key: 'managerUser',
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
                url: incomeRecordApi.delete,
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
            let postUrl = incomeRecordApi.create
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
          const params = commonQueryListRef.value?.getQueryParams()
          get({
            url: incomeRecordApi.getTotals,
            data: params,
          })
            .then((res) => {
              totalsRef.inMoney = res.data.inMoney
              totalsRef.outMoney = res.data.outMoney
              totalsRef.profitMoney = res.data.profitMoney
            })
            .finally(() => {})
        },
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      return {
        incomeRecordApi,
        commonQueryListRef,
        tableColumns,
        SearchIncomeRecordOptions,
        CreateIncomeRecordFormOptions,
        submitLoading,
        submitForm,
        editTitle,
        totalsRef,
        onBtnClick,
      }
    },
  })
</script>
