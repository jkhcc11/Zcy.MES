import { defineStore } from 'pinia'

const useVodMainEditEpUpdateStore = defineStore('vod-main-edit-ep-update', {
  state: () => {
    return {
      epItemsWithPlay: [],
      epItemsWithDown: [],
    }
  },
  actions: {
    /**
     * 初始化数据
     * @param epItemsWithPlay 播放剧集
     * @param epItemsWithDown 下载剧集
     */
    initData(epItemsWithPlay: [], epItemsWithDown: []) {
      this.epItemsWithPlay = epItemsWithPlay
      this.epItemsWithDown = epItemsWithDown
    },
    /**
     * 更新播放剧集
     * @param epItemsWithPlay 播放剧集
     */
    updateEpItemsWithPlay(epItemsWithPlay: []) {
      this.epItemsWithPlay = epItemsWithPlay
    },
    /**
     * 更新下载剧集
     * @param epItemsWithPlay 播放剧集
     */
    updateEpItemsWithDown(epItemsWithDown: []) {
      this.epItemsWithDown = epItemsWithDown
    },
    reset() {
      this.$reset()
    },
  },
})

export default useVodMainEditEpUpdateStore
