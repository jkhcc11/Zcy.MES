import { http } from '@/utils/http'
/**
 * 修改密码
 * @param input input
 */
export const modifyUserPwdApi = (input: any) => {
  return http<any>({
    method: 'POST',
    url: `/api/mes-login/User/modify-pwd`,
    data: input,
  })
}
