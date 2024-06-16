<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="reportWorkApi.queryForAdmin"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchReportWorkOptions"
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

        <n-button type="warning" size="small" @click="onBtnClick.batchCreate"> 批量创建 </n-button>
        <n-tag :bordered="false" type="warning">
          计时总工时： {{ totalsRef.timingWordDuration ?? '-' }}</n-tag
        >
        <n-tag :bordered="false" type="error">
          计件总数： {{ totalsRef.countingWordDuration ?? '-' }}</n-tag
        >

        <n-gradient-text type="warning">
          注：默认数据为近30天数据，更多数据请点击搜索
        </n-gradient-text>
      </n-space>
    </template>

    <template #showDialogContent>
      <DataForm
        ref="submitForm"
        :options="CreateReportWorkFormOptions"
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
  import { reportWorkApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import {
    SearchReportWorkOptions,
    CreateReportWorkFormOptions,
    UpdateReportWorkFormOptions,
  } from './Data'
  import { renderTagByEnum } from '@/hooks/form'
  import { BillingTypeEnum, ProductTypeEnum } from '@/store/types'
  import useProductCacheStore from '@/store/modules/product'
  import useCompanyCacheStore from '@/store/modules/company'
  import { useRouter } from 'vue-router'
  export default defineComponent({
    name: 'ReportWorkAdmin',
    setup() {
      const useCompanyCache = useCompanyCacheStore()
      const useProductCache = useProductCacheStore()
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const router = useRouter()
      const totalsRef = reactive({
        timingWordDuration: null,
        countingWordDuration: null,
      })

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)
      const updateSubmitForm = ref<DataFormType | null>(null)
      const editTitle = ref('新增')
      const optType = ref<'create' | 'update'>('create')

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
          title: '报工日期',
          key: 'reportWorkDate',
          width: 110,
        },
        {
          title: '员工名',
          key: 'employeeNickName',
        },
        {
          title: '产品名',
          key: 'productName',
        },
        {
          title: '工艺名',
          key: 'productCraftName',
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
          width: 180,
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
        batchCreate: function () {
          router.push('/production/batch-create-report-work')
        },
        //创建
        create: function () {
          optType.value = 'create'
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
                url: reportWorkApi.delete,
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
          if (submitForm.value?.validator() || updateSubmitForm.value?.validator()) {
            let pd = submitForm.value?.generatorParams()
            let postUrl = reportWorkApi.create
            let method = 'PUT'
            submitLoading.value = true
            post({
              url: postUrl,
              data: pd,
              method: method,
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
            url: reportWorkApi.getTotals,
            data: params,
          })
            .then((res) => {
              totalsRef.timingWordDuration = res.data.timingWordDuration
              totalsRef.countingWordDuration = res.data.countingWordDuration
            })
            .finally(() => {})
        },
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      onMounted(async () => {
        await useCompanyCache.init()
        await useProductCache.init(ProductTypeEnum.加工产品)
      })

      return {
        reportWorkApi,
        commonQueryListRef,
        tableColumns,
        SearchReportWorkOptions,
        CreateReportWorkFormOptions,
        UpdateReportWorkFormOptions,
        submitLoading,
        submitForm,
        updateSubmitForm,
        editTitle,
        totalsRef,
        optType,
        onBtnClick,
      }
    },
  })
</script>
