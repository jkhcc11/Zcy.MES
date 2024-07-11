import { ProductTypeEnum } from '@/services/constants'
import { getProductProcessWithNormalApi, queryValidProductWithNormalApi } from '@/services/product'
import { defineStore } from 'pinia'
import { ref } from 'vue'

//产品picker
export const useProductPickerStore = defineStore('productPicker', () => {
  //产品列表
  const currentProducts = ref<any[]>([])
  //产品工序列表
  const currentProductProcesses = ref<any[]>([])
  //初始化有效产品列表
  const init = async (productType: ProductTypeEnum) => {
    var res = await queryValidProductWithNormalApi({
      productType: productType,
    })
    currentProducts.value = res.data
  }

  //获取产品Item
  const getProductItem = (index: number) => {
    return currentProducts.value[index]
  }

  //获取产品工序
  const getProductProcesses = async (id: string) => {
    var res = await getProductProcessWithNormalApi(id)
    currentProductProcesses.value = res.data
  }

  //获取产品工序Item
  const getProductProcessesItem = (index: number) => {
    return currentProductProcesses.value[index]
  }

  //清空
  const clearInfo = () => {
    currentProducts.value.length = 0
    currentProductProcesses.value.length = 0
  }

  return {
    currentProducts,
    currentProductProcesses,
    init,
    getProductProcesses,
    getProductItem,
    getProductProcessesItem,
    clearInfo,
  }
})
