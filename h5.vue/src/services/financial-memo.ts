import type {
  CreateIncomeRecordInput,
  QueryPageIncomeRecordInput,
  QueryPageIncomeRecordDto,
  QueryPageProceedsRecordInput,
  QueryPageProceedsRecordDto,
  CreateProceedsRecordInput,
} from '@/types/financial-memo'
import type { PageResult } from '@/types/global'
import { http } from '@/utils/http'
/**
 * 查询收支列表
 * @param input query
 */
export const queryPageIncomeRecordApi = (input: QueryPageIncomeRecordInput) => {
  return http<PageResult<QueryPageIncomeRecordDto>>({
    method: 'GET',
    url: `/api/mes-manager/IncomeRecord/query`,
    data: input,
  })
}

/**
 * 获取收支记录汇总
 * @param input query
 */
export const getIncomeRecordTotalsApi = (input: QueryPageIncomeRecordInput) => {
  return http<any>({
    method: 'GET',
    url: `/api/mes-manager/IncomeRecord/get-totals`,
    data: input,
  })
}

/**
 * 创建收支
 * @param input query
 */
export const createIncomeRecordApi = (input: CreateIncomeRecordInput) => {
  return http<string>({
    method: 'PUT',
    url: `/api/mes-manager/IncomeRecord/create`,
    data: input,
  })
}

/**
 * 查询收款列表
 * @param input query
 */
export const queryPageProceedsRecordApi = (input: QueryPageProceedsRecordInput) => {
  return http<PageResult<QueryPageProceedsRecordDto>>({
    method: 'GET',
    url: `/api/mes-manager/ProceedsRecord/query`,
    data: input,
  })
}

/**
 * 获取收款汇总
 * @param input query
 */
export const getProceedsRecordTotalsApi = (input: QueryPageProceedsRecordInput) => {
  return http<any>({
    method: 'GET',
    url: `/api/mes-manager/ProceedsRecord/get-totals`,
    data: input,
  })
}

/**
 * 创建收款
 * @param input query
 */
export const createProceedsRecordApi = (input: CreateProceedsRecordInput) => {
  return http<string>({
    method: 'PUT',
    url: `/api/mes-manager/ProceedsRecord/create`,
    data: input,
  })
}
