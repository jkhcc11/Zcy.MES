import type { PageResult } from '@/types/global'
import { http } from '@/utils/http'
/**
 * 我的报工记录
 * @param input query
 */
export const queryPageReportWorkForNormalApi = (input: any) => {
  return http<PageResult<any>>({
    method: 'GET',
    url: `/api/mes-login/ReportWork/query`,
    data: input,
  })
}

/**
 * 获取我的报工汇总
 * @param input query
 */
export const getMeReportWorkTotalsApi = (input: any) => {
  return http<any>({
    method: 'GET',
    url: `/api/mes-login/ReportWork/get-totals`,
    data: input,
  })
}

/**
 * 员工创建报工
 * @param input query
 */
export const createReportWorkWithNormalApi = (input: any) => {
  return http<any>({
    method: 'PUT',
    url: `/api/mes-login/ReportWork/create`,
    data: input,
  })
}

/**
 * 员工报工记录
 * @param input query
 */
export const queryPageReportWorkForAdminApi = (input: any) => {
  return http<PageResult<any>>({
    method: 'GET',
    url: `/api/mes-manager/ReportWork/query-for-admin`,
    data: input,
  })
}

/**
 * 获取报工汇总
 * @param input query
 */
export const getReportWorkTotalsApi = (input: any) => {
  return http<any>({
    method: 'GET',
    url: `/api/mes-manager/ReportWork/get-totals`,
    data: input,
  })
}

/**
 * 通过报工
 * @param input query
 */
export const approvedReportWorkApi = (id: string) => {
  return http<any>({
    method: 'POST',
    url: `/api/mes-manager/ReportWork/approved/${id}`,
  })
}

/**
 * 驳回报工
 * @param input query
 */
export const rejectReportWorkApi = (id: string) => {
  return http<any>({
    method: 'POST',
    url: `/api/mes-manager/ReportWork/reject/${id}`,
  })
}
