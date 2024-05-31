<template>
  <div class="main-container">
    <n-card :title="`【${editData.vodMain?.keyWord ?? ''} 】编辑`" style="margin-bottom: 16px">
      <n-skeleton v-if="editLoadingRef" text :repeat="10" />
      <n-tabs type="segment" v-else>
        <n-tab-pane name="default" tab="影片信息">
          <DataForm
            ref="submitForm"
            :options="VodMainEditFormOptions"
            :form-config="{
              labelWidth: 100,
              size: 'medium',
              labelAlign: 'right',
            }"
            preset="grid-item"
          />
        </n-tab-pane>
        <n-tab-pane name="down" tab="剧集组">
          <VodMainEpisodeEdit>
            <template #headTip>
              <n-gradient-text type="warning"> 注：同名剧集名会直接覆盖!!! </n-gradient-text>
            </template>
          </VodMainEpisodeEdit>
        </n-tab-pane>
      </n-tabs>

      <template #action>
        <n-button type="info" size="small" @click="onSave" :loading="editLoadingRef">
          保存
        </n-button>
      </template>
    </n-card>
  </div>
</template>

<script lang="ts">
  import { vodApi, vodMainUrl } from '@/api/url'
  import { useMessage } from 'naive-ui'
  import { defineComponent, onMounted, reactive, ref } from 'vue'
  import { VodMainEditFormOptions } from './Data'
  import { RouteRecordRaw, useRoute, useRouter } from 'vue-router'
  import { get, post } from '@/api/http'
  import { EpisodeGroupTypeEnum } from '@/store/types'
  import useVisitedRouteStore from '@/store/modules/visited-routes'
  import useVodMainEditEpUpdateStore from '@/store/modules/vod-main-edit-ep-update'
  import VodMainEpisodeEdit from './components/VodMainEpisodeEdit.vue'
  export default defineComponent({
    name: 'VodMainEdit',
    components: { VodMainEpisodeEdit },
    setup() {
      const message = useMessage()
      const useVisitedRoute = useVisitedRouteStore()
      const useVodMainEditEpUpdate = useVodMainEditEpUpdateStore()
      const route = useRoute()
      const router = useRouter()

      const submitForm = ref<any>(null)
      const editLoadingRef = ref(false)
      const editData = reactive<any>({
        vodMain: null,
      })

      //加载详情
      function loadDetail() {
        //detailId.value = route.params.id
        editLoadingRef.value = true
        get({
          url: vodApi.vodDetail + '/' + route.params.id,
        })
          .then((res) => {
            editData.vodMain = res.data

            useVodMainEditEpUpdate.initData(
              editData.vodMain.episodeGroup.filter(
                (it: any) => it.episodeGroupType == EpisodeGroupTypeEnum.视频播放
              )[0]?.episodes ?? [],
              editData.vodMain.episodeGroup.filter(
                (it: any) => it.episodeGroupType == EpisodeGroupTypeEnum.视频下载
              )[0]?.episodes ?? []
            )

            //主要信息遍历赋值
            VodMainEditFormOptions.forEach((it: any) => {
              if (
                (it.key == 'videoMainStatus' || it.key == 'subtype' || it.key == 'orderBy') &&
                editData.vodMain[it.key] == 0
              ) {
                //特殊值
                it.value.value = 0
              } else {
                switch (it.key) {
                  case 'videoGenres':
                  case 'videoSummary':
                  case 'videoCasts':
                  case 'videoDirectors':
                  case 'videoCountries':
                  case 'banVideoJumpUrl':
                  case 'narrateUrl': {
                    //特殊
                    it.value.value = editData.vodMain.videoMainInfo[it.key] || null
                    break
                  }
                  default: {
                    it.value.value = editData.vodMain[it.key] || null
                  }
                }
              }
            })
            //console.log('detail', res.data)
          })
          .finally(() => {
            editLoadingRef.value = false
          })
      }

      //新增剧集
      function onCreateEp() {
        return {
          episodeName: '',
          episodeUrl: '',
        }
      }

      //保存
      function onSave() {
        if (!submitForm.value) {
          message.warning('请切换至【影片信息】后提交')
          return
        }

        if (
          useVodMainEditEpUpdate.epItemsWithDown.length <= 0 &&
          useVodMainEditEpUpdate.epItemsWithPlay.length <= 0
        ) {
          message.error('请添加剧集或下载地址')
          return
        }

        if (submitForm.value?.validator()) {
          editLoadingRef.value = true
          const formData = submitForm.value?.generatorParams()
          post({
            url: vodApi.vodModify,
            data: {
              episodeGroup: [
                {
                  episodeGroupType: EpisodeGroupTypeEnum.视频下载,
                  groupName: '默认下载',
                  episodes: useVodMainEditEpUpdate.epItemsWithDown.filter(
                    (it: any) => it.episodeUrl && it.episodeName
                  ),
                },
                {
                  episodeGroupType: EpisodeGroupTypeEnum.视频播放,
                  groupName: '默认播放',
                  episodes: useVodMainEditEpUpdate.epItemsWithPlay.filter(
                    (it: any) => it.episodeUrl && it.episodeName
                  ),
                },
              ],
              ...formData,
            },
          })
            .then((res: any) => {
              message.success(res.msg)
            })
            .finally(() => {
              editLoadingRef.value = false
              useVisitedRoute
                .removeVisitedRoute({
                  name: 'VodMainEdit',
                } as RouteRecordRaw)
                .then((lastPath) => {
                  router.push(lastPath)
                })
            })
        }
      }

      onMounted(() => {
        loadDetail()
      })

      return {
        submitForm,
        vodApi,
        vodMainUrl,
        VodMainEditFormOptions,
        editLoadingRef,
        editData,
        useVodMainEditEpUpdate,
        onCreateEp,
        onSave,
      }
    },
  })
</script>
