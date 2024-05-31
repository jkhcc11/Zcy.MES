import { defineStore } from 'pinia'

const useVodSearchStore = defineStore('vod-search', {
  state: () => {
    return {
      keyWord: '',
    }
  },
  getters: {},
  actions: {
    setKeyWord(keyWord: string) {
      this.keyWord = keyWord
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

export default useVodSearchStore
