import { queryValidProductTypeApi } from '@/services/product'
import { defineStore } from 'pinia'
import { ref } from 'vue'

//产品分类
export const useProductTypeStore = defineStore('productType', () => {
  const valiedProductTypeArray = ref<any[]>([])

  //初始化
  const init = async () => {
    var res = await queryValidProductTypeApi()
    valiedProductTypeArray.value = res.data
  }

  const getItem = (index: number) => {
    return valiedProductTypeArray.value[index]
  }

  return {
    valiedProductTypeArray,
    init,
    getItem,
  }
})
