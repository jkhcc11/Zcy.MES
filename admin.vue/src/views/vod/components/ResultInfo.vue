<template>
  <div>
    <div class="text-center">
      <n-icon size="50" color="#0e7a0d">
        <CheckmarkCircle />
      </n-icon>
    </div>
    <div class="result"> 操作成功 </div>
    <div class="tip"> 请前往播放页检查 </div>
    <n-card :content-style="{ padding: '10px' }" class="mt-4">
      <n-space vertical>
        <div>
          <span>初始影片名：</span>
          <n-gradient-text type="primary">{{ useVodUpdate.baseVodName }}</n-gradient-text>
        </div>
        <div v-if="useVodUpdate.vodId">
          <span>更新的影片名：</span>
          <n-gradient-text type="primary">{{ useVodUpdate.vodName }}</n-gradient-text>
        </div>
        <div v-if="useVodUpdate.vodId == null">
          <span>新增的影片名：</span>
          <n-gradient-text type="primary">{{ useVodUpdate.doubanName }}</n-gradient-text>
        </div>
        <div v-if="useVodUpdate.baseVodSeriesId">
          <span>系列名：</span>
          <n-gradient-text type="primary">{{ useVodUpdate.baseVodSeriesStr }}</n-gradient-text>
        </div>
        <div>
          <span>剧集数量：</span>
          <n-gradient-text type="primary">{{ useVodUpdate.vodPlayItems.length }}</n-gradient-text>
        </div>
        <div>
          <span>是否新增：</span>
          <n-gradient-text type="primary">
            {{ useVodUpdate.vodId == null ? '是' : '否' }}
          </n-gradient-text>
        </div>
      </n-space>
    </n-card>
    <div class="flex justify-end mt-2 mb-2">
      <n-space>
        <n-button size="small" type="warning" @click="onPreStep">再录一次</n-button>
        <!-- <n-button
          type="primary"
          size="small"
          tag="a"
          target="_blank"
          text
          href="https://www.baidu.com"
          rel="noreferrer"
          >影片详情</n-button
        > -->
      </n-space>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue'
  import { CheckmarkCircle } from '@vicons/ionicons5'
  import useVodUpdateStore from '@/store/modules/vod-update'
  export default defineComponent({
    name: 'VodUpdateResultInfo',
    components: { CheckmarkCircle },
    emits: ['pre-step'],
    setup(props, { emit }) {
      const useVodUpdate = useVodUpdateStore()

      function onPreStep() {
        useVodUpdate.reset()
        emit('pre-step')
      }

      return {
        useVodUpdate,
        onPreStep,
      }
    },
  })
</script>

<style lang="scss" scoped>
  @media screen and (max-width: 768px) {
    .result-wrapper {
      width: 90%;
      margin: 0 auto;
    }
  }
  @media screen and (min-width: 768px) {
    .result-wrapper {
      width: 50%;
      margin: 0 auto;
    }
  }
  .form-wrapper {
    margin-top: 20px;
    padding: 10px;
  }
  .icon {
    color: #67c23a;
    font-size: 100px;
    margin: 30px 0;
  }
  .result {
    font-size: 24px;
    line-height: 1.8;
    text-align: center;
  }
  .tip {
    font-size: 14px;
    line-height: 1.6;
    text-align: center;
  }
  .action {
    margin-top: 30px;
  }
</style>
