import { get } from '@/api/http'
import { productTypeApi } from '@/api/url'
import { defineStore } from 'pinia'

const useProductTypeCacheStore = defineStore('ProductTypeCache', {
  state: () => {
    return {
      cachedItems: [] as { lable: string; value: string }[],
    }
  },
  getters: {},
  actions: {
    async init() {
      const getResult = await get({
        url: productTypeApi.getOpen,
      })

      this.cachedItems = getResult.data.map((item: any) => {
        return {
          label: item.typeName,
          value: item.id,
        }
      })
    },
    reset() {
      this.$reset()
    },
  },
})

export default useProductTypeCacheStore
