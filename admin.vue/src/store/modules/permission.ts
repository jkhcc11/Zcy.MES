import { RouteRecordRaw } from 'vue-router'
import { defineStore } from 'pinia'
import useUserStore from './user'
import router from '@/router'
import { baseAddress, getMenuListByRoleId } from '@/api/url'
import { get } from '@/api/http'
import defaultRoutes from '@/router/routes/default-routes'
import { findRootPathRoute, generatorRoutes, mapTwoLevelRouter } from '../help'
import { constantRoutes } from '@/router/routes/constants'

const usePermissionStore = defineStore('permission-route', {
  state: () => {
    return {
      permissionRoutes: [] as RouteRecordRaw[],
    }
  },
  getters: {
    getPermissionSideBar(state) {
      return state.permissionRoutes.filter((it) => {
        return it.meta && !it.meta.hidden
      })
    },
    getPermissionSplitTabs(state) {
      return state.permissionRoutes.filter((it) => {
        return it.meta && !it.meta.hidden && it.children && it.children.length > 0
      })
    },
  },
  actions: {
    async getRoutes(data: { token: string }) {
      const userStore = useUserStore()
      try {
        if (getMenuListByRoleId) {
          // const res = await post({
          //   url: baseAddress + getMenuListByRoleId,
          //   // 在实际的开发中，这个地方可以换成 token，让后端解析用户信息获取 userId 和 roleId，前端可以不用传 userId 和 roleId。
          //   // 这样可以增加安全性
          //   data,
          // })
          const res = await get({
            url: baseAddress + getMenuListByRoleId,
            headers: {
              Authorization: 'Bearer ' + data.token,
            },
          })

          return generatorRoutes(res.data)
        } else {
          return generatorRoutes(defaultRoutes)
        }
      } catch (error: any) {
        if (error.message && error.message == '401') {
          //401 重新拉取目录
          const refreshResult = await userStore.onRefreshToken()
          if (refreshResult) {
            const res = await get({
              url: baseAddress + getMenuListByRoleId,
              headers: {
                Authorization: 'Bearer ' + userStore.accessToken,
              },
            })

            return generatorRoutes(res.data)
          } else {
            //刷新失败 直接清空
            userStore.logout().then(() => {
              window.location.reload()
            })
          }
          //console.log('refreshResult', refreshResult)
        }

        // window.$dialog.error({
        //   title: '错误',
        //   content: '加载菜单失败',
        //   positiveText: '重新登录',
        //   onPositiveClick: () => {
        //     userStore.logout().then(() => {
        //       window.location.reload()
        //     })
        //   },
        // })
        // console.log(
        //   '路由加载失败了，请清空一下Cookie和localStorage，重新登录；如果已经采用真实接口的，请确保菜单接口地址真实可用并且返回的数据格式和mock中的一样'
        // )
        return []
      }
    },
    async initPermissionRoute() {
      const userStore = useUserStore()
      // 加载路由
      const accessRoutes = await this.getRoutes({
        token: userStore.accessToken,
      })
      const mapRoutes = mapTwoLevelRouter(accessRoutes)
      mapRoutes.forEach((it: any) => {
        router.addRoute(it)
      })
      // 配置 `/` 路由的默认跳转地址
      router.addRoute({
        path: '/',
        redirect: findRootPathRoute(accessRoutes),
        meta: {
          hidden: true,
        },
      })
      // 这个路由一定要放在最后
      router.addRoute({
        path: '/:pathMatch(.*)*',
        redirect: '/404',
        meta: {
          hidden: true,
        },
      })
      this.permissionRoutes = [...constantRoutes, ...accessRoutes]
    },
    isEmptyPermissionRoute() {
      return !this.permissionRoutes || this.permissionRoutes.length === 0
    },
    reset() {
      this.$reset()
    },
  },
})

export default usePermissionStore
