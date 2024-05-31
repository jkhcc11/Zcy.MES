<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="vodSeriesApi.querySeries"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchVodSeriesOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :submitLoading="submitLoading"
    :editDialogTilte="editTitle"
    :onDialogConfirm="onBtnClick.onSubmitVodSeries"
    editContentHeight="50vh"
  >
    <template #tableToolbar>
      <n-button type="info" size="small" @click="onBtnClick.create"> 创建 </n-button>
    </template>

    <template #showDialogContent>
      <DataForm
        ref="submitForm"
        :options="CreateVodSeriesFormOptions"
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
  import { vodSeriesApi } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog, NImage } from 'naive-ui'
  import { defineComponent, h, ref } from 'vue'
  import { DataFormType } from '@/types/components'
  import { SearchVodSeriesOptions, CreateVodSeriesFormOptions } from './Data'
  import useVodSeriesStore from '@/store/modules/vod-series'
  export default defineComponent({
    name: 'VodSeries',
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
              innerHTML: `<strong>备注：</strong>${rowData.seriesRemark ?? ''} <br/>`,
            })
          },
        },
        {
          title: '系列名',
          key: 'seriesName',
          fixed: 'left',
        },
        {
          title: '系列图',
          key: 'seriesImg',
          render: (rowData: any) => {
            return h(
              NImage,
              {
                width: 25,
                src: rowData.seriesImg,
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
          title: '排序',
          key: 'orderBy',
        },
        {
          title: '备注',
          key: 'seriesRemark',
          width: 100,
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
          editTitle.value = `${rowData.seriesName} 编辑`

          CreateVodSeriesFormOptions.forEach((it: any) => {
            it.value.value = rowData[it.key] || null
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
                url: vodSeriesApi.delete + '/' + rowData.id,
                data: {},
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
        onSubmitVodSeries: function () {
          if (submitForm.value?.validator()) {
            const pd = submitForm.value?.generatorParams()
            let postUrl = vodSeriesApi.create
            let method = 'PUT'
            if (pd.id) {
              pd.seriesId = pd.id
              postUrl = vodSeriesApi.modify
              method = 'PATCH'
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
        vodSeriesApi,
        commonQueryListRef,
        tableColumns,
        SearchVodSeriesOptions,
        CreateVodSeriesFormOptions,
        submitLoading,
        submitForm,
        editTitle,
        onBtnClick,
      }
    },
  })
</script>
