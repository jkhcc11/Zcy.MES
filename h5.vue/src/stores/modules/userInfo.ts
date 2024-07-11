import type { LoginResult } from '@/types/userInfo'
import { defineStore } from 'pinia'
import { ref } from 'vue'

// 用户信息 Store
export const useUserInfoStore = defineStore(
  'member',
  () => {
    // 用户信息
    const userInfo = ref<LoginResult>()

    // 保存用户信息，登录时使用
    const setProfile = (val: LoginResult) => {
      userInfo.value = val
    }

    //退出
    const clearProfile = () => {
      userInfo.value = undefined
    }

    // 记得 return
    return {
      userInfo,
      setProfile,
      clearProfile,
    }
  },
  {
    // 网页端配置
    // persist: true,
    // 小程序端配置
    persist: {
      storage: {
        getItem(key) {
          return uni.getStorageSync(key)
        },
        setItem(key, value) {
          uni.setStorageSync(key, value)
        },
      },
    },
  },
)
