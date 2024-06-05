import { get } from '@/api/http'
import { productCraftApi } from '@/api/url'
import { defineStore } from 'pinia'

const useProductCraftCacheStore = defineStore('ProductCraftCache', {
  state: () => {
    return {
      cachedItems: [] as { lable: string; value: string }[],
    }
  },
  getters: {},
  actions: {
    async init() {
      const getResult = await get({
        url: productCraftApi.getOpen,
        data: () => {},
      })

      this.cachedItems = getResult.data.map((item: any) => {
        return {
          label: item.craftName,
          value: item.id,
        }
      })
    },
    reset() {
      this.$reset()
    },
  },
})

export default useProductCraftCacheStore
