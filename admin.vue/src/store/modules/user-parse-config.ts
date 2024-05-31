import { get } from '@/api/http'
import { baseUserInfoApi } from '@/api/url'
import { defineStore } from 'pinia'

//用户配置
const useUserParseConfigStore = defineStore('user-parse-config', {
  state: () => {
    return {
      userParseConfig: null as any,
      //解析是否快过期  5天到期
      isToExpiration: false,
      //过期日期
      expirationDate: '',
    }
  },
  actions: {
    async init() {
      if (this.userParseConfig) {
        //有值 不重新获取
        return
      }

      const getConfig = await get({
        url: baseUserInfoApi.getLoginInfo,
      })

      this.userParseConfig = getConfig.data

      if (this.userParseConfig.expirationDateTime) {
        // 获取当前日期和时间
        const now = new Date()
        const expirationDate = new Date(this.userParseConfig.expirationDateTime)
        const timeDiff = expirationDate.getTime() - now.getTime()
        // 将毫秒差转换为天数差
        const daysDiff = timeDiff / (1000 * 60 * 60 * 24)
        // 返回整数部分，表示完整的天数
        this.isToExpiration = Math.ceil(daysDiff) <= 5

        this.expirationDate = new Date(this.userParseConfig.expirationDateTime)
          .toLocaleDateString('zh-CN', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
          })
          .replace(/\//g, '-')
      }
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

export default useUserParseConfigStore
