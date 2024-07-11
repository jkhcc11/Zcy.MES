<script setup lang="ts">
import ProductItem from './components/ProductItem.vue'
import { useProductSelectedStore } from '@/stores/modules/productSelected'

// 获取页面参数
const query = defineProps<{
  productType?: string | null
}>()

//产品选择持久化
const useProductSelected = useProductSelectedStore()

//选中回调
function onChecked(keys: any) {
  //console.log('keys', keys)
  useProductSelected.setSelected(keys.checkedRows)
  uni.navigateBack()
}
</script>

<template>
  <ProductItem :productType="query.productType" @onChecked="onChecked" />
</template>

<style lang="scss">
page {
  background-color: #f7f7f7;
  height: 100%;
  overflow: hidden;
}
</style>
