import type { ClientTypeEnum } from '@/services/constants'
import { getValidSupplierClientAPI } from '@/services/supplierClient'
import type { GetValidSupplierClientDto } from '@/types/company'
import { defineStore } from 'pinia'
import { ref } from 'vue'

//客户|供应商
export const useSupplierClientStore = defineStore('supplier-client', () => {
  const valiedClientArray = ref<GetValidSupplierClientDto[]>([])

  //初始化
  const init = async (clientType: ClientTypeEnum) => {
    var res = await getValidSupplierClientAPI(clientType)
    valiedClientArray.value = res.data
  }

  const getItem = (index: number) => {
    return valiedClientArray.value[index]
  }

  return {
    valiedClientArray,
    init,
    getItem,
  }
})
