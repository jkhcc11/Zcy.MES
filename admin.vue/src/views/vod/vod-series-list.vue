<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="vodSeriesApi.querySeriesList"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchVodSeriesListOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :submitLoading="submitLoading"
    :editDialogTilte="editTitle"
    :onDialogConfirm="onBtnClick.onSubmitVodSeries"
    editContentHeight="80vh"
  >
    <template #tableToolbar>
      <n-button type="success" size="small" @click="onBtnClick.addVod"> 添加影片 </n-button>
      <n-button type="warning" size="small" @click="onBtnClick.batchRemove"> 移除影片 </n-button>
    </template>

    <template #showDialogContent>
      <n-form ref="editForm" size="medium" label-placement="left">
        <n-grid :cols="2">
          <n-form-item-gi label="系列" :span="2">
            <n-select
              placeholder="请选择系列"
              filterable
              :options="useVodSeries.cachedVodSeries"
              v-model:value="vodSelectorRef.currentSeriesId"
            />
          </n-form-item-gi>
          <n-form-item-gi label="影片" path="alias" :span="2">
            <CommonTableSelector
              ref="vodTableSelectorRef"
              :getApiUrl="vodSelectorRef.url"
              :searchFormOptions="searchForm"
              :baseTableColumns="vodSelectorRef.tableColumns"
              :tableHeight="300"
            />
          </n-form-item-gi>
        </n-grid>
      </n-form>
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { post, sendDelete } from '@/api/http'
  import { vodSeriesApi, vodApi, vodMainUrl } from '@/api/url'
  import { TableActionModel, useRenderAction, useTable } from '@/hooks/table'
  import { useMessage, useDialog, NImage, NButton } from 'naive-ui'
  import { defineComponent, h, ref, onMounted, reactive } from 'vue'
  import { DataFormType, FormItem } from '@/types/components'
  import { SearchVodSeriesListOptions, CreateVodSeriesFormOptions } from './Data'
  import useVodSeriesStore from '@/store/modules/vod-series'
  import { renderInput, renderTag } from '@/hooks/form'
  import { constSystemInput } from '@/store/types'
  import { useRouter } from 'vue-router'
  export default defineComponent({
    name: 'VodSeriesList',
    setup() {
      const useVodSeries = useVodSeriesStore()
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()

      const vodTableSelectorRef = ref<any>(null)
      const router = useRouter()
      const searchForm = [
        {
          label: '关键字',
          key: 'keyWord',
          value: ref(null),
          render: (formItem: any) =>
            renderInput(formItem.value, {
              placeholder: '关键字',
              clearable: true,
            }),
        },
      ] as Array<FormItem>
      const vodSelectorRef = reactive<any>({
        url: vodApi.vodSearch,
        tableColumns: [
          {
            type: 'selection',
          },
          {
            title: '影片名',
            key: 'keyWord',
            width: 100,
            ellipsis: {
              tooltip: true,
            },
            render(row: any) {
              return h(
                NButton,
                {
                  type: 'info',
                  href: `${vodMainUrl}/Vod/Detail/${row.id}`,
                  target: '_blank',
                  tag: 'a',
                  text: true,
                },
                {
                  default: () => {
                    return row.keyWord
                  },
                }
              )
            },
          },
          {
            title: '年份',
            key: 'videoYear',
          },
          {
            title: '评分',
            key: 'videoDouBan',
          },
          {
            title: '封面',
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
            title: '人工',
            key: 'isDeal',
            render: (rowData: any) => {
              const isDeal = rowData.videoContentFeature === constSystemInput
              return renderTag(isDeal ? '是' : '否', {
                type: isDeal ? 'success' : 'default',
              })
            },
          },
          {
            title: '创建时间',
            key: 'createdTime',
            width: 80,
            ellipsis: {
              tooltip: true,
            },
          },
          {
            title: '修改时间',
            key: 'modifyTime',
            width: 80,
            ellipsis: {
              tooltip: true,
            },
          },
        ],
        currentSeriesId: null,
      })

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)
      const editTitle = ref('新增')

      //表格
      const table = useTable()
      const tableColumns = [
        {
          type: 'selection',
          fixed: 'left',
        },
        table.indexColumn,
        {
          title: '系列名',
          key: 'videoSeries.seriesName',
          fixed: 'left',
        },
        {
          title: '影片名',
          key: 'videoMain.keyWord',
          fixed: 'left',
          width: 180,
          ellipsis: {
            tooltip: true,
          },
          render(row: any) {
            return h(
              NButton,
              {
                type: 'info',
                href: `${vodMainUrl}/Vod/Detail/${row.videoMain.id}`,
                target: '_blank',
                tag: 'a',
                text: true,
              },
              {
                default: () => {
                  return row.videoMain.keyWord
                },
              }
            )
          },
        },
        {
          title: '海报',
          key: 'vodImg',
          render: (rowData: any) => {
            return h(
              NImage,
              {
                width: 25,
                src: rowData.videoMain?.videoImg,
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
          key: 'videoMain.orderBy',
        },
        {
          title: '人工',
          key: 'isDeal',
          render: (rowData: any) => {
            const isDeal = rowData.videoMain?.videoContentFeature === constSystemInput
            return renderTag(isDeal ? '是' : '否', {
              type: isDeal ? 'success' : 'default',
            })
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
          width: 120,
          render: (rowData: any) => {
            const tempArray = [
              {
                label: '移除',
                type: 'warning',
                onClick: onBtnClick.remove.bind(null, rowData),
              },
            ] as TableActionModel[]

            if (rowData.videoMain?.videoContentFeature !== constSystemInput) {
              tempArray.push({
                label: '编辑',
                type: 'info',
                onClick: onBtnClick.vodEdit.bind(null, rowData),
              } as TableActionModel)
            }

            const normalAction = useRenderAction(tempArray)

            return normalAction
          },
        },
      ]

      const onBtnClick = {
        //批量移除
        batchRemove: function () {
          const checkedRowInfo = commonQueryListRef.value?.getCheckedRow()
          if (checkedRowInfo.checkedRowKeys.length == 0) {
            message.warning('请至少选择一行后进行操作')
            return
          }

          naiveDialog.warning({
            title: '提示',
            content: '是否要移除选中影片？',
            positiveText: '移除',
            onPositiveClick: () => {
              sendDelete({
                url: vodSeriesApi.deleteVodList,
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
        //移除
        remove: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: '是否要移除此影片？',
            positiveText: '移除',
            onPositiveClick: () => {
              sendDelete({
                url: vodSeriesApi.deleteVodList,
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
        onSubmitVodSeries: function () {
          if (!vodSelectorRef.currentSeriesId) {
            message.warning('请选择系列')
            return
          }

          const postData = {
            seriesId: vodSelectorRef.currentSeriesId,
            videoMainId: vodTableSelectorRef.value?.checkedRowKeys,
          }

          submitLoading.value = true
          post({
            url: vodSeriesApi.createSeriesList,
            data: postData,
            method: 'PUT',
          })
            .then((res) => {
              message.success(res.msg)
            })
            .finally(() => {
              doRefresh()
              submitLoading.value = false
              commonQueryListRef.value?.closeDialog()
            })
        },
        //添加影片
        addVod: function () {
          commonQueryListRef.value?.showDialog()
          vodTableSelectorRef.value?.clearSelected()
        },
        //影片编辑
        vodEdit: function (rowData: any) {
          router.push('/only-page/vod-main-edit/' + rowData.videoMain?.id)
        },
      }

      //刷新
      function doRefresh() {
        commonQueryListRef.value?.doRefresh()
      }

      onMounted(async () => {
        await useVodSeries.init()
      })

      return {
        vodSeriesApi,
        commonQueryListRef,
        tableColumns,
        SearchVodSeriesListOptions,
        CreateVodSeriesFormOptions,
        submitLoading,
        submitForm,
        editTitle,
        useVodSeries,
        vodSelectorRef,
        vodTableSelectorRef,
        searchForm,
        onBtnClick,
      }
    },
  })
</script>
