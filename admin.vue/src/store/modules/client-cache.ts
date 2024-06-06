import { get } from '@/api/http'
import { supplierClientApi } from '@/api/url'
import { defineStore } from 'pinia'
import { ClientTypeEnum } from '../types'

const useClientCacheStore = defineStore('ClientCache', {
  state: () => {
    return {
      cachedItems: [] as { lable: string; value: string }[],
      supplierCacheItems: [] as { lable: string; value: string }[],
    }
  },
  getters: {},
  actions: {
    async init(clientType: ClientTypeEnum) {
      const getResult = await get({
        url: supplierClientApi.getOpenClient,
        data: () => {
          return {
            clientType: clientType,
          }
        },
      })

      if (clientType == ClientTypeEnum.供应商) {
        this.supplierCacheItems = getResult.data.map((item: any) => {
          return {
            label: item.clientName,
            value: item.id,
          }
        })

        return
      }

      this.cachedItems = getResult.data.map((item: any) => {
        return {
          label: item.clientName,
          value: item.id,
        }
      })
    },
    reset() {
      this.$reset()
    },
  },
})

export default useClientCacheStore
