import { AxiosResponse } from 'axios'
import { App } from 'vue'
import request from './axios.config'

export interface HttpOption {
  url: string
  data?: any
  method?: string
  headers?: any
  beforeRequest?: () => void
  afterRequest?: () => void
}

//接口返回通用类型
export interface Response<T = any> {
  code: number
  msg: string
  data: T
  isSuccess: boolean
}

function http<T = any>({ url, data, method, headers, beforeRequest, afterRequest }: HttpOption) {
  const successHandler = (res: AxiosResponse<Response<T>>) => {
    //status 是http状态码
    if (res.status == 200) {
      if (res.data.isSuccess) {
        return res.data
      }

      window.$message.error(res.data.msg)

      //NMessage.error(res.data.msg)
      //throw new Error(res.data.errorMsg || '请求失败')
    }
    throw new Error(res.data.msg || '请求失败，未知异常')
  }
  const failHandler = (error: Response<Error>) => {
    afterRequest && afterRequest()
    if (error.code == 401) {
      window.$message.error('请刷新页面或重新登录')
      //特殊抛出去
      throw new Error('401')
    }

    window.$message.error(error.msg)
    throw new Error(error.msg || '请求失败，未知异常')
  }
  beforeRequest && beforeRequest()
  method = method || 'GET'
  const params = Object.assign(typeof data === 'function' ? data() : data || {}, {})

  switch (method) {
    case 'GET': {
      return request.get(url, { params, headers: headers }).then(successHandler, failHandler)
    }
    case 'DELETE': {
      return request
        .delete(url, { headers: headers, data: params })
        .then(successHandler, failHandler)
    }
    case 'PATCH': {
      return request.patch(url, params, { headers: headers }).then(successHandler, failHandler)
    }
    case 'PUT': {
      return request.put(url, params, { headers: headers }).then(successHandler, failHandler)
    }
    default: {
      return request.post(url, params, { headers: headers }).then(successHandler, failHandler)
    }
  }
}

export function get<T = any>({
  url,
  data,
  method = 'GET',
  headers,
  beforeRequest,
  afterRequest,
}: HttpOption): Promise<Response<T>> {
  return http<T>({
    url,
    method,
    data,
    headers,
    beforeRequest,
    afterRequest,
  })
}

export function post<T = any>({
  url,
  data,
  method = 'POST',
  headers,
  beforeRequest,
  afterRequest,
}: HttpOption): Promise<Response<T>> {
  return http<T>({
    url,
    method,
    data,
    headers,
    beforeRequest,
    afterRequest,
  })
}

//delete
export function sendDelete<T = any>({
  url,
  data,
  method = 'DELETE',
  headers,
  beforeRequest,
  afterRequest,
}: HttpOption): Promise<Response<T>> {
  return http<T>({
    url,
    method,
    data,
    headers,
    beforeRequest,
    afterRequest,
  })
}

export function sendHttpWithCors(
  method: string,
  url: string,
  params?: any,
  headers?: any,
  successHandler?: any,
  failHandler?: any
) {
  switch (method) {
    case 'GET': {
      return request.get(url, { params, headers: headers }).then(successHandler, failHandler)
    }
    case 'DELETE': {
      return request
        .delete(url, { headers: headers, data: params })
        .then(successHandler, failHandler)
    }
    case 'PATCH': {
      return request.patch(url, params, { headers: headers }).then(successHandler, failHandler)
    }
    case 'PUT': {
      return request.put(url, params, { headers: headers }).then(successHandler, failHandler)
    }
    default: {
      return request.post(url, params, { headers: headers }).then(successHandler, failHandler)
    }
  }
}

/**
 * 下载blob格式文件 仅get
 * @param param0
 * @returns
 */
export function downloadFile({ url, data, headers, afterRequest }: HttpOption) {
  const params = Object.assign(typeof data === 'function' ? data() : data || {}, {})
  return request
    .get(url, { params, headers: headers, responseType: 'blob' })
    .then(afterRequest)
    .catch((res) => {
      window.$message.error('下载文件异常,请联系管理员')
    })
}

function install(app: App): void {
  app.config.globalProperties.$http = http

  app.config.globalProperties.$get = get

  app.config.globalProperties.$post = post

  app.config.globalProperties.$delete = sendDelete
}

export default {
  install,
  get,
  post,
  sendDelete,
  sendHttpWithCors,
  downloadFile,
}
