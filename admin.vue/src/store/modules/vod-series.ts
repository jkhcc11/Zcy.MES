import { get } from '@/api/http'
import { vodSeriesApi } from '@/api/url'
import { defineStore } from 'pinia'

const useVodSeriesStore = defineStore('vod-series', {
  state: () => {
    return {
      cachedVodSeries: [] as { label: string; value: string }[],
    }
  },
  getters: {},
  actions: {
    async init() {
      const getVodSeries = await get({
        url: vodSeriesApi.getSeriesAllList,
      })

      this.cachedVodSeries = getVodSeries.data.map((item: any) => {
        return {
          label: item.text,
          value: item.value,
        }
      })
    },
    reset() {
      this.$reset()
    },
  },
  presist: {
    enable: true,
    resetToState: true,
  },
})

export default useVodSeriesStore
