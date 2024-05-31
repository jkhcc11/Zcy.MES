import useUserStore from '@/store/modules/user'
import Axios, { AxiosResponse } from 'axios'
import qs from 'qs'

export const baseURL = import.meta.env.VITE_API_BASE_URL

export const CONTENT_TYPE = 'Content-Type'

export const FORM_URLENCODED = 'application/x-www-form-urlencoded; charset=UTF-8'

export const APPLICATION_JSON = 'application/json; charset=UTF-8'

export const TEXT_PLAIN = 'text/plain; charset=UTF-8'

const service = Axios.create({
  baseURL,
  timeout: 10 * 60 * 1000,
})

service.interceptors.request.use(
  (config) => {
    !config.headers && (config.headers = {})
    if (!config.headers[CONTENT_TYPE]) {
      config.headers[CONTENT_TYPE] = APPLICATION_JSON
    }
    if (config.headers[CONTENT_TYPE] === FORM_URLENCODED) {
      config.data = qs.stringify(config.data)
    }

    config.params = config.params || {}
    config.params._ = Date.now()
    return config
  },
  (error) => {
    return Promise.reject(error.response)
  }
)

service.interceptors.response.use(
  (response: AxiosResponse): AxiosResponse => {
    if (response.status === 200) {
      return response
    } else {
      throw new Error(response.status.toString())
    }
  },
  (error) => {
    if (import.meta.env.MODE === 'development') {
      console.log(error)
    }

    if (error.response && error.response.status === 401) {
      return Promise.reject({ code: 401, msg: '身份认证失效,请重新登录' })
      // const useUser = useUserStore()
      // useUser
      //   .onRefreshToken()
      //   .then((res) => {
      //     console.log('refresh', res)
      //     useUser.saveUser(res.data)
      //   })
      //   .catch((res) => {
      //     return Promise.reject({ code: 401, msg: '身份认证失效,请重新登录' })
      //   })

      // return Promise.reject({ code: 200, msg: '请刷新' })
    }

    if (error.response && error.response.status === 403) {
      return Promise.reject({ code: 403, msg: '无权限,请联系管理员' })
    }

    return Promise.reject({ code: 500, msg: '服务器异常，请稍后重试…' })
  }
)

export default service
