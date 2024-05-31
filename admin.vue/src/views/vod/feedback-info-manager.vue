<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="feedBackApi.query"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchFeedbackInfoOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :submitLoading="submitLoading"
    :onDialogConfirm="onBtnClick.onSendMessageConfrim"
    @load-data-success="onLoadDataSuccess"
  >
    <template #topSummary>
      <n-tag> 待处理:{{ sumSummary.waitCount }} </n-tag>
    </template>

    <template #showDialogContent>
      <DataForm
        ref="submitForm"
        :options="SendEmailFormOptions"
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
  import { post } from '@/api/http'
  import { feedBackApi, messageApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog, NButton } from 'naive-ui'
  import { defineComponent, h, reactive, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchFeedbackInfoOptions, SendEmailFormOptions } from './Data'
  import { renderTagByEnum } from '@/hooks/form'
  import { FeedBackInfoStatusEnum, UserDemandTypeEnum } from '@/store/types'
  export default defineComponent({
    name: 'FeedbackInfoManager',
    setup() {
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const sumSummary = reactive({
        waitCount: 0,
      })
      const sendEmailModel = reactive<any>({
        email: '',
        subject: '',
        content: '',
        feedbackId: null,
      })

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)

      //表格
      const table = useTable()
      const tableColumns = [
        table.indexColumn,
        {
          title: '名称',
          key: 'videoName',
          fixed: 'left',
          render: (rowData: any) => {
            return h(
              NButton,
              {
                type: 'info',
                tag: 'a',
                quaternary: true,
                text: true,
                rel: 'noreferrer',
                target: '_blank',
                href: rowData.originalUrl,
              },
              {
                default: () => {
                  return rowData.videoName
                },
              }
            )
          },
        },
        {
          title: '类型',
          key: 'demandType',
          render: (rowData: any) =>
            renderTagByEnum(rowData.demandType, UserDemandTypeEnum, {
              10: 'success',
              5: 'warning',
            }),
        },
        {
          title: '状态',
          key: 'feedBackInfoStatus',
          render: (rowData: any) =>
            renderTagByEnum(rowData.feedBackInfoStatus, FeedBackInfoStatusEnum, {
              10: 'success',
              0: 'warning',
              5: 'info',
            }),
        },
        {
          title: '备注',
          key: 'remark',
          width: 100,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: 'Email',
          key: 'userEmail',
        },
        {
          title: '时间',
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
            const normalActionList = [] as TableActionModel[]
            switch (rowData.feedBackInfoStatus) {
              case FeedBackInfoStatusEnum.待审核: {
                normalActionList.push({
                  label: '通过',
                  type: 'info',
                  onClick: onBtnClick.itemAudit.bind(null, rowData),
                } as TableActionModel)
                normalActionList.push({
                  label: '忽略',
                  type: 'warning',
                  onClick: onBtnClick.itemIgnore.bind(null, rowData),
                } as TableActionModel)

                normalActionList.push({
                  label: '删除',
                  type: 'error',
                  onClick: onBtnClick.itemDelete.bind(null, rowData),
                } as TableActionModel)
                break
              }
              case FeedBackInfoStatusEnum.处理中: {
                normalActionList.push({
                  label: '发通知',
                  type: 'success',
                  onClick: onBtnClick.itemSendMessage.bind(null, rowData),
                } as TableActionModel)
                normalActionList.push({
                  label: '完成',
                  type: 'success',
                  onClick: onBtnClick.itemFinished.bind(null, rowData),
                } as TableActionModel)
              }
            }

            const normalAction = useRenderAction(normalActionList)
            return normalAction
          },
        },
      ]

      const onBtnClick = {
        //删除
        itemDelete: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: `是否删除：【${rowData.videoName}】？`,
            positiveText: '确认',
            onPositiveClick: () => {
              post({
                url: feedBackApi.delete,
                data: {
                  ids: [rowData.id],
                },
              }).then((res) => {
                doRefresh()
                message.success(res.msg)
              })
            },
          })
        },
        //忽略
        itemIgnore: function (rowData: any) {
          naiveDialog.info({
            title: '提示',
            content: `是否忽略：【${rowData.videoName}】？`,
            positiveText: '确认',
            onPositiveClick: () => {
              post({
                url: feedBackApi.changeStatus,
                data: {
                  ids: [rowData.id],
                  feedBackInfoStatus: FeedBackInfoStatusEnum.忽略,
                },
              }).then((res) => {
                doRefresh()
                message.success(res.msg)
              })
            },
          })
        },
        //通过
        itemAudit: function (rowData: any) {
          post({
            url: feedBackApi.changeStatus,
            data: {
              ids: [rowData.id],
              feedBackInfoStatus: FeedBackInfoStatusEnum.处理中,
            },
          }).then((res) => {
            doRefresh()
            message.success(res.msg)
          })
        },
        //发消息
        itemSendMessage: function (rowData: any) {
          sendEmailModel.email = rowData.userEmail
          if (rowData.demandType == UserDemandTypeEnum.反馈) {
            sendEmailModel.subject = `《${rowData.videoName}》反馈结果通知`
            sendEmailModel.content = `老铁，久等了，资源已修复  多谢支持！！ ${rowData.originalUrl}
————————————————————————————
1、电脑浏览器有弹幕观看，可使用电脑跟段友嗨起来哈！
2、全站纯净无广告，请牢记域名：www.17kandy.com
`
          } else {
            sendEmailModel.subject = `《${rowData.videoName}》资源录入结果通知`
            sendEmailModel.content = `老铁，久等了，资源已录入，前往站点右上角【个人中心】->【我的收藏】->【我的录入】列表查看。  多谢支持！！
————————————————————————————
1、电脑浏览器有弹幕观看，可使用电脑跟段友嗨起来哈！
2、全站纯净无广告，请牢记域名：www.17kandy.com
`
          }

          sendEmailModel.feedbackId = rowData.id

          SendEmailFormOptions.forEach((it: any) => {
            it.value.value = sendEmailModel[it.key] || null
          })
          commonQueryListRef.value?.showDialog()
        },
        //发消息确认
        onSendMessageConfrim: function () {
          if (submitForm.value?.validator()) {
            const pd = submitForm.value?.generatorParams()
            submitLoading.value = true
            post({
              url: messageApi.sendEmail,
              data: pd,
            }).then((res) => {
              message.success(res.msg)
              //发送成功变更状态
              post({
                url: feedBackApi.changeStatus,
                data: {
                  ids: [sendEmailModel.feedbackId],
                  feedBackInfoStatus: FeedBackInfoStatusEnum.正常,
                },
              })
                .then((res) => {
                  doRefresh()
                  commonQueryListRef.value?.closeDialog()
                  message.success(res.msg)
                })
                .finally(() => {
                  submitLoading.value = false
                })
            })
          }
        },
        //直接完成
        itemFinished: function (rowData: any) {
          //发送成功变更状态
          post({
            url: feedBackApi.changeStatus,
            data: {
              ids: [rowData.id],
              feedBackInfoStatus: FeedBackInfoStatusEnum.正常,
            },
          }).then((res) => {
            doRefresh()
            message.success(res.msg)
          })
        },
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      //数据加载成功后
      function onLoadDataSuccess(pageData: any[]) {
        sumSummary.waitCount = pageData.filter((item) => item.feedBackInfoStatus === 0).length
      }

      return {
        feedBackApi,
        commonQueryListRef,
        tableColumns,
        SearchFeedbackInfoOptions,
        SendEmailFormOptions,
        submitLoading,
        submitForm,
        sumSummary,
        onBtnClick,
        onLoadDataSuccess,
      }
    },
  })
</script>
