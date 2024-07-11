import type { GetLoginInfoWithWxDto, LoginResult } from '@/types/userInfo'
import { http } from '@/utils/http'

type LoginParams = {
  userName: string
  userPwd: string
}
/**
 * 传统登录-用户名+密码
 * @param input 请求参数
 */
export const postLoginAPI = (input: LoginParams) => {
  return http<LoginResult>({
    method: 'POST',
    url: '/api/mes-normal/User/login',
    data: input,
  })
}

/**
 * 小程序登录信息
 */
export const getLoginInfoWithWxAsync = () => {
  return http<GetLoginInfoWithWxDto>({
    method: 'GET',
    url: '/api/mes-login/User/get-login-info-wx',
  })
}
