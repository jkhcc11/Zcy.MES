import { get } from '@/api/http'
import { roleMenuApi } from '@/api/url'
import { defineStore } from 'pinia'

const useRoleCacheStore = defineStore('SystemRoleCache', {
  state: () => {
    return {
      cachedItems: [] as { lable: string; value: string }[],
    }
  },
  getters: {},
  actions: {
    /**
     * 初始化
     */
    async init() {
      const getResult = await get({
        url: roleMenuApi.queryRole,
        data: {
          pageSize: 100,
          page: 1,
        },
      })

      this.cachedItems = getResult.data.items.map((item: any) => {
        return {
          label: item.roleShowName,
          value: item.id,
        }
      })
    },
    reset() {
      this.$reset()
    },
  },
})

export default useRoleCacheStore
