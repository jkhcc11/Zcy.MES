<template>
  <div class="main-container">
    <n-card :content-style="{ padding: '25px' }">
      <n-steps :current="activeStep" status="process" class="steps-wrapper">
        <n-step title="基本信息" />
        <n-step title="确认信息" />
        <n-step title="入库完成" />
      </n-steps>
    </n-card>
    <n-card :body-style="{ padding: '5px' }" shadow="hover" class="margin-top-xs">
      <!-- <div class="p-4 text-center title">
        <n-text type="primary">
          {{ activeStep === 1 || activeStep === 2 ? '影片信息' : '入库结果' }}
        </n-text>
      </div> -->
      <VodBaseInfo v-if="activeStep === 1" @next-step="next" />
      <ConfirmVodInfo v-if="activeStep === 2" @next-step="activeStep++" @pre-step="activeStep--" />
      <ResultInfo v-if="activeStep === 3" @pre-step="activeStep = 1" />
    </n-card>
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue'
  import VodBaseInfo from './components/VodBaseInfo.vue'
  import ConfirmVodInfo from './components/ConfirmVodInfo.vue'
  import ResultInfo from './components/ResultInfo.vue'
  export default defineComponent({
    name: 'FastUpdateVod',
    components: { VodBaseInfo, ConfirmVodInfo, ResultInfo },
    data() {
      return {
        activeStep: 1,
      }
    },
    methods: {
      next() {
        if (this.activeStep === 1) {
          this.activeStep += 1
        }
      },
    },
  })
</script>

<style lang="scss" scoped>
  @media screen and (max-width: 768px) {
    .steps-wrapper {
      width: 100%;
      margin: 0 auto;
    }
  }
  @media screen and (min-width: 768px) {
    .steps-wrapper {
      width: 60%;
      margin: 0 auto;
    }
  }
</style>
