<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="pageSearchApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchPageSearchConfigOptions"
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
        :options="CreatePageSearchConfigFormOptions"
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
        ref="modalDialogRef"
        :maskClosable="false"
        :isClosable="true"
        :title="editTitle"
        contentHeight="60vh"
        :submit-loading="submitLoading"
        @confirm="onBtnClick.onTestConfirm"
      >
        <template #content>
          <DataForm
            ref="saveSourceForm"
            :options="TestSearchPageSearchConfigOptions"
            :form-config="{
              labelWidth: 100,
              size: 'medium',
              labelAlign: 'right',
            }"
            preset="grid-item"
          />
        </template>
      </ModalDialog>
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { post, get, sendDelete } from '@/api/http'
  import { pageSearchApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog } from 'naive-ui'
  import { defineComponent, ref } from 'vue'
  import { DataFormType, ModalDialogType } from '@/types/components'
  import {
    SearchPageSearchConfigOptions,
    CreatePageSearchConfigFormOptions,
    TestSearchPageSearchConfigOptions,
  } from './Data'
  import useVodSeriesStore from '@/store/modules/vod-series'
  import { renderTagByEnum } from '@/hooks/form'
  import { ConfigHttpMethodEnum, SearchConfigStatusEnum } from '@/store/types'
  import { stringify } from 'qs'
  export default defineComponent({
    name: 'PageSearchConfigList',
    setup() {
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const useVodSeries = useVodSeriesStore()

      const modalDialogRef = ref<ModalDialogType | null>(null)
      const saveSourceForm = ref<DataFormType | null>(null)

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)
      const editTitle = ref('新增')

      //表格
      const table = useTable()
      const tableColumns = [
        table.indexColumn,
        // {
        //   type: 'expand',
        //   fixed: 'left',
        //   renderExpand: (rowData: any) => {
        //     return h('div', {
        //       innerHTML: `<strong>Cookie：</strong>${
        //         rowData.cookie ?? ''
        //       } <br/> <strong>Post数据：</strong>${rowData.postData ?? ''} `,
        //     })
        //   },
        // },
        {
          title: '站点名',
          key: 'hostName',
          fixed: 'left',
        },
        {
          title: 'BaseHost',
          key: 'baseHost',
          fixed: 'left',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '备用Host',
          key: 'otherHost',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '请求方式',
          key: 'configHttpMethod',
          render: (rowData: any) =>
            renderTagByEnum(rowData.configHttpMethod, ConfigHttpMethodEnum, {
              0: 'info',
              1: 'success',
            }),
        },
        {
          title: '处理类',
          key: 'serviceFullName',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '状态',
          key: 'searchConfigStatus',
          render: (rowData: any) =>
            renderTagByEnum(rowData.searchConfigStatus, SearchConfigStatusEnum, {
              1: 'success',
              2: 'error',
            }),
        },
        {
          title: '搜索路径',
          key: 'searchPath',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: 'Post数据',
          key: 'searchData',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '搜索Xpath',
          key: 'searchXpath',
          width: 110,
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
          fixed: 'left',
          width: 140,
          render: (rowData: any) => {
            const normalAction = useRenderAction([
              {
                label: '编辑',
                type: 'info',
                onClick: onBtnClick.modify.bind(null, rowData),
              },
              {
                label: '复制',
                type: 'warning',
                onClick: onBtnClick.copy.bind(null, rowData),
              },
              {
                label: '禁用',
                type: 'error',
                onClick: onBtnClick.ban.bind(null, rowData),
              },
              {
                label: '测试',
                type: 'success',
                onClick: onBtnClick.test.bind(null, rowData),
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
          editTitle.value = `${rowData.baseHost} 编辑`

          CreatePageSearchConfigFormOptions.forEach((it: any) => {
            if (it.key === 'configId') {
              it.value.value = rowData['id'] || null
            } else if (it.key === 'configHttpMethod') {
              it.value.value = rowData[it.key] || 0
            } else {
              it.value.value = rowData[it.key] || null
            }
          })
          commonQueryListRef.value?.showDialog()
        },
        //复制
        copy: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: '是否要复制【' + rowData.hostName + '】此数据？',
            positiveText: '确认',
            onPositiveClick: () => {
              get({
                url: pageSearchApi.copy + '/' + rowData.id,
              })
                .then((res) => {
                  doRefresh()
                  message.success(res.msg)
                })
                .catch(console.log)
            },
          })
        },
        //禁用
        ban: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: '是否要禁用【' + rowData.hostName + '】此数据？',
            positiveText: '禁用',
            onPositiveClick: () => {
              sendDelete({
                url: pageSearchApi.ban + '/' + rowData.id,
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
            let postUrl = pageSearchApi.create
            if (pd.configId) {
              postUrl = pageSearchApi.modify
            }
            submitLoading.value = true
            post({
              url: postUrl,
              data: pd,
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
        //测试
        test: function (rowData: any) {
          editTitle.value = `${rowData.hostName} 测试规则`
          TestSearchPageSearchConfigOptions.forEach((it: any) => {
            if (it.key === 'configId') {
              it.value.value = rowData.id
            } else {
              it.value.value = null
            }
          })

          modalDialogRef.value?.show()
        },
        //测试提交
        onTestConfirm: function () {
          if (!saveSourceForm.value?.validator()) {
            return
          }

          const pd = saveSourceForm.value?.generatorParams()
          submitLoading.value = true
          pd.result = null
          get({
            url: pageSearchApi.test,
            data: pd,
          })
            .then((res) => {
              message.success('请求成功')

              TestSearchPageSearchConfigOptions.forEach((it: any) => {
                if (it.key === 'result') {
                  it.value.value = JSON.stringify(res.data)
                } else {
                  switch (it.key) {
                    case 'result1': {
                      it.value.value = res.data.results
                        .map((it) => {
                          return `第${it.resultName}集$${it.resultUrl}`
                        })
                        .join('\r\n')
                      break
                    }
                    case 'resultImg': {
                      it.value.value = res.data.imgUrl
                      break
                    }
                    case 'resultName': {
                      it.value.value = res.data.resultName
                      break
                    }
                  }
                }
              })
            })
            .finally(() => {
              submitLoading.value = false
            })
        },
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      return {
        pageSearchApi,
        commonQueryListRef,
        tableColumns,
        SearchPageSearchConfigOptions,
        CreatePageSearchConfigFormOptions,
        TestSearchPageSearchConfigOptions,
        submitLoading,
        submitForm,
        editTitle,
        modalDialogRef,
        saveSourceForm,
        onBtnClick,
      }
    },
  })
</script>
