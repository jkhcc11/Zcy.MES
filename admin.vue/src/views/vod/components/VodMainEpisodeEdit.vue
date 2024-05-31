<template>
  <slot name="headTip"></slot>
  <n-input
    v-model:value="editData.epText"
    type="textarea"
    placeholder="批量剧集 格式 xxx$xxxx 换行"
    :on-blur="onBlurChange"
  />
  <n-tabs :value="editData.tabValue" :on-update:value="onTabChange">
    <n-tab-pane name="default" tab="播放组" style="max-height: 40vh; overflow-y: auto">
      <n-dynamic-input v-model:value="editData.epItemsWithPlay" :on-create="onCreateEp">
        <template #create-button-default> 添加剧集 </template>
        <template #default="{ value }">
          <div style="display: flex; align-items: center; width: 100%">
            <n-input-number
              v-model:value="value.orderBy"
              :min="0"
              :max="999"
              placeholder="越大越靠前"
            />
            <n-input v-model:value="value.episodeName" type="text" placeholder="剧集名" />
            <n-input v-model:value="value.episodeUrl" type="text" placeholder="剧集Url" />

            <n-button
              v-if="value.id && value.id > 0"
              text
              tag="a"
              :href="`${vodMainUrl}/Movie/Vod/${value.id}`"
              target="_blank"
              type="primary"
            >
              前往播放
            </n-button>
          </div>
        </template>
      </n-dynamic-input>
    </n-tab-pane>
    <n-tab-pane name="down" tab="下载组" style="max-height: 40vh; overflow-y: auto">
      <!-- <n-input
        v-model:value="editData.epTextWithDown"
        type="textarea"
        placeholder="批量 格式 xxx$xxxx 换行"
        :on-blur="onSyncDown"
      /> -->

      <n-dynamic-input v-model:value="editData.epItemsWithDown" :on-create="onCreateEp">
        <template #create-button-default> 添加下载 </template>
        <template #default="{ value }">
          <div style="display: flex; align-items: center; width: 100%">
            <n-input-number
              v-model:value="value.orderBy"
              :min="0"
              :max="999"
              placeholder="越大越靠前"
            />
            <n-input v-model:value="value.episodeName" type="text" placeholder="剧集名" />
            <n-input v-model:value="value.episodeUrl" type="text" placeholder="剧集Url" />
          </div>
        </template>
      </n-dynamic-input>
    </n-tab-pane>
  </n-tabs>
</template>

<script lang="ts">
  import { vodMainUrl } from '@/api/url'
  import { defineComponent, reactive, watch } from 'vue'
  import useVodMainEditEpUpdateStore from '@/store/modules/vod-main-edit-ep-update'
  import { firstOrDefault, getEpItemsByVodPlayUrls } from '@/utils'
  export default defineComponent({
    name: 'VodMainEpisodeEdit',
    setup() {
      const useVodMainEditEpUpdate = useVodMainEditEpUpdateStore()

      const editData = reactive<any>({
        epItemsWithPlay: useVodMainEditEpUpdate.epItemsWithPlay,
        epItemsWithDown: useVodMainEditEpUpdate.epItemsWithDown,
        epText: '',
        tabValue: 'default',
      })

      //新增剧集
      function onCreateEp() {
        return {
          episodeName: '',
          episodeUrl: '',
          orderBy: 0,
        }
      }

      function onBlurChange() {
        if (!editData.epText) {
          return
        }

        const tempEpItems = getEpItemsByVodPlayUrls(editData.epText)
        if (tempEpItems.length <= 0) {
          return
        }

        tempEpItems.forEach((item) => {
          let epItem = firstOrDefault(
            editData.epItemsWithPlay,
            (it: any) => it.episodeName === item.episodeName
          )
          if (editData.tabValue == 'down') {
            epItem = firstOrDefault(
              editData.epItemsWithDown,
              (it: any) => it.episodeName === item.episodeName
            )
          }

          if (epItem) {
            epItem.episodeUrl = item.episodeUrl
          } else {
            if (editData.tabValue == 'down') {
              editData.epItemsWithDown.push({
                episodeName: item.episodeName,
                episodeUrl: item.episodeUrl,
                orderBy: 0,
              })
            } else {
              editData.epItemsWithPlay.push({
                episodeName: item.episodeName,
                episodeUrl: item.episodeUrl,
                orderBy: 0,
              })
            }
          }
        })

        editData.epText = ''
      }

      function onTabChange(val: any) {
        editData.tabValue = val
      }

      watch(editData, (newValue) => {
        useVodMainEditEpUpdate.updateEpItemsWithPlay(newValue.epItemsWithPlay)
        useVodMainEditEpUpdate.updateEpItemsWithDown(newValue.epItemsWithDown)
      })

      return {
        vodMainUrl,
        editData,
        onTabChange,
        onCreateEp,
        onBlurChange,
      }
    },
  })
</script>
