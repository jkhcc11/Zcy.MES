import { getCurrentCompanyValidEmployeeApi } from '@/services/company'
import type { GetCurrentCompanyValidEmployeeDto } from '@/types/company'
import type { FilterItem } from '@/types/global'
import { defineStore } from 'pinia'
import { ref } from 'vue'

//员工
export const useEmployeeStore = defineStore('employeeStore', () => {
  const valiedEmployeeArray = ref<GetCurrentCompanyValidEmployeeDto[]>([])
  //筛选数组 {label,value}
  const filterArray = ref<FilterItem[]>([])

  //初始化
  const init = async () => {
    var res = await getCurrentCompanyValidEmployeeApi()
    valiedEmployeeArray.value = res.data
    filterArray.value.push({
      label: '不限',
      value: '',
    } as FilterItem)
    filterArray.value.push(
      ...res.data.map((it) => {
        return {
          label: it.userNick,
          value: it.id,
        } as FilterItem
      }),
    )
  }

  const getItemById = (id: string) => {
    return valiedEmployeeArray.value.filter((it: any) => it.id == id)[0]
  }

  //获取筛选列表
  const getFilterArray = async () => {
    if (filterArray.value && filterArray.value.length > 0) {
      return filterArray
    }

    await init()
    return filterArray.value
  }

  return {
    valiedEmployeeArray,
    filterArray,
    init,
    getFilterArray,
    getItemById,
  }
})
