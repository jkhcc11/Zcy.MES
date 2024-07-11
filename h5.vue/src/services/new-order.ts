import type { PageResult } from '@/types/global'
import type {
  CreateSaleOrderInput,
  GetSaleOrderDetailDto,
  QueryPageSaleOrderDto,
  QueryPageSaleOrderInput,
} from '@/types/new-order'
import { http } from '@/utils/http'
/**
 * 获取销售订单列表
 * @param input 分页查询
 */
export const getSaleOrderAPI = (input: QueryPageSaleOrderInput) => {
  return http<PageResult<QueryPageSaleOrderDto>>({
    method: 'GET',
    url: `/api/mes-manager/SaleOrder/query`,
    data: input,
  })
}

/**
 * 获取销售订单统计
 * @param input 分页查询
 */
export const getSaleOrderTotalsApi = (input: QueryPageSaleOrderInput) => {
  return http<any>({
    method: 'GET',
    url: `/api/mes-manager/SaleOrder/get-totals`,
    data: input,
  })
}

/**
 * 获取销售订单详情
 * @param id 订单Id
 * @returns
 */
export const getSaleOrderDetail = (id: string) => {
  return http<GetSaleOrderDetailDto>({
    method: 'GET',
    url: `/api/mes-manager/SaleOrder/get-detail/${id}`,
  })
}

/**
 * 创建销售订单
 * @param input input
 * @returns
 */
export const createSaleOrderApi = (input: CreateSaleOrderInput) => {
  return http<string>({
    method: 'PUT',
    url: `/api/mes-manager/SaleOrder/create`,
    data: input,
  })
}

/**
 * 查询采购订单列表
 * @param input 分页查询
 */
export const queryPagePurchaseOrderApi = (input: any) => {
  return http<PageResult<any>>({
    method: 'GET',
    url: `/api/mes-manager/PurchaseOrder/query`,
    data: input,
  })
}

/**
 * 获取采购订单统计
 * @param input 分页查询
 */
export const getPurchaseOrderTotalsApi = (input: any) => {
  return http<any>({
    method: 'GET',
    url: `/api/mes-manager/PurchaseOrder/get-totals`,
    data: input,
  })
}

/**
 * 获取采购订单详情
 * @param id 订单Id
 * @returns
 */
export const getPurchaseOrderDetailApi = (id: string) => {
  return http<any>({
    method: 'GET',
    url: `/api/mes-manager/PurchaseOrder/get-detail/${id}`,
  })
}

/**
 * 创建采购订单
 * @param input input
 * @returns
 */
export const createPurchaseOrderApi = (input: any) => {
  return http<string>({
    method: 'PUT',
    url: `/api/mes-manager/PurchaseOrder/create`,
    data: input,
  })
}
