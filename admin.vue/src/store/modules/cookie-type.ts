import { get } from '@/api/http'
import { parseUserApi } from '@/api/url'
import { defineStore } from 'pinia'

const useCookieTypeStore = defineStore('cookie-type', {
  state: () => {
    return {
      cachedCookieType: [] as { lable: string; value: string }[],
      tyTypeId: '' as string,
      aliTypeId: '' as string,
    }
  },
  getters: {},
  actions: {
    async init() {
      const getCookieType = await get({
        url: parseUserApi.getAllCookieType,
        data: () => {
          return {
            page: 1,
            maxResultCount: 100,
          }
        },
      })

      this.cachedCookieType = getCookieType.data.map((item: any) => {
        return {
          label: item.showText,
          value: item.id,
        }
      })
      this.tyTypeId = getCookieType.data.filter((it: any) => it.showText === '天翼个人')[0].id
      this.aliTypeId = getCookieType.data.filter((it: any) => it.showText === '阿里云盘')[0].id
    },
    reset() {
      this.$reset()
    },
  },
})

export default useCookieTypeStore
