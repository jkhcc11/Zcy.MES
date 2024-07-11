import type { GetCurrentCompanyValidEmployeeDto } from '@/types/company'
import { http } from '@/utils/http'

/**
 * 获取有效员工列表
 */
export const getCurrentCompanyValidEmployeeApi = () => {
  return http<GetCurrentCompanyValidEmployeeDto[]>({
    method: 'GET',
    url: `/api/mes-manager/SystemCompany/get-valid-employee`,
  })
}
