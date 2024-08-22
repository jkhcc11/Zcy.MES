<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="reportWorkApi.query"
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

        <n-button-group size="small">
          <DownloadButton @export="onBtnClick.export"> 员工汇总 </DownloadButton>
          <DownloadButton @export="onBtnClick.exportDateHorizontal"> 员工汇总样式1 </DownloadButton>
          <DownloadButton @export="onBtnClick.exportProduct"> 产品汇总 </DownloadButton>
        </n-button-group>

        <n-tag :bordered="false" type="success">
          实结算价格： {{ totalsRef.totalActualSettlementPrice ?? '-' }}</n-tag
        >
        <n-tag :bordered="false" type="info">
          应结算价格： {{ totalsRef.totalReceivableSettlementPrice ?? '-' }}</n-tag
        >
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
        v-if="optType == 'create'"
      />

      <DataForm
        ref="updateSubmitForm"
        :options="UpdateReportWorkFormOptions"
        :form-config="{
          labelWidth: 100,
          size: 'medium',
          labelAlign: 'right',
        }"
        preset="grid-item"
        v-if="optType == 'update'"
      />
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { get, post, sendDelete } from '@/api/http'
  import { reportWorkApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog, NTag } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import {
    SearchReportWorkOptions,
    CreateReportWorkFormOptions,
    UpdateReportWorkFormOptions,
  } from './Data'
  import { renderTagByEnum } from '@/hooks/form'
  import { BillingTypeEnum, ProductTypeEnum, PublicStatusEnum } from '@/store/types'
  import useProductCacheStore from '@/store/modules/product'
  import useCompanyCacheStore from '@/store/modules/company'
  export default defineComponent({
    name: 'ReportWork',
    setup() {
      const useCompanyCache = useCompanyCacheStore()
      const useProductCache = useProductCacheStore()
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const totalsRef = reactive({
        totalActualSettlementPrice: null,
        totalReceivableSettlementPrice: null,
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
          title: '工作量',
          key: 'wordDuration',
        },

        {
          title: '应结算价格',
          key: 'receivableSettlementPrice',
        },
        {
          title: '实结算价格',
          key: 'actualSettlementPrice',
          render: (rowData: any) => {
            return h(
              NTag,
              {
                type:
                  rowData.receivableSettlementPrice == rowData.actualSettlementPrice
                    ? 'default'
                    : 'warning',
                size: 'small',
              },
              {
                default: () => rowData.actualSettlementPrice,
              }
            )
          },
        },
        {
          title: '状态',
          key: 'reportWorkStatus',
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.reportWorkStatus,
              PublicStatusEnum,
              {
                1: 'success',
                2: 'warning',
                5: 'error',
                6: 'default',
              },
              {
                size: 'small',
              }
            ),
        },
        {
          title: '备注',
          key: 'remark',
          width: 50,
          ellipsis: true,
        },
        {
          title: '自助报工',
          key: 'isSelf',
          render: (rowData: any) => {
            const isSelf = rowData.createdUserId == rowData.employeeId
            return h(
              NTag,
              {
                type: isSelf ? 'warning' : 'success',
                size: 'small',
              },
              {
                default: () => (isSelf ? '是' : '否'),
              }
            )
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
          width: 180,
          render: (rowData: any) => {
            const tempArray: TableActionModel[] = []
            if (rowData.reportWorkStatus >= 5) {
              //禁用|驳回可删除
              tempArray.push({
                label: '删除',
                type: 'error',
                onClick: onBtnClick.delete.bind(null, rowData),
              } as TableActionModel)
            } else if (rowData.reportWorkStatus == PublicStatusEnum.正常) {
              tempArray.push({
                label: '调整价格',
                type: 'warning',
                onClick: onBtnClick.update.bind(null, rowData),
              } as TableActionModel)
            }

            const normalAction = useRenderAction(tempArray)
            return normalAction
          },
        },
      ]

      const onBtnClick = {
        //创建
        create: function () {
          optType.value = 'create'
          editTitle.value = '新增'
          submitForm.value?.reset()
          commonQueryListRef.value?.showDialog()
        },
        //调整
        update: function (rowData: any) {
          const tipMsg = `${rowData.employeeNickName}->${rowData.reportWorkDate}->${rowData.productCraftName}`
          optType.value = 'update'
          editTitle.value = `【${tipMsg}】调整价格`
          UpdateReportWorkFormOptions.forEach((it: any) => {
            it.value.value = rowData[it.key] || null
          })
          commonQueryListRef.value?.showDialog()
        },
        //删除
        delete: function (rowData: any) {
          const tipMsg = `${rowData.employeeNickName}->${rowData.reportWorkDate}->${rowData.productCraftName}`
          naiveDialog.warning({
            title: '提示',
            content: `是否要【废弃】${tipMsg}报工数据？`,
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
            if (optType.value == 'update') {
              pd = updateSubmitForm.value?.generatorParams()
              postUrl = reportWorkApi.update
              method = 'POST'
            }

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
              totalsRef.totalActualSettlementPrice = res.data.totalActualSettlementPrice
              totalsRef.totalReceivableSettlementPrice = res.data.totalReceivableSettlementPrice
              totalsRef.timingWordDuration = res.data.timingWordDuration
              totalsRef.countingWordDuration = res.data.countingWordDuration
            })
            .finally(() => {})
        },
        //导出
        export: function () {
          commonQueryListRef.value?.onDownloadFile(reportWorkApi.exportDayReportWork)
        },
        //导出日期横板
        exportDateHorizontal: function () {
          commonQueryListRef.value?.onDownloadFile(
            reportWorkApi.exportDayReportWorkWithDateHorizontal
          )
        },
        //导出产品
        exportProduct: function () {
          commonQueryListRef.value?.onDownloadFile(reportWorkApi.exportProductReportWork)
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
