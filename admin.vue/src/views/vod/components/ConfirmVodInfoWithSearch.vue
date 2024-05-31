<template>
  <div class="main-container">
    <n-grid x-gap="12" :cols="2">
      <n-gi>
        <n-card :content-style="{ padding: '10px' }" title="影片信息">
          <n-space justify="space-between" size="large">
            <n-gradient-text type="warning">
              匹配影片数量: {{ vodDataList.length }}
            </n-gradient-text>

            <n-button size="tiny" type="tertiary" @click="onClearVod">清除选中</n-button>
          </n-space>

          <n-data-table
            size="small"
            :loading="tableLoading"
            :data="vodDataList"
            :columns="vodTableColumns"
            :style="{ height: `40vh` }"
            :flex-height="true"
            :row-key="rowKey"
            @update:checked-row-keys="onVodRowCheck"
            :checked-row-keys="vodCheckedRowKeys"
          />
        </n-card>
      </n-gi>
      <n-gi>
        <n-card :content-style="{ padding: '10px' }" title="剧集信息">
          <n-gradient-text type="error">
            剧集数量: {{ useVodUpdate.epItems.length }}
          </n-gradient-text>
          <n-data-table
            size="small"
            :data="useVodUpdate.epItems"
            :columns="epTableColumns"
            :style="{ height: `40vh` }"
            :flex-height="true"
            :row-key="rowKey"
          />
        </n-card>
      </n-gi>
    </n-grid>
  </div>
</template>

<script lang="ts">
  import { get, post, sendDelete } from '@/api/http'
  import { vodApi } from '@/api/url'
  import { useRowKey } from '@/hooks/table'
  import { useDialog, useMessage } from 'naive-ui'
  import { defineComponent, onMounted, reactive, ref } from 'vue'
  import { createEpColumns, createVodColumns } from '../Data'
  import useVodUpdateStore from '@/store/modules/vod-update'
  import { oldParseUrlFlag, newParseUrlFlag } from '@/store/modules/vod-update'
  export default defineComponent({
    name: 'ConfirmVodInfoWithSearch',
    setup(props) {
      const useVodUpdate = useVodUpdateStore()
      const tableLoading = ref(false)
      const naiveDialog = useDialog()

      //表格
      const vodCheckedRowKeys = reactive<any[]>([useVodUpdate.vodId])
      const rowKey = useRowKey('id')
      const message = useMessage()
      const vodDataList = ref<any>([])
      const vodTableColumns = createVodColumns({
        remove(row: any) {
          naiveDialog.warning({
            title: '提示',
            content: '是否要删除【' + row.keyWord + '】？',
            positiveText: '删除',
            onPositiveClick: () => {
              tableLoading.value = true
              sendDelete({
                url: vodApi.vodDelete,
                data: {
                  ids: [row.id],
                },
              })
                .then((res) => {
                  message.success(res.msg)
                  search()
                })
                .finally(() => {
                  tableLoading.value = false
                })
            },
          })
        },
        down(row: any) {
          naiveDialog.warning({
            title: '提示',
            content: '是否要下架【' + row.keyWord + '】？',
            positiveText: '下架',
            onPositiveClick: () => {
              tableLoading.value = true
              post({
                url: vodApi.vodUpAndDown + '/' + row.id,
              })
                .then((res) => {
                  message.success(res.msg)
                  search()
                })
                .finally(() => {
                  tableLoading.value = false
                })
            },
          })
        },
      })
      //const epDataList = ref(useVodUpdate.epItems)
      const epTableColumns = createEpColumns({
        remove(row: any) {
          if (
            row.episodeUrl.indexOf(oldParseUrlFlag) != -1 ||
            row.episodeUrl.indexOf(newParseUrlFlag) != -1
          ) {
            message.warning('此剧集不支持移除,请直接替换或手动调整')
            return
          }

          if (useVodUpdate.epItems.length == 1) {
            message.warning('移除失败,至少有一行剧集')
            return
          } else if (useVodUpdate.epItems.length == useVodUpdate.vodPlayItems.length) {
            naiveDialog.warning({
              title: '提示',
              content: '当前剧集已和播放数量一致，确认要移除？',
              positiveText: '移除',
              onPositiveClick: () => {
                useVodUpdate.removeEpItem(row)
              },
            })

            return
          }

          useVodUpdate.removeEpItem(row)
        },
      })
      //选中
      function onVodRowCheck(rowKeys: Array<any>) {
        vodCheckedRowKeys.length = 0
        vodCheckedRowKeys.push(...rowKeys)

        const vodId = rowKeys[0]
        getDetail(vodId)
      }

      //获取详情
      function getDetail(vodId: any) {
        tableLoading.value = true
        get({
          url: vodApi.vodDetail + '/' + vodId,
        })
          .then((res) => {
            if (res.isSuccess && res.data) {
              //const firstGroupInfo = res.data.episodeGroup[0]
              //epDataList.value = firstGroupInfo.episodes
              //更新
              useVodUpdate.saveVodInfo(res.data)
            }
          })
          .finally(() => {
            tableLoading.value = false
          })
      }

      //搜素
      function search() {
        tableLoading.value = true
        get({
          url: vodApi.vodSearch,
          data: () => {
            return {
              keyWord: useVodUpdate.baseVodName,
              pageSize: 20,
            }
          },
        })
          .then((res) => {
            if (res.isSuccess && res.data) {
              vodDataList.value = res.data.data
              if (vodDataList.value.length == 0) {
                //未匹配到 清空之前的
                useVodUpdate.clearVodInfo()
              }
            }
          })
          .finally(() => {
            tableLoading.value = false
          })
      }

      //清除选中
      function onClearVod() {
        useVodUpdate.clearVodInfo()
        vodCheckedRowKeys.length = 0
      }

      //   //值变化
      //   watch(
      //     () => props.vodBaseInfo,
      //     (newVal) => {
      //       search()
      //       //localValue.value = newVal || ''
      //     }
      //   )

      onMounted(() => {
        search()
      })

      return {
        rowKey,
        vodCheckedRowKeys,
        tableLoading,
        vodTableColumns,
        epTableColumns,
        vodDataList,
        useVodUpdate,
        onVodRowCheck,
        onClearVod,
      }
    },
  })
</script>
