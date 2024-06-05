import { get } from '@/api/http'
import { companyApi } from '@/api/url'
import { defineStore } from 'pinia'

const useCompanyCacheStore = defineStore('CompanyCache', {
  state: () => {
    return {
      cachedItems: [] as { lable: string; value: string; userName: string }[],
    }
  },
  getters: {},
  actions: {
    /**
     * 初始化
     */
    async init() {
      const getResult = await get({
        url: companyApi.getValidEmployee,
      })

      this.cachedItems = getResult.data.map((item: any) => {
        return {
          label: item.userNick,
          userName: item.userName,
          value: item.id,
        }
      })
    },
    /**
     * 初始化
     */
    async initCompanyData() {
      const getResult = await get({
        url: companyApi.query,
        data: {
          pageSize: 100,
          page: 1,
        },
      })

      this.cachedItems = getResult.data.items.map((item: any) => {
        return {
          label: item.companyName,
          value: item.id,
        }
      })
    },
    reset() {
      this.$reset()
    },
  },
})

export default useCompanyCacheStore
