import { defineStore } from 'pinia'
import { CmsSendInfo } from '../types'

const useCmsSendInfoStore = defineStore('cms-send-info', {
  state: () => {
    return {
      sendInfo: null as CmsSendInfo | null,
    }
  },
  actions: {
    /**
     * 保存
     */
    save(info: CmsSendInfo) {
      this.sendInfo = {
        apiUrl: info.apiUrl,
        sendPassWord: info.sendPassWord,
        sendType: info.sendType,
        vodTypeName: info.vodTypeName,
        playFrom: info.playFrom,
      } as CmsSendInfo
    },
    reset() {
      this.$reset()
      // localStorage.clear()
      // sessionStorage.clear()
    },
  },
  presist: {
    enable: true,
    resetToState: true,
  },
})

export default useCmsSendInfoStore
