import type { QueryValidProductDto } from '@/types/product'
import { defineStore } from 'pinia'
import { ref } from 'vue'

//产品选择
export const useProductSelectedStore = defineStore(
  'productSelected',
  () => {
    const selectedProducts = ref<QueryValidProductDto[]>([])

    // 设置选择值
    const setSelected = (val: QueryValidProductDto[]) => {
      selectedProducts.value = val
    }

    // 添加选择
    const addSelected = (val: QueryValidProductDto) => {
      selectedProducts.value.push(val)
    }

    //清空
    const clearInfo = () => {
      selectedProducts.value.length = 0
    }

    return {
      selectedProducts,
      addSelected,
      setSelected,
      clearInfo,
    }
  },
  {
    // 网页端配置
    // persist: true,
    // 小程序端配置
    persist: {
      storage: {
        getItem(key) {
          return uni.getStorageSync(key)
        },
        setItem(key, value) {
          uni.setStorageSync(key, value)
        },
      },
    },
  },
)
