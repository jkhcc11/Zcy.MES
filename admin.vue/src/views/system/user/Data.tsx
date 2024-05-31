import { FormItem } from '@/types/components'
import { ref } from 'vue'
import { renderInput } from '@/hooks/form'

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
