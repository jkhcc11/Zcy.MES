<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="jobApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchJobOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :submitLoading="submitLoading"
    :editDialogTilte="editTitle"
    :onDialogConfirm="onBtnClick.onSubmitJob"
    editContentHeight="60vh"
  >
    <template #tableToolbar>
      <n-button type="info" size="small" @click="onBtnClick.create"> 创建 </n-button>
    </template>

    <template #showDialogContent>
      <DataForm
        ref="submitForm"
        :options="CreateJobFormOptions"
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
  import { jobApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog, NImage } from 'naive-ui'
  import { defineComponent, h, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchJobOptions, CreateJobFormOptions } from './Data'
  import useVodSeriesStore from '@/store/modules/vod-series'
  import { renderTagByEnum } from '@/hooks/form'
  import { ConfigHttpMethodEnum, SearchConfigStatusEnum } from '@/store/types'
  export default defineComponent({
    name: 'JobList',
    setup() {
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const useVodSeries = useVodSeriesStore()

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
              innerHTML: `<strong>Cookie：</strong>${
                rowData.cookie ?? ''
              } <br/> <strong>Post数据：</strong>${rowData.postData ?? ''} `,
            })
          },
        },
        {
          title: '请求Url',
          key: 'requestUrl',
          fixed: 'left',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '分类',
          key: 'subtype',
          render: (rowData: any) =>
            renderTagByEnum(rowData.httpMethod, ConfigHttpMethodEnum, {
              0: 'info',
              1: 'success',
            }),
        },
        {
          title: '状态',
          key: 'searchConfigStatus',
          render: (rowData: any) =>
            renderTagByEnum(rowData.searchConfigStatus, SearchConfigStatusEnum, {
              1: 'success',
              2: 'warning',
            }),
        },
        {
          title: 'Cron',
          key: 'urlCron',
        },
        {
          title: '来源',
          key: 'referer',
        },
        {
          title: '成功标识Xpath',
          key: 'msgXpath',
        },
        {
          title: '成功标识',
          key: 'successFlag',
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
                label: '编辑',
                type: 'info',
                onClick: onBtnClick.modify.bind(null, rowData),
              },
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
        //修改
        modify: function (rowData: any) {
          editTitle.value = `${rowData.requestUrl.substring(0, 25)} 编辑`

          CreateJobFormOptions.forEach((it: any) => {
            if (it.key === 'httpMethod') {
              it.value.value = rowData[it.key] || 0
            } else {
              it.value.value = rowData[it.key] || null
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
                url: jobApi.delete,
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
        onSubmitJob: function () {
          if (submitForm.value?.validator()) {
            const pd = submitForm.value?.generatorParams()
            let postUrl = jobApi.create
            let method = 'PUT'
            if (pd.id) {
              postUrl = jobApi.modify
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
              })
              .finally(() => {
                doRefresh()
                submitLoading.value = false
                commonQueryListRef.value?.closeDialog()
                useVodSeries.reset()
              })
          }
        },
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      return {
        jobApi,
        commonQueryListRef,
        tableColumns,
        SearchJobOptions,
        CreateJobFormOptions,
        submitLoading,
        submitForm,
        editTitle,
        onBtnClick,
      }
    },
  })
</script>
