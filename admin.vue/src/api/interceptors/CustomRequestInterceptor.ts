import { useUserStoreContext } from '@/store/modules/user'
import { AxiosRequestConfig } from 'axios'

export default function (config: AxiosRequestConfig) {
  const useStore = useUserStoreContext()
  if (config) {
    if (!config.headers) {
      config.headers = {}
    }
    if (!config.headers['Authorization']) {
      config.headers['Authorization'] = 'Bearer ' + useStore.accessToken
    }
  }
  return config
}
