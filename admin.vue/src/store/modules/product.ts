import { get } from '@/api/http'
import { productApi } from '@/api/url'
import { defineStore } from 'pinia'
import { ProductTypeEnum } from '../types'

const useProductCacheStore = defineStore('ProductCache', {
  state: () => {
    return {
      cachedItems: [] as { lable: string; value: string; isLeaf: boolean }[],
    }
  },
  getters: {},
  actions: {
    /**
     * 初始化
     * @param productType 产品类型
     */
    async init(productType: ProductTypeEnum) {
      const getResult = await get({
        url: productApi.getOpen,
        data: {
          productType: productType,
        },
      })

      this.cachedItems = getResult.data.map((item: any) => {
        return {
          label: item.productName,
          value: item.id,
          isLeaf: false,
        }
      })
    },
    /**
     * 获取产品工序
     * @param productId 产品ID
     */
    async getProductProcesses(productId: string) {
      const getResult = await get({
        url: productApi.detailCascade + '/' + productId,
      })

      return getResult.data.productProcesses.map((item: any) => {
        return {
          label: `${item.productCraft.craftName}`,
          value: item.productProcessId,
        }
      })
    },
    reset() {
      this.$reset()
    },
  },
})

export default useProductCacheStore
