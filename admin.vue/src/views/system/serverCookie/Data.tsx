import { FormItem } from '@/types/components'
import { ref } from 'vue'
import { renderInput, renderSelect } from '@/hooks/form'
import { getListByEnum } from '@/utils'
import { ServerCookieStatusEnum } from '@/store/types'
import useSubAccountListStore from '@/store/modules/sub-account-list'

//编辑|新增
export const EditFormOptions = [
  {
    hidden: true,
    key: 'id',
    value: ref(null),
  },
  {
    label: '子账号',
    key: 'subAccountId',
    value: ref(null),
    render: (formItem) => {
      const useSubAccountList = useSubAccountListStore()
      return renderSelect(formItem.value, useSubAccountList.subAccountList, {
        placeholder: '请选择子账号',
        filterable: true,
      })
    },
  },
  {
    label: '服务器Ip',
    key: 'serverIp',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '服务器Ip',
      }),
  },
  {
    label: 'Cookie',
    key: 'cookieInfo',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: 'Cookie',
        type: 'textarea',
      }),
  },
] as Array<FormItem>

//弹窗搜索
export const SearchFormOptions = [
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
      }),
  },
  {
    label: '状态',
    key: 'serverCookieStatus',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(ServerCookieStatusEnum), {
        clearable: true,
      }),
  },
] as Array<FormItem>
