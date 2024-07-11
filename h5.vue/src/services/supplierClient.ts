import type { GetValidSupplierClientDto } from '@/types/company'
import { http } from '@/utils/http'
import { PublicStatusEnum, type ClientTypeEnum } from './constants'
import type { PageResult } from '@/types/global'
import { useUserInfoStore } from '@/stores'
/**
 * 获取有效客户|供应商列表
 * @param clientType 客户类型
 */
export const getValidSupplierClientAPI = (clientType: ClientTypeEnum) => {
  return http<GetValidSupplierClientDto[]>({
    method: 'GET',
    url: `/api/mes-manager/SupplierClient/get-open-client`,
    data: {
      clientType,
    },
  })
}

/**
 * 查询供应商|客户列表
 * @param input query
 */
export const queryPageSupplierClientApi = (input: any) => {
  return http<PageResult<any>>({
    method: 'GET',
    url: `/api/mes-manager/SupplierClient/query`,
    data: input,
  })
}

/**
 * 创建供应商|客户
 * @param input query
 */
export const createAndUpdateSupplierClientApi = (input: any) => {
  const useUserInfo = useUserInfoStore()
  return http<any>({
    method: 'POST',
    url: `/api/mes-manager/SupplierClient/create-and-update`,
    data: {
      ...input,
      clientStatus: PublicStatusEnum.正常,
      companyId: useUserInfo.userInfo?.companyId,
    },
  })
}

/**
 * 禁用
 * @param id key
 */
export const banSupplierClientApi = (id: string) => {
  return http<any>({
    method: 'DELETE',
    url: `/api/mes-manager/SupplierClient/ban/${id}`,
  })
}

/**
 * 启用
 * @param id key
 */
export const openSupplierClientApi = (id: string) => {
  return http<any>({
    method: 'POST',
    url: `/api/mes-manager/SupplierClient/open/${id}`,
  })
}
