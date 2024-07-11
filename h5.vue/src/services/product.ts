import type { PageResult } from '@/types/global'
import type { QueryValidProductInput, QueryValidProductDto } from '@/types/product'
import { http } from '@/utils/http'
/**
 * 查询有效产品列表
 * @param input query
 */
export const queryValidProductAPI = (input: QueryValidProductInput) => {
  return http<QueryValidProductDto[]>({
    method: 'GET',
    url: `/api/mes-manager/Product/query-valid`,
    data: input,
  })
}

/**
 * 查询产品分页
 * @param input query
 */
export const queryPageProductApi = (input: any) => {
  return http<PageResult<any>>({
    method: 'GET',
    url: `/api/mes-manager/Product/query`,
    data: input,
  })
}

/**
 * 创建|修改产品
 * @param input query
 */
export const createUpdateProductApi = (input: any) => {
  return http<any>({
    method: 'POST',
    url: `/api/mes-manager/Product/create-or-update`,
    data: input,
  })
}

/**
 * 启用|禁用产品
 * @param input query
 */
export const banOrEnableProductApi = (id: string) => {
  return http<any>({
    method: 'DELETE',
    url: `/api/mes-manager/Product/ban-or-enable/${id}`,
  })
}

/**
 * 查询产品分类分页
 * @param input query
 */
export const queryPageProductTypeApi = (input: any) => {
  return http<PageResult<any>>({
    method: 'GET',
    url: `/api/mes-manager/ProductType/query`,
    data: input,
  })
}

/**
 * 查询有效产品分类
 * @param input query
 */
export const queryValidProductTypeApi = () => {
  return http<any[]>({
    method: 'GET',
    url: `/api/mes-manager/ProductType/query-valid`,
  })
}

/**
 * 创建|修改产品分类
 * @param input query
 */
export const createUpdateProductTypeApi = (input: any) => {
  return http<any>({
    method: 'POST',
    url: `/api/mes-manager/ProductType/create-or-update`,
    data: input,
  })
}

/**
 * 启用|禁用产品分类
 * @param input query
 */
export const banOrEnableApi = (id: string) => {
  return http<any>({
    method: 'DELETE',
    url: `/api/mes-manager/ProductType/ban-or-enable/${id}`,
  })
}

/**
 * 普通用户-查询有效产品列表
 * @param input query
 */
export const queryValidProductWithNormalApi = (input: any) => {
  return http<any[]>({
    method: 'GET',
    url: `/api/mes-login/Product/query-valid`,
    data: input,
  })
}

/**
 * 普通用户-获取产品工序
 * @param input query
 */
export const getProductProcessWithNormalApi = (id: string) => {
  return http<any[]>({
    method: 'GET',
    url: `/api/mes-login/Product/get-product-process/${id}`,
  })
}
