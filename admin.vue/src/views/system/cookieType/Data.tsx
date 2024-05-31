import { FormItem } from '@/types/components'
import { ref } from 'vue'
import { renderInput } from '@/hooks/form'

//编辑|新增
export const EditFormOptions = [
  {
    hidden: true,
    key: 'id',
    value: ref(null),
  },
  {
    label: '显示文案',
    key: 'showText',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '显示文案',
      }),
  },
  {
    label: '网盘标识',
    key: 'businessFlag',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '网盘标识',
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
] as Array<FormItem>
