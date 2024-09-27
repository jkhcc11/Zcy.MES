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

        <n-button-group size="small">
          <DownloadButton @export="onBtnClick.export"> 员工汇总 </DownloadButton>
          <DownloadButton @export="onBtnClick.exportDateHorizontal"> 员工汇总样式1 </DownloadButton>
          <DownloadButton @export="onBtnClick.exportProduct"> 产品汇总 </DownloadButton>
        </n-button-group>
        <n-button-group size="small">
          <n-button strong secondary round type="info" @click="onBtnClick.printPreview">
            打印员工报工
          </n-button>
        </n-button-group>
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

    <template #hideElement>
      <ModalDialog
        ref="printDialogRef"
        @confirm="onBtnClick.onPrint"
        :maskClosable="false"
        :isClosable="true"
        title="打印数据"
        :submit-loading="submitLoading"
        contentHeight="60vh"
        contentWidth="80vh"
      >
        <template #content>
          <div ref="zcyPrintRef">
            <n-skeleton v-if="loadingPrintData" :width="146" :sharp="false" size="medium" />
            <n-table
              v-else
              :bordered="true"
              :single-line="false"
              size="small"
              class="zcy_print_table"
              v-for="(employeeItem, index) of printDataRef.data"
              :key="index"
            >
              <tbody>
                <tr>
                  <th :colspan="33" style="text-align: center">
                    表格序号：{{ index + 1 }}----{{ employeeItem.headTitle }}
                  </th>
                </tr>
                <tr>
                  <th>工艺/日期</th>
                  <th v-for="titleItem in employeeItem.tableTitleArray" :key="titleItem">{{
                    titleItem
                  }}</th>
                </tr>
                <tr v-for="rowItem in employeeItem.tableRowsItems" :key="rowItem">
                  <td v-for="colItem in rowItem.columnItems" :key="colItem">{{ colItem }}</td>
                </tr></tbody
              >
              <tfoot v-if="index != 0">
                <n-divider dashed> 裁剪线 </n-divider>
              </tfoot>
            </n-table>
          </div>
        </template>
      </ModalDialog>
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { get, post, sendDelete } from '@/api/http'
  import { reportWorkApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog, NTag } from 'naive-ui'
  import { defineComponent, h, onMounted, reactive, ref } from 'vue'
  import { DataFormType, ModalDialogType } from '@/types/components'
  import {
    SearchReportWorkOptions,
    CreateReportWorkFormOptions,
    UpdateReportWorkFormOptions,
  } from './Data'
  import { renderTagByEnum } from '@/hooks/form'
  import { BillingTypeEnum, ProductTypeEnum, PublicStatusEnum } from '@/store/types'
  import useProductCacheStore from '@/store/modules/product'
  import useCompanyCacheStore from '@/store/modules/company'
  import { useRouter } from 'vue-router'
  import { VuePrintNext } from 'vue-print-next'
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

      const zcyPrintRef = ref<HTMLElement | undefined>(undefined)
      const printDialogRef = ref<ModalDialogType | null>(null)

      const loadingPrintData = ref(false)
      const printDataRef = reactive({
        data: [] as any[],
      })

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)
      const updateSubmitForm = ref<DataFormType | null>(null)
      const editTitle = ref('新增')
      const optType = ref<'create' | 'update' | 'edit'>('create')

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
          title: '备注',
          key: 'remark',
          width: 80,
          ellipsis: {
            tooltip: true,
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
            } else if (rowData.reportWorkStatus == PublicStatusEnum.待处理) {
              tempArray.push({
                label: '通过',
                type: 'success',
                onClick: onBtnClick.approved.bind(null, rowData),
              } as TableActionModel)

              tempArray.push({
                label: '驳回',
                type: 'warning',
                onClick: onBtnClick.reject.bind(null, rowData),
              } as TableActionModel)
            } else {
              tempArray.push({
                label: '编辑',
                type: 'info',
                onClick: onBtnClick.edit.bind(null, rowData),
              } as TableActionModel)
              tempArray.push({
                label: '废弃',
                type: 'error',
                onClick: onBtnClick.ban.bind(null, rowData),
              } as TableActionModel)
            }

            const normalAction = useRenderAction(tempArray)
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
        //修改
        edit: function (rowData: any) {
          optType.value = 'edit'
          editTitle.value = `【${rowData.employeeNickName} 】报工编辑`

          CreateReportWorkFormOptions.forEach((it: any) => {
            if (it.key === 'productProcessId') {
              it.value.value = `${rowData['productName']}/${rowData['productCraftName']}`
            } else {
              it.value.value = rowData[it.key] || null
            }

            if (
              it.key === 'employeeId' ||
              it.key === 'reportWorkDate' ||
              it.key === 'productProcessId'
            ) {
              it.disabled.value = true
            }
          })

          commonQueryListRef.value?.showDialog()
        },
        //驳回
        reject: function (rowData: any) {
          const tipMsg = `${rowData.employeeNickName}->${rowData.reportWorkDate}->${rowData.productCraftName}`
          naiveDialog.warning({
            title: '提示',
            content: `是否要【驳回】${tipMsg}报工数据？`,
            positiveText: '确定',
            onPositiveClick: () => {
              post({
                url: reportWorkApi.reject + '/' + rowData.id,
              })
                .then((res) => {
                  doRefresh()
                  message.success(res.msg)
                })
                .catch(console.log)
            },
          })
        },
        //通过
        approved: function (rowData: any) {
          const tipMsg = `${rowData.employeeNickName}->${rowData.reportWorkDate}->${rowData.productCraftName}`
          naiveDialog.warning({
            title: '提示',
            content: `是否要【通过】${tipMsg}报工数据？`,
            positiveText: '确定',
            onPositiveClick: () => {
              post({
                url: reportWorkApi.approved + '/' + rowData.id,
              })
                .then((res) => {
                  doRefresh()
                  message.success(res.msg)
                })
                .catch(console.log)
            },
          })
        },
        //废弃
        ban: function (rowData: any) {
          const tipMsg = `${rowData.employeeNickName}->${rowData.reportWorkDate}->${rowData.productCraftName}`
          naiveDialog.warning({
            title: '提示',
            content: `是否要【废弃】${tipMsg}报工数据？`,
            positiveText: '确认',
            onPositiveClick: () => {
              sendDelete({
                url: reportWorkApi.ban + '/' + rowData.id,
              })
                .then((res) => {
                  doRefresh()
                  message.success(res.msg)
                })
                .catch(console.log)
            },
          })
        },
        //删除
        delete: function (rowData: any) {
          const tipMsg = `${rowData.employeeNickName}->${rowData.reportWorkDate}->${rowData.productCraftName}`
          naiveDialog.warning({
            title: '提示',
            content: `是否要【删除】${tipMsg}报工数据？`,
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
            if (optType.value === 'edit') {
              postUrl = reportWorkApi.edit
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
        //打印预览
        printPreview: function () {
          loadingPrintData.value = true
          const params = commonQueryListRef.value?.getQueryParams()
          get({
            url: reportWorkApi.queryReportWithPrint,
            data: params,
          })
            .then((res) => {
              printDataRef.data = res.data
            })
            .finally(() => {
              loadingPrintData.value = false
            })
          printDialogRef.value?.show()
        },
        //打印
        onPrint: function () {
          // console.log('zcyPrintRef.value', zcyPrintRef.value)
          //todo:这里有个bug 如果不加preview: true时，打印无数据
          new VuePrintNext({
            el: zcyPrintRef.value,
            preview: true,
            popTitle: '员工报工记录',
          })
          //new VuePrintNext({ el: zcyPrintRef.value })
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
        printDialogRef,
        zcyPrintRef,
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
        loadingPrintData,
        printDataRef,
        onBtnClick,
      }
    },
  })
</script>

<style lang="scss" scoped>
  .zcy_print_table {
    border-top: 1px solid #000 !important;
    border-left: 1px solid #000 !important;
  }
  .zcy_print_table th {
    font-weight: bolder;
  }

  .zcy_print_table th,
  .zcy_print_table td {
    border-bottom: 0.5px solid #000 !important;
    border-right: 0.5px solid #000 !important;
    background: none !important;
  }
</style>
