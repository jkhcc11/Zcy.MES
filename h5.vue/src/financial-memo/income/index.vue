<script setup lang="ts">
import IncomeItem from '../components/IncomeItem.vue'
import { onUnload } from '@dcloudio/uni-app'
// #ifdef MP-WEIXIN
onUnload(() => {
  //因为小程序无法监听onBackPress 控制返回，目前监听卸载 然后直接跳转到首页
  uni.switchTab({ url: '/pages/new-index/index' })
})
// #endif
</script>

<template>
  <IncomeItem />
</template>

<style lang="scss">
page {
  background-color: #f7f7f7;
  height: 100%;
  overflow: hidden;
}
</style>
