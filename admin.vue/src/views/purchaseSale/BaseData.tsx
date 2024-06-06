import { renderInput, renderSelect } from '@/hooks/form'
import useCompanyCacheStore from '@/store/modules/company'
import { baseRemarkLength } from '@/store/types'
import { FormItem } from '@/types/components'
import { NDatePicker } from 'naive-ui'
import { h, ref } from 'vue'

//创建订单基础
export const BaseCreateOrderFormOptions = [
  {
    label: '经办人',
    key: 'managerUserId',
    value: ref(null),
    render: (formItem) => {
      const useCompanyCache = useCompanyCacheStore()
      return renderSelect(formItem.value, useCompanyCache.cachedItems, {
        placeholder: '经办人 不选择就是自己',
        filterable: true,
      })
    },
  },
  {
    label: '订单日期',
    key: 'orderDate',
    value: ref(null),
    span: 1,
    render: (formItem: any) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd 00:00:00',
        defaultFormattedValue: formItem.value.value,
        placeholder: '订单日期，不填为当天',
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
    label: '备注',
    key: 'orderRemark',
    value: ref(null),
    span: 3, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '订单备注',
        maxlength: baseRemarkLength,
        type: 'textarea',
      }),
  },
] as Array<FormItem>
