<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="vodApi.doubanQuery"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchDouBanOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :editDialogTilte="editTitle"
    :onDialogConfirm="onBtnClick.onSubmitCreate"
    :submitLoading="submitLoading"
  >
    <template #tableToolbar>
      <n-button-group>
        <n-button type="info" size="small" @click="onBtnClick.create"> 创建豆瓣 </n-button>
        <n-button type="error" size="small" @click="onBtnClick.batchDel"> 批量删除 </n-button>
      </n-button-group>
    </template>

    <template #showDialogContent>
      <DataForm
        ref="submitForm"
        :options="CreateByDouBanFormOptions"
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
        @confirm="onBtnClick.onSubmitCreateDouBan"
        :maskClosable="false"
        :isClosable="true"
        title="创建豆瓣信息"
        :submit-loading="submitLoading"
      >
        <template #content>
          <DataForm
            ref="submitDouBanForm"
            :options="CreateDouBanFormOptions"
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
  import { post, sendDelete } from '@/api/http'
  import { vodApi } from '@/api/url'
  import { TableActionModel, useRenderAction } from '@/hooks/table'
  import { useMessage, useDialog, NImage, NButton } from 'naive-ui'
  import { defineComponent, h, ref } from 'vue'
  import { DataFormType, ModalDialogType } from '@/types/components'
  import { SearchDouBanOptions, CreateByDouBanFormOptions, CreateDouBanFormOptions } from './Data'
  import { DouBanInfoStatusEnum, VodSubtypeEnum } from '@/store/types'
  import { renderTagByEnum } from '@/hooks/form'
  import { getEpItemsByVodPlayUrls } from '@/utils'
  export default defineComponent({
    name: 'DouBanList',
    setup() {
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const modalDialogRef = ref<ModalDialogType | null>(null)

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)
      const submitDouBanForm = ref<DataFormType | null>(null)
      const editTitle = ref('新增')

      //表格
      //const table = useTable()
      const tableColumns = [
        {
          type: 'selection',
        },
        //table.indexColumn,
        // {
        //   type: 'expand',
        //   fixed: 'left',
        //   renderExpand: (rowData: any) => {
        //     return h('div', {
        //       innerHTML: `<strong>备注：</strong>${rowData.seriesRemark ?? ''} <br/>`,
        //     })
        //   },
        // },
        {
          title: '编号',
          key: 'id',
          fixed: 'left',
        },
        {
          title: '影片名',
          key: 'videoTitle',
          fixed: 'left',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
          render(row: any) {
            return h(
              NButton,
              {
                type: 'info',
                href: `//movie.douban.com/subject/${row.videoDetailId}/`,
                target: '_blank',
                tag: 'a',
                text: true,
                rel: 'noreferrer',
              },
              {
                default: () => {
                  return row.videoTitle
                },
              }
            )
          },
        },
        {
          title: '又名',
          key: 'aka',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '分类',
          key: 'subtype',
          render: (rowData: any) =>
            renderTagByEnum(rowData.subtype, VodSubtypeEnum, {
              0: 'tertiary',
              5: 'success',
              6: 'info',
              7: 'default',
              8: 'default',
              9: 'default',
            }),
        },
        {
          title: '年份',
          key: 'videoYear',
        },
        {
          title: '评分',
          key: 'videoRating',
        },
        {
          title: '人数',
          key: 'ratingsCount',
        },
        {
          title: '国家',
          key: 'videoCountries',
          width: 100,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '海报',
          key: 'videoImg',
          render: (rowData: any) => {
            return h(
              NImage,
              {
                width: 25,
                src: rowData.videoImg,
              },
              {
                placeholder: () => {
                  return '无封面'
                },
              }
            )
            // return h(NAvatar, {
            //   circle: true,
            //   size: 'small',
            //   src: firstOrDefault(
            //     rowData.weGymImgItems,
            //     (item: any) => item.imgType == ImgItemTypeEnum.封面
            //   )?.imgUrl,
            // })
          },
        },
        {
          title: '演员',
          key: 'videoCasts',
          width: 100,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '类型',
          key: 'videoGenres',
          width: 100,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '状态',
          key: 'douBanInfoStatus',
          render: (rowData: any) =>
            renderTagByEnum(rowData.douBanInfoStatus, DouBanInfoStatusEnum, {
              2: 'success',
              1: 'info',
              0: 'tertiary',
              3: 'warning',
            }),
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
                label: '录入',
                type: 'info',
                onClick: onBtnClick.createByDouban.bind(null, rowData),
              },
              {
                label: '重试',
                type: 'error',
                onClick: onBtnClick.retrySaveImg.bind(null, rowData),
              },
            ] as TableActionModel[])

            return normalAction
          },
        },
      ]

      const onBtnClick = {
        //创建
        create: function () {
          editTitle.value = '新增豆瓣信息'
          submitDouBanForm.value?.reset()
          modalDialogRef.value?.show()
        },
        //创建豆瓣信息提交
        onSubmitCreateDouBan: function () {
          if (submitDouBanForm.value?.validator()) {
            var pd = submitDouBanForm.value?.generatorParams()

            submitLoading.value = true
            const match = pd.doubanUrl.match(/\/subject\/(\d+)[\/?]/)
            const doubanId = match ? match[1] : null

            post({
              url: vodApi.doubanCreateGet + '/' + doubanId,
              data: {},
            })
              .then((res) => {
                doRefresh()
                message.success('创建成功')
              })
              .catch(console.log)
              .finally(() => {
                submitLoading.value = false
                modalDialogRef.value?.close()
              })
          }
        },
        //创建影片保存
        onSubmitCreate: function () {
          if (submitForm.value?.validator()) {
            var pd = submitForm.value?.generatorParams()

            submitLoading.value = true
            post({
              url: vodApi.createByDouBan,
              data: {
                epItems: getEpItemsByVodPlayUrls(pd.playUrls),
                ...pd,
              },
            })
              .then((res) => {
                doRefresh()
                message.success(res.msg)
              })
              .catch(console.log)
              .finally(() => {
                submitLoading.value = false
                commonQueryListRef.value?.closeDialog()
              })
          }
        },
        //根据豆瓣创建影片
        createByDouban: function (rowData: any) {
          submitForm.value?.reset()
          CreateByDouBanFormOptions.forEach((it: any) => {
            if (it.key == 'douBanInfoId') {
              it.value.value = rowData.id
            }
          })

          editTitle.value = `${rowData.videoTitle} 创建影片`
          commonQueryListRef.value?.showDialog()
        },
        //重试保存图片
        retrySaveImg: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: `是否重新匹配【${rowData.videoTitle}】的图片`,
            positiveText: '重试',
            onPositiveClick: () => {
              post({
                url: vodApi.doubanRetrySaveImg + '/' + rowData.id,
                data: {},
                method: 'PUT',
              })
                .then((res) => {
                  doRefresh()
                  message.success(res.msg)
                })
                .catch(console.log)
            },
          })
        },
        //批量删除
        batchDel: function () {
          const checkedRowInfo = commonQueryListRef.value?.getCheckedRow()
          if (checkedRowInfo.checkedRowKeys.length == 0) {
            message.warning('请至少选择一行后进行操作')
            return
          }

          naiveDialog.error({
            title: '提示',
            content: `是否要批量删除，数量：${checkedRowInfo.checkedRowKeys.length}?`,
            positiveText: '删除',
            onPositiveClick: () => {
              sendDelete({
                url: vodApi.doubanDelete,
                data: {
                  ids: checkedRowInfo.checkedRowKeys,
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
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      return {
        vodApi,
        commonQueryListRef,
        modalDialogRef,
        tableColumns,
        SearchDouBanOptions,
        CreateByDouBanFormOptions,
        CreateDouBanFormOptions,
        submitLoading,
        submitForm,
        submitDouBanForm,
        editTitle,
        onBtnClick,
      }
    },
  })
</script>
