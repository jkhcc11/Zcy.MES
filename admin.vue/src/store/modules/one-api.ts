import { defineStore } from 'pinia'

const useOneApiStore = defineStore('one-api-cache', {
  state: () => {
    return {
      apiToken: '',
    }
  },
  actions: {
    save(apiToken: string) {
      this.apiToken = apiToken
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

export default useOneApiStore
