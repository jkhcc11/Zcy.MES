<template>
  <CommonQueryList
    ref="commonQueryListRef"
    :queryApi="vodApi.vodSearchAdmin"
    :tableColumns="tableColumns"
    :isShowQuery="true"
    :searchFormOptions="SearchVodMainOptions"
    :defaultPageSize="20"
    :scrollX="1000"
    :editDialogTilte="editTitle"
    :onDialogConfirm="onBtnClick.onSubmitEpSave"
    :submitLoading="submitLoading"
    editContentHeight="60vh"
  >
    <template #tableToolbar>
      <n-button-group>
        <n-tooltip trigger="hover">
          <template #trigger>
            <n-button type="warning" size="small" @click="onBtnClick.batchDown">
              批量上|下架
            </n-button>
          </template>
          批量请谨慎操作 <br />
          正常会变为 下架 <br />下架会变为 正常
        </n-tooltip>

        <n-button type="error" size="small" @click="onBtnClick.batchDel"> 批量删除 </n-button>
      </n-button-group>

      <n-button-group>
        <n-tooltip trigger="hover">
          <template #trigger>
            <n-button ghost size="small" @click="onBtnClick.batchMatchDouBanInfo">
              一键匹配豆瓣
            </n-button>
          </template>
          未匹配豆瓣信息的，才会自动匹配
        </n-tooltip>
        <n-tooltip trigger="hover">
          <template #trigger>
            <n-button ghost size="small"> 检查海报 </n-button>
          </template>
          批量检查海报，图片有问题的会自动匹配
        </n-tooltip>
      </n-button-group>
    </template>

    <template #showDialogContent>
      <VodMainEpisodeEdit>
        <template #headTip>
          <n-gradient-text type="error">
            注：同名剧集名会直接覆盖,下载分组不支持!!!
          </n-gradient-text>
        </template>
      </VodMainEpisodeEdit>
    </template>

    <template #hideElement>
      <ModalDialog
        ref="modalDialogRef"
        :maskClosable="false"
        :isClosable="true"
        :title="editTitle"
        contentHeight="60vh"
        :submit-loading="submitLoading"
        @confirm="onBtnClick.onSaveDouBanInfo"
      >
        <template #content>
          <CommonTableSelector
            ref="doubanKeyWordSearchRef"
            :getApiUrl="vodApi.doubanKeyWord"
            :searchFormOptions="SearchKeyWordDouBanOptions"
            :baseTableColumns="doubanKeyWordColumns"
            :tableHeight="300"
            rowKey="subjectId"
            :defaultFilter="{
              keyWord: doubanKeyWordRef.currentVodRowData.keyWord,
            }"
          />
        </template>
      </ModalDialog>
    </template>
  </CommonQueryList>
</template>

<script lang="ts">
  import { post, sendDelete } from '@/api/http'
  import { vodApi, vodMainUrl, taskApi } from '@/api/url'
  import { useTable } from '@/hooks/table'
  import { useMessage, useDialog, NImage, NDropdown, NButton } from 'naive-ui'
  import { defineComponent, h, reactive, ref } from 'vue'
  import { DataFormType, ModalDialogType } from '@/types/components'
  import { SearchVodMainOptions, SearchKeyWordDouBanOptions } from './Data'
  import {
    ConvertTaskTypeEnum,
    SourceLinkTypeEnum,
    VideoMainStatusEnum,
    VodSubtypeEnum,
    constNewZyFlag,
    constSystemInput,
  } from '@/store/types'
  import { renderTag, renderTagByEnum } from '@/hooks/form'
  import { useRouter } from 'vue-router'
  import useVodMainEditEpUpdateStore from '@/store/modules/vod-main-edit-ep-update'
  import VodMainEpisodeEdit from './components/VodMainEpisodeEdit.vue'
  import useVodSearchStore from '@/store/modules/vod-search'
  export default defineComponent({
    name: 'VodMain',
    components: { VodMainEpisodeEdit },
    setup() {
      const defaultGift = 0.8
      const message = useMessage()
      const commonQueryListRef = ref()
      const naiveDialog = useDialog()
      const router = useRouter()
      const useVodMainEditEpUpdate = useVodMainEditEpUpdateStore()
      const useVodSearch = useVodSearchStore()
      const modalDialogRef = ref<ModalDialogType | null>(null)

      //提交
      const submitLoading = ref(false)
      const submitForm = ref<DataFormType | null>(null)
      const editTitle = ref('新增')
      const currentEditRowRef = ref<any>(null)

      //豆瓣搜索
      const doubanKeyWordSearchRef = ref<any>(null)
      const doubanKeyWordRef = reactive<any>({
        currentVodRowData: null,
      })
      const doubanKeyWordColumns = [
        {
          type: 'selection',
          multiple: false,
        },
        {
          title: '豆瓣名',
          key: 'title',
          width: 200,
          ellipsis: {
            tooltip: true,
          },
          render(row: any) {
            return h(
              NButton,
              {
                type: 'info',
                href: row.douBanUrl,
                target: '_blank',
                tag: 'a',
                text: true,
                rel: 'noreferrer',
              },
              {
                default: () => {
                  return row.title
                },
              }
            )
          },
        },
        {
          title: '年份',
          key: 'year',
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
          },
        },
        {
          title: '豆瓣主题Id',
          key: 'subjectId',
          width: 80,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '子标题',
          key: 'subtitle',
          width: 80,
          ellipsis: {
            tooltip: true,
          },
        },
      ]

      //表格
      const table = useTable()
      const tableColumns = [
        {
          type: 'selection',
        },
        table.indexColumn,
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
          title: '影片名',
          key: 'keyWord',
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
          title: '分类',
          key: 'subtype',
          width: 60,
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.subtype,
              VodSubtypeEnum,
              {
                0: 'tertiary',
                5: 'success',
                6: 'info',
                7: 'default',
                8: 'default',
                9: 'default',
              },
              {
                size: 'small',
              }
            ),
        },
        {
          title: '豆瓣',
          key: 'videoDouBan',
          width: 45,
          render(row: any) {
            return h(
              NButton,
              {
                type: 'info',
                href: `https://www.douban.com/search?cat=1002&q=${row.keyWord}`,
                target: '_blank',
                tag: 'a',
                text: true,
                rel: 'noreferrer',
              },
              {
                default: () => {
                  return row.videoDouBan
                },
              }
            )
          },
        },
        {
          title: '年份',
          key: 'videoYear',
          width: 50,
        },
        {
          title: '国家',
          key: 'videoMainInfo.videoCountries',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '海报',
          key: 'seriesImg',
          width: 45,
          render: (rowData: any) => {
            return h(
              NImage,
              {
                width: 25,
                src: rowData.videoImg,
              },
              {
                placeholder: () => {
                  return rowData.keyWord.substring(0, 2)
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
          title: '导演',
          key: 'videoMainInfo.videoDirectors',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '类型',
          key: 'videoMainInfo.videoGenres',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
        },
        {
          title: '排序',
          key: 'orderBy',
          width: 45,
        },
        {
          title: '状态',
          key: 'videoMainStatus',
          width: 50,
          render: (rowData: any) =>
            renderTagByEnum(
              rowData.videoMainStatus,
              VideoMainStatusEnum,
              {
                0: 'success',
                1: 'info',
                2: 'tertiary',
                10: 'warning',
              },
              {
                size: 'small',
              }
            ),
        },
        {
          title: '完结',
          key: 'isEnd',
          render: (rowData: any) => {
            return renderTag(rowData.isEnd ? '是' : '否', {
              type: rowData.isEnd ? 'success' : 'default',
              size: 'small',
            })
          },
        },
        {
          title: '豆瓣',
          key: 'isMatchInfo',
          render: (rowData: any) => {
            return renderTag(rowData.isMatchInfo ? '是' : '否', {
              type: rowData.isMatchInfo ? 'success' : 'default',
              size: 'small',
            })
          },
        },
        {
          title: '人工',
          key: 'isDeal',
          render: (rowData: any) => {
            const isDeal = rowData.videoContentFeature === constSystemInput
            return renderTag(isDeal ? '是' : '否', {
              type: isDeal ? 'success' : 'default',
              size: 'small',
            })
          },
        },
        {
          title: '新Zy',
          key: 'isNewZy',
          render: (rowData: any) => {
            const isDeal = rowData.sourceUrl && rowData.sourceUrl.indexOf(constNewZyFlag) != -1
            return renderTag(isDeal ? '是' : '否', {
              type: isDeal ? 'success' : 'default',
              size: 'small',
            })
          },
        },
        {
          title: '最后更新',
          width: 110,
          ellipsis: {
            tooltip: true,
          },
          render: (rowData: any) => {
            if (rowData.modifyTime) {
              return rowData.modifyTime
            }
            return rowData.createdTime
          },
        },
        {
          title: '操作',
          key: 'actions',
          fixed: 'right',
          width: 120,
          render: (rowData: any) => {
            const dropdownOptions: Array<{
              label: string
              key: string
            }> = [
              {
                //直接前端处理格式为json 然后json还可以编辑
                label: '剧集',
                key: 'videoEpisode',
              },
              {
                label: '删除',
                key: 'delete',
              },
              {
                label: '找资源',
                key: 'vodSearch',
              },
            ]
            if (rowData.videoMainStatus != VideoMainStatusEnum.下架) {
              dropdownOptions.push({
                label: '下架',
                key: 'down',
              })
            } else {
              dropdownOptions.push({
                label: '上架',
                key: 'down',
              })
            }

            if (rowData.isMatchInfo == false) {
              dropdownOptions.push({
                label: '匹配豆瓣',
                key: 'matchDouban',
              })
            } else if (rowData.videoContentFeature !== constSystemInput) {
              dropdownOptions.push({
                label: '创建任务',
                key: 'createTask',
              })
            }

            if (rowData.videoContentFeature !== constSystemInput) {
              dropdownOptions.push({
                label: '匹配资源',
                key: 'matchSource',
              })
            }

            var clickArray = [
              h(
                NButton,
                {
                  type: 'info',
                  size: 'tiny',
                  round: true,
                  //   style: 'margin-right: 10px;',
                  onClick: function () {
                    router.push('/only-page/vod-main-edit/' + rowData.id)
                    //message.info('编辑')
                  },
                },
                {
                  default: () => {
                    return '编辑'
                  },
                }
              ),
              h(
                NDropdown,
                {
                  trigger: 'click',
                  showArrow: true,
                  size: 'huge',
                  options: dropdownOptions,
                  onSelect: function (key: string | number) {
                    switch (key) {
                      case 'videoEpisode': {
                        useVodMainEditEpUpdate.reset()
                        currentEditRowRef.value = rowData
                        editTitle.value = `${rowData.keyWord} 剧集编辑`
                        commonQueryListRef.value?.showDialog()
                        break
                      }
                      case 'down': {
                        onBtnClick.down(rowData)
                        break
                      }
                      case 'delete': {
                        onBtnClick.delete(rowData)
                        break
                      }
                      case 'matchDouban': {
                        doubanKeyWordRef.currentVodRowData = rowData
                        editTitle.value = `【${rowData.keyWord}】 匹配豆瓣信息 年份：${rowData.videoYear}`
                        modalDialogRef.value?.show()
                        doubanKeyWordSearchRef.value?.clearSelected()
                        break
                      }
                      case 'createTask': {
                        onBtnClick.createTask(rowData)
                        break
                      }
                      case 'vodSearch': {
                        useVodSearch.setKeyWord(rowData.keyWord)
                        router.push('/resource-manager/search-vod')
                        break
                      }
                    }
                  },
                },
                {
                  default: () => {
                    return h(
                      NButton,
                      {
                        type: 'warning',
                        size: 'tiny', // 按钮大小
                        round: true, // 圆角
                      },
                      {
                        default: () => '更多', // 触发器按钮显示的文本
                      }
                    )
                  },
                }
              ),
            ]

            return clickArray
          },
        },
      ]

      const onBtnClick = {
        //下架
        down: function (rowData: any) {
          let tipMsg = `是否要下架【${rowData.keyWord}】?`
          if (rowData.videoMainStatus == VideoMainStatusEnum.下架) {
            tipMsg = `是否要上架【${rowData.keyWord}】?`
          }

          naiveDialog.warning({
            title: '提示',
            content: tipMsg,
            positiveText: '确认',
            onPositiveClick: () => {
              post({
                url: vodApi.vodUpAndDown + '/' + rowData.id,
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
        //删除
        delete: function (rowData: any) {
          naiveDialog.error({
            title: '提示',
            content: `是否要删除【${rowData.keyWord}】?`,
            positiveText: '删除',
            onPositiveClick: () => {
              sendDelete({
                url: vodApi.vodDelete,
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
        //批量下架
        batchDown: function () {
          const checkedRowInfo = commonQueryListRef.value?.getCheckedRow()
          if (checkedRowInfo.checkedRowKeys.length == 0) {
            message.warning('请至少选择一行后进行操作')
            return
          }

          naiveDialog.warning({
            title: '提示',
            content: `是否要批量下架，数量：${checkedRowInfo.checkedRowKeys.length}?`,
            positiveText: '下架',
            onPositiveClick: () => {
              const allPromises = checkedRowInfo.checkedRowKeys.map((id: any) => {
                return post({
                  url: vodApi.vodUpAndDown + '/' + id,
                })
              })

              Promise.all(allPromises)
                .then((results) => {
                  // 所有请求都成功完成时才会执行这里的代码
                  console.log('所有请求都成功了', results)
                  message.success('操作成功')
                })
                .catch((error) => {
                  // 如果任何一个请求失败了，则会执行这里的代码
                  console.error('请求失败', error)
                  message.error('部分操作失败，请等待刷新')
                })
                .finally(() => {
                  doRefresh()
                })
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
                url: vodApi.vodDelete,
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
        //保存剧集
        onSubmitEpSave: function () {
          if (
            useVodMainEditEpUpdate.epItemsWithDown.length <= 0 &&
            useVodMainEditEpUpdate.epItemsWithPlay.length <= 0
          ) {
            message.error('请添加剧集或下载地址')
            return
          }

          submitLoading.value = true
          post({
            url: vodApi.vodEpisodeCreate,
            data: {
              epItems: useVodMainEditEpUpdate.epItemsWithPlay,
              mainId: currentEditRowRef.value.id,
            },
          })
            .then((res: any) => {
              message.success(res.msg)
            })
            .finally(() => {
              submitLoading.value = false
              useVodMainEditEpUpdate.reset()
              commonQueryListRef.value?.closeDialog()
            })
        },
        //批量匹配豆瓣信息
        batchMatchDouBanInfo: function () {
          const checkedRowInfo = commonQueryListRef.value?.getCheckedRow()
          if (checkedRowInfo.checkedRowKeys.length == 0) {
            message.warning('请至少选择一行后进行操作')
            return
          }

          const pd = checkedRowInfo.checkedRows
            .filter((item: any) => item.isMatchInfo == false)
            .map((item: any) => {
              return {
                vodTitle: item.keyWord,
                keyId: item.id,
                vodYear: item.videoYear,
              }
            })
          if (pd.length == 0) {
            message.warning('无有效数据，请选择未匹配豆瓣信息的')
            return
          }

          naiveDialog.warning({
            title: '提示',
            content: `批量匹配可能耗时，请确认。数量：${checkedRowInfo.checkedRowKeys.length}?`,
            positiveText: '确认',
            onPositiveClick: () => {
              post({
                url: vodApi.vodBatchDouBanInfo,
                data: pd,
              })
                .then((res) => {
                  message.success(res.msg)
                })
                .finally(() => {
                  doRefresh()
                })
            },
          })
        },
        //保存豆瓣信息
        onSaveDouBanInfo: function () {
          if (doubanKeyWordSearchRef.value?.checkedRowKeys.length <= 0) {
            message.warning('请选择')
            return
          }

          //1、根据subject存豆瓣信息
          //2、匹配豆瓣信息
          const firstRow = doubanKeyWordSearchRef.value?.checkedRows[0]
          naiveDialog.info({
            title: '提示',
            content: `豆瓣名称：${firstRow.title}，年份：${firstRow.year} 主题Id:${firstRow.subjectId} 请确认?`,
            positiveText: '确认',
            onPositiveClick: () => {
              submitLoading.value = true
              post({
                url: vodApi.doubanCreateGet + '/' + firstRow.subjectId,
                data: {},
              }).then((res) => {
                post({
                  url: vodApi.vodmatchDouBanInfo,
                  data: {
                    keyId: doubanKeyWordRef.currentVodRowData.id,
                    douBanId: res.data.id,
                  },
                })
                  .then((res) => {
                    message.success(res.msg)
                  })
                  .finally(() => {
                    submitLoading.value = false
                    modalDialogRef.value?.close()
                    doRefresh()
                  })
              })
            },
          })
          //message.info(doubanKeyWordSearchRef.value?.checkedRowKeys[0])
        },
        //创建任务
        createTask: function (rowData: any) {
          naiveDialog.warning({
            title: '提示',
            content: `是否为【${rowData.keyWord}】创建“找资源”任务?`,
            positiveText: '确认',
            onPositiveClick: () => {
              post({
                url: taskApi.create,
                data: {
                  taskName: `${rowData.videoYear}${rowData.keyWord}`,
                  taskType: ConvertTaskTypeEnum.找资源,
                  giftPoints: defaultGift,
                  sourceLinkType: SourceLinkTypeEnum.百度云,
                  sourceLink: rowData.videoInfoUrl,
                  taskRemark:
                    (rowData.videoMainInfo.videoCountries ?? '').includes('中国') &&
                    rowData.subtype == VodSubtypeEnum.电视剧
                      ? '只要阿里云盘'
                      : '',
                },
                method: 'PUT',
              })
                .then((res) => {
                  doRefresh()
                  message.success(res.msg)
                })
                .finally(() => {
                  submitLoading.value = false
                })
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
        modalDialogRef,
        commonQueryListRef,
        tableColumns,
        SearchVodMainOptions,
        SearchKeyWordDouBanOptions,
        submitLoading,
        submitForm,
        editTitle,
        doubanKeyWordColumns,
        doubanKeyWordRef,
        doubanKeyWordSearchRef,
        onBtnClick,
      }
    },
  })
</script>
