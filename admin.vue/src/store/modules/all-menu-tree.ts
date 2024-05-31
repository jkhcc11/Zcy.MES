import { get } from '@/api/http'
import { roleMenuApi } from '@/api/url'
import { defineStore } from 'pinia'

const useAllMenuTreeStore = defineStore('all-menu-tree', {
  state: () => {
    return {
      dataList: [] as { menuName: string; id: string; children: [] }[],
      oneMenu: [] as { menuName: string; menuId: string }[],
    }
  },
  getters: {
    //获取Tree 适配的数据
    getTreeData(state) {
      const transformTreeData = (data: any) => {
        return data.map((item: any) => {
          // 创建新对象，将menuName和id字段分别重命名为label和key
          const newItem = {
            label: item.menuName,
            key: item.id,
            children: null,
          }

          // 如果存在children属性且它不是null，则递归处理它
          if (item.children && Array.isArray(item.children)) {
            newItem.children = transformTreeData(item.children)
          }

          return newItem
        })
      }

      return transformTreeData(state.dataList)
    },
    //所有一级菜单Id
    getAllOneMenuIds(state) {
      return state.oneMenu.map((item) => {
        return item.menuId
      })
    },
  },
  actions: {
    async init() {
      const getAllMenu = await get({
        url: roleMenuApi.getAllMenuTree,
        data: () => {
          return {}
        },
      })

      this.dataList = getAllMenu.data

      //一级菜单
      const getAllOneMenu = await get({
        url: roleMenuApi.getAllOneMenu,
        data: () => {
          return {}
        },
      })

      this.oneMenu = getAllOneMenu.data
    },
    async refresh() {
      await this.init()
    },
    reset() {
      this.$reset()
    },
  },
})

export default useAllMenuTreeStore
