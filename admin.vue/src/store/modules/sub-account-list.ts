import { get } from '@/api/http'
import { parseUserApi } from '@/api/url'
import { defineStore } from 'pinia'
import useCookieTypeStore from './cookie-type'

const useSubAccountListStore = defineStore('sub-account-list', {
  state: () => {
    return {
      subAccountList: [] as { lable: string; value: string; typeId: string }[],
    }
  },
  getters: {
    getSubAccountByCookeTypeId: (state) => {
      return (typeId: string): any[] =>
        state.subAccountList.filter((item) => item.typeId === typeId)
    },
  },
  actions: {
    /**
     * 初始化
     */
    async init() {
      if (this.subAccountList.length > 0) {
        //有值 不重新获取
        return
      }

      const getSubAccount = await get({
        url: parseUserApi.getAllSubAccount,
      })

      this.subAccountList = getSubAccount.data.map((item: any) => {
        return {
          label: item.alias,
          value: item.subAccountId,
          typeId: item.subAccountTypeId,
        }
      })
    },
    /**
     * 根据子账号Id判断是否为天翼
     * @param subAccountId 子账号Id
     */
    isTyTypeBySubAccountId(subAccountId: string) {
      const currentSubAccountInfo = this.subAccountList.filter((it) => it.value == subAccountId)
      if (currentSubAccountInfo && currentSubAccountInfo.length == 1) {
        const useCookieType = useCookieTypeStore()
        return useCookieType.tyTypeId == currentSubAccountInfo[0].typeId
      }

      return false
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

export default useSubAccountListStore
