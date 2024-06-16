import { renderInput } from '@/hooks/form'
import { FormItem } from '@/types/components'
import { NDatePicker } from 'naive-ui'
import { h, ref } from 'vue'

//产品销售报表搜索
export const SearchProductSaleOptions = [
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
  {
    label: '开始',
    key: 'startTime',
    value: ref(null),
    render: (formItem) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd 00:00:00',
        defaultFormattedValue: formItem.value.value,
        clearable: true,
        isDateDisabled: function (ts: number) {
          return ts > Date.now()
        },
        onUpdateFormattedValue: function (newVal: any) {
          formItem.value.value = newVal
        },
      })
    },
  },
  {
    label: '结束',
    key: 'endTime',
    value: ref(null),
    render: (formItem) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd 23:59:59',
        defaultFormattedValue: formItem.value.value,
        clearable: true,
        isDateDisabled: function (ts: number) {
          return ts > Date.now()
        },
        onUpdateFormattedValue: function (newVal: any) {
          formItem.value.value = newVal
        },
      })
    },
  },
] as Array<FormItem>
