<template>
  <div class="main-container">
    <n-grid x-gap="12" :cols="2">
      <n-gi>
        <n-card title="基础信息">
          <n-form label-width="120px" label-placement="left">
            <n-form-item label="影片基础名">
              <span>{{ useVodUpdate.baseVodName }}</span>
            </n-form-item>
            <n-form-item label="豆瓣链接">
              <span>
                {{ useVodUpdate.doubanUrl }}
              </span>
            </n-form-item>
            <n-form-item label="所属系列">
              <span>
                {{ useVodUpdate.baseVodSeriesStr ?? '暂无' }}
              </span>
            </n-form-item>
            <n-form-item label="播放链接" path="vodPlayUrls">
              <n-input
                v-model:value="useVodUpdate.vodPlayUrlsWithStr"
                placeholder="播放链接"
                readonly
                type="textarea"
                :rows="5"
              />
            </n-form-item>
          </n-form>
          <n-space justify="end">
            <n-gradient-text type="info">
              录入数量: {{ useVodUpdate.vodPlayItems.length }}
            </n-gradient-text>
            <n-popconfirm @positive-click="onReplaceOk">
              <template #trigger>
                <n-button size="tiny" type="warning">替换剧集</n-button>
              </template>
              是否替换已选影片剧集播放链接？
            </n-popconfirm>
          </n-space>
        </n-card>
      </n-gi>
      <n-gi>
        <n-card title="豆瓣信息">
          <n-form label-width="120px" label-placement="left">
            <n-form-item label="豆瓣名称">
              <span :style="useVodUpdate.isSameName ? 'color: green;' : 'color: red;'">{{
                useVodUpdate.doubanName
              }}</span>
            </n-form-item>
            <n-form-item label="豆瓣演员">
              <span :style="useVodUpdate.isSameCasts ? 'color: green;' : 'color: red;'">{{
                useVodUpdate.doubanCasts
              }}</span>
            </n-form-item>
            <n-form-item label="豆瓣年份">
              <span
                :style="
                  useVodUpdate.doubanYear == useVodUpdate.vodYear ? 'color: green;' : 'color: red;'
                "
              >
                {{ useVodUpdate.doubanYear }}
              </span>
            </n-form-item>
            <n-form-item label="海报">
              <n-image width="80" :src="useVodUpdate.doubanConvert" />
            </n-form-item>
          </n-form>
        </n-card>
      </n-gi>
      <n-gi :span="2">
        <ConfirmVodInfoWithSearch />
      </n-gi>

      <n-gi :span="2">
        <n-space>
          <n-gradient-text
            type="warning"
            v-if="useVodUpdate.isAllMatch != null && !useVodUpdate.isAllMatch"
          >
            当前影片年份、名称和豆瓣信息不一致，请留意。如果确实如此，请忽略
          </n-gradient-text>
          <n-gradient-text
            type="success"
            v-if="useVodUpdate.isAllMatch != null && useVodUpdate.isAllMatch"
          >
            完全匹配
          </n-gradient-text>
        </n-space>
        <div class="flex justify-end mt-2 mb-2">
          <n-space>
            <n-button size="small" type="warning" @click="onPreStep">上一步</n-button>
            <n-button @click="onNextStep" size="small" type="primary" :loading="loadingStatusRef">
              下一步
            </n-button>
          </n-space>
        </div>
      </n-gi>
    </n-grid>
  </div>
</template>

<script lang="ts">
  import { post } from '@/api/http'
  import { vodApi } from '@/api/url'
  import { NForm, useMessage, useDialog } from 'naive-ui'
  import { defineComponent, ref } from 'vue'
  import ConfirmVodInfoWithSearch from './ConfirmVodInfoWithSearch.vue'
  import useVodUpdateStore from '@/store/modules/vod-update'
  export default defineComponent({
    name: 'ConfirmVodInfo',
    components: { ConfirmVodInfoWithSearch },
    emits: ['pre-step', 'next-step'],
    setup(props, { emit }) {
      const useVodUpdate = useVodUpdateStore()
      const loadingStatusRef = ref(false)
      const naiveDialog = useDialog()
      const message = useMessage()

      function onPreStep() {
        emit('pre-step')
      }

      function onNextStep() {
        //1、影片Id、豆瓣Id(已经入库的)
        //2、剧集列表可删除，但必须有一条数据
        //3、检查当前剧集数量跟链接是否一致，如果不一致，则不能进入下一步。
        //一致时弹窗确认后提交。
        if (!useVodUpdate.vodId) {
          //无记录 创建
          naiveDialog.warning({
            title: '提示',
            content: '当前操作为【创建影片】操作，请检查播放链接和豆瓣信息是否正确？',
            positiveText: '确认',
            onPositiveClick: () => {
              //
              loadingStatusRef.value = true
              post({
                url: vodApi.createByDouBan,
                data: {
                  douBanInfoId: useVodUpdate.doubanDbId,
                  epItems: useVodUpdate.vodPlayItems,
                  seriesId: useVodUpdate.baseVodSeriesId,
                },
              })
                .then((res) => {
                  if (res.isSuccess) {
                    emit('next-step')
                  }
                })
                .finally(() => {
                  loadingStatusRef.value = false
                })
            },
          })

          return
        }

        //有记录更新
        if (!useVodUpdate.checkEpItem()) {
          message.error('检测到剧集未替换，请确认')
          return
        }

        naiveDialog.warning({
          title: '提示',
          content:
            '当前需要更新影片名：【' + useVodUpdate.vodName + '】，请检查是否已替换最新链接？',
          positiveText: '确认',
          onPositiveClick: () => {
            loadingStatusRef.value = true
            post({
              url: vodApi.updateByDouBan,
              data: {
                douBanInfoId: useVodUpdate.doubanDbId,
                epItems: useVodUpdate.epItems,
                vodId: useVodUpdate.vodId,
                epGroupId: useVodUpdate.groupId,
                seriesId: useVodUpdate.baseVodSeriesId,
              },
            })
              .then((res) => {
                if (res.isSuccess) {
                  emit('next-step')
                }
              })
              .finally(() => {
                loadingStatusRef.value = false
              })
          },
        })
      }

      //替换集剧
      function onReplaceOk() {
        const replaceResult = useVodUpdate.tryReplace()
        if (replaceResult) {
          message.success('替换成功，可进入下一步')
        } else {
          message.warning('替换失败,剧集数量和播放链接数量不一致')
        }
      }
      return {
        loadingStatusRef,
        useVodUpdate,
        onPreStep,
        onNextStep,
        onReplaceOk,
      }
    },
  })
</script>

<style lang="scss" scoped>
  @media screen and (max-width: 768px) {
    .form-wrapper {
      width: 90%;
      margin: 0 auto;
    }
  }
  @media screen and (min-width: 768px) {
    .form-wrapper {
      width: 50%;
      margin: 0 auto;
    }
  }
</style>
