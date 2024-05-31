<template>
  <n-form
    ref="stepOneForm"
    :model="vodBaseInfo"
    :rules="vodBaseInfoRule"
    label-width="120"
    label-placement="left"
  >
    <n-form-item label="影片基础名" path="vodName">
      <n-input
        v-model:value="vodBaseInfo.vodName"
        placeholder="影片基础名 不要包含特殊字符或第几季"
      />
      <n-button
        text
        tag="a"
        :href="`https://www.douban.com/search?cat=1002&q=${vodBaseInfo.vodName}`"
        target="_blank"
        type="primary"
        rel="noreferrer"
      >
        搜索豆瓣
      </n-button>
    </n-form-item>
    <n-form-item label="豆瓣链接" path="doubanUrl">
      <n-input v-model:value="vodBaseInfo.doubanUrl" placeholder="豆瓣链接" />
    </n-form-item>
    <!-- <n-form-item label="年份" path="vodYear">
      <n-input-number
        v-model:value="vodBaseInfo.vodYear"
        placeholder="影片年份 以豆瓣为准"
        :min="1000"
        :max="3000"
      />
    </n-form-item> -->
    <n-form-item label="所属系列" path="vodSeriesId">
      <n-select
        v-model:value="vodBaseInfo.vodSeriesId"
        placeholder="可不选"
        :options="useVodSeries.cachedVodSeries"
        :clearable="true"
      />
    </n-form-item>
    <n-form-item label="播放链接" path="vodPlayUrls">
      <n-input
        v-model:value="vodBaseInfo.vodPlayUrls"
        placeholder="影片播放链接"
        type="textarea"
        :rows="5"
      />
    </n-form-item>
    <n-form-item class="flex justify-end mt-2 mb-2">
      <n-space>
        <n-button size="small" @click="clearStepOneInfo">重置</n-button>
        <n-button type="primary" size="small" @click="nextStep" :loading="nextLoading"
          >下一步</n-button
        >
      </n-space>
    </n-form-item>
  </n-form>
  <n-divider />
  <n-card class="ml-2 mr-2 tip-wrapper" :content-style="{ padding: 0 }">
    <div class="text-lg text-bold">录入说明</div>
    <div class="margin-tb">电影</div>
    名称只需要基础名称，例如： <br />蜘蛛侠2：英雄远征，名称只需要：<abbr
      style="color: red; font-weight: bold"
      >蜘蛛侠</abbr
    >
    <br />
    饥饿游戏：鸣鸟与蛇之歌 ，名称只需要：<abbr style="color: red; font-weight: bold">饥饿游戏</abbr>
    <div class="margin-tb">电视剧</div>
    名称只需要基础名称，例如：疑犯追踪 第二季，名称只需要：<abbr
      style="color: red; font-weight: bold"
      >疑犯追踪</abbr
    >
  </n-card>
</template>

<script lang="ts">
  import { NForm } from 'naive-ui'
  import { defineComponent, onActivated, onMounted, reactive, ref } from 'vue'
  import useVodUpdateStore from '@/store/modules/vod-update'
  import { VodUpdateState } from '@/store/types'
  import useVodSeriesStore from '@/store/modules/vod-series'
  export default defineComponent({
    name: 'VodBaseInfo',
    emits: ['next-step'],
    setup(props, { emit }) {
      const useVodSeries = useVodSeriesStore()
      const nextLoading = ref(false)
      const useVodUpdate = useVodUpdateStore()
      const stepOneForm = ref<typeof NForm | null>(null)
      const vodBaseInfo = reactive({
        vodName: useVodUpdate.baseVodName,
        vodSeriesId: useVodUpdate.baseVodSeriesId,
        vodPlayUrls: useVodUpdate.vodPlayUrlsWithStr,
        doubanUrl: useVodUpdate.doubanUrl,
      })
      const vodBaseInfoRule = {
        vodName: [{ required: true, message: '影片基础名', trigger: 'blur' }],
        doubanUrl: [
          {
            required: true,
            trigger: 'blur',
            validator(rule: any, value: string) {
              if (!value) {
                return new Error('豆瓣链接必填')
              } else if (value.indexOf('/subject/') == -1) {
                return new Error('豆瓣链接错误')
              }

              return true
            },
          },
        ],
        // vodYear: [
        //   {
        //     required: true,
        //     message: '影片所属年份',
        //     trigger: 'blur',
        //     type: 'number',
        //   },
        // ],
        vodPlayUrls: [
          {
            required: true,
            trigger: 'blur',
            validator(rule: any, value: string) {
              if (!value) {
                return new Error('播放链接必填')
              } else if (!/^([^\$\n]+\$[^\$\n]+)(\n[^\$\n]+\$[^\$\n]+)*$/.test(value)) {
                return new Error('播放链接格式错误')
              }

              return true
            },
          },
        ],
      }

      function clearStepOneInfo() {
        vodBaseInfo.doubanUrl = ''
        vodBaseInfo.vodName = ''
        vodBaseInfo.vodPlayUrls = ''
        vodBaseInfo.vodSeriesId = null
        useVodUpdate.reset()
      }

      function nextStep() {
        stepOneForm.value?.validate(async (error: any) => {
          if (!error) {
            nextLoading.value = true
            const currentSeriesArray = useVodSeries.cachedVodSeries.filter(
              (it) => it.value == vodBaseInfo.vodSeriesId
            )

            await useVodUpdate.saveBaseInfo({
              baseVodName: vodBaseInfo.vodName,
              //baseVodYear: vodBaseInfo.vodYear,
              baseVodSeriesId: vodBaseInfo.vodSeriesId,
              baseVodSeriesStr: currentSeriesArray[0]?.label,
              vodPlayUrlsWithStr: vodBaseInfo.vodPlayUrls,
              doubanUrl: vodBaseInfo.doubanUrl,
            } as VodUpdateState)

            nextLoading.value = false
            emit('next-step')
          }
        })
      }

      onActivated(() => {
        if ((useVodUpdate.fromPage = 'cloudlink')) {
          vodBaseInfo.vodName = useVodUpdate.baseVodName
          vodBaseInfo.vodSeriesId = useVodUpdate.baseVodSeriesId
          vodBaseInfo.vodPlayUrls = useVodUpdate.vodPlayUrlsWithStr
          vodBaseInfo.doubanUrl = ''
        }
      })

      onMounted(async () => {
        await useVodSeries.init()
      })

      return {
        nextLoading,
        stepOneForm,
        vodBaseInfo,
        vodBaseInfoRule,
        useVodSeries,
        clearStepOneInfo,
        nextStep,
      }
    },
  })
</script>

<style lang="scss" scoped>
  .tip-wrapper {
    padding: 10px;
  }
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
