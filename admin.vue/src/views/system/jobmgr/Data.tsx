import { renderInput, renderSelect } from '@/hooks/form'
import { ConfigHttpMethodEnum, SearchConfigStatusEnum } from '@/store/types'
import { FormItem } from '@/types/components'
import { getListByEnum } from '@/utils'
import { ref } from 'vue'

//编辑|新增
export const CreateJobFormOptions = [
  {
    hidden: true,
    key: 'id',
    value: ref(null),
  },
  {
    label: '请求Url',
    key: 'requestUrl',
    required: true,
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '请求Url',
      }),
  },
  {
    label: '类型',
    key: 'httpMethod',
    value: ref(null),
    validator(value, message) {
      if (value) {
        return true
      }

      if (value == 0) {
        return true
      }

      message.error('请选择类型')
    },
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(ConfigHttpMethodEnum), {
        placeholder: '类型',
        clearable: true,
      }),
  },
  {
    label: '状态',
    key: 'searchConfigStatus',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(SearchConfigStatusEnum), {
        placeholder: '状态',
        clearable: true,
      }),
  },
  {
    label: 'Cron',
    key: 'urlCron',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: 'Cron表达式',
      }),
  },
  {
    label: 'Cookie',
    key: 'cookie',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: 'Cookie',
        type: 'textarea',
      }),
  },
  {
    label: 'Post数据',
    key: 'postData',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: 'Post数据',
      }),
  },
  {
    label: '来源',
    key: 'referer',
    value: ref(null),
    span: 2,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '来源Url',
      }),
  },
  {
    label: '成功标识',
    key: 'successFlag',
    value: ref(null),
    span: 1,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '成功标识',
      }),
  },
  {
    label: '成功标识Xpath',
    key: 'msgXpath',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '成功标识Xpath',
      }),
  },
] as Array<FormItem>

//搜索Job
export const SearchJobOptions = [
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
] as Array<FormItem>
