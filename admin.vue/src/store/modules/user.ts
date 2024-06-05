import { defineStore } from 'pinia'
import { UserState } from '../types'
import store from '../pinia'

import Avatar from '@/assets/img_avatar.gif'
// import { post } from '@/api/http'
// import { refresh } from '@/api/url'

const defaultAvatar = Avatar

const useUserStore = defineStore('user-info', {
  state: () => {
    return {
      userId: '',
      companyId: '',
      refreshToken: '',
      accessToken: '',
      userNo: '',
      userNick: '',
      baseSettlement: 0,
      isSuperAdmin: false,
      avatar: defaultAvatar,
    }
  },
  actions: {
    /**
     * 保存token
     * @param userInfo 只处理 accessToken、refreshToken
     * @returns
     */
    saveToken(userInfo: UserState) {
      return new Promise<UserState>((resolve) => {
        this.accessToken = userInfo.accessToken
        this.refreshToken = userInfo.refreshToken
        this.userNo = userInfo.userNo
        this.userNick = userInfo.userNick
        this.companyId = userInfo.companyId
        this.baseSettlement = userInfo.baseSettlement
        this.isSuperAdmin = userInfo.isSuperAdmin
        resolve(userInfo)
      })
    },
    /**
     * 获取最新用户信息并保存
     */
    saveUser() {
      // get({
      //   url: baseUserInfoApi.getLoginInfo,
      //   headers: {
      //     Authorization: 'Bearer ' + this.accessToken,
      //   },
      // }).then((res: any) => {
      //   const userInfo = res.data
      //   this.userId = userInfo.userId
      //   this.roleId = userInfo.roleId
      //   this.userName = userInfo.userName
      //   this.nickName = userInfo.userNick
      //   this.avatar = userInfo.avatar || defaultAvatar
      // })
    },
    isTokenExpire() {
      //这里失效直接跳转到登录页
      return !this.accessToken
    },
    changeNickName(newNickName: string) {
      this.userNick = newNickName
    },
    async onRefreshToken() {
      return true
      // const refreshResult = await post({
      //   url: refresh + '/' + this.refreshToken,
      // }).catch(() => {
      //   //刷新失败,清空缓存
      //   this.$reset()
      //   localStorage.clear()
      //   sessionStorage.clear()
      // })

      // if (refreshResult.data) {
      //   this.saveToken(refreshResult.data as UserState)
      //   return true
      // }

      // return false
    },
    logout() {
      return new Promise<void>((resolve) => {
        this.$reset()
        localStorage.clear()
        sessionStorage.clear()
        resolve()
      })
    },
  },
  presist: {
    enable: true,
    resetToState: true,
    option: {
      exclude: ['userId'],
    },
  },
})

export default useUserStore

export function useUserStoreContext() {
  return useUserStore(store)
}
