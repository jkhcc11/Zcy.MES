import { renderInput, renderMoneyInput, renderSelect } from '@/hooks/form'
import useClientCacheStore from '@/store/modules/client-cache'
import { IncomeTypeEnum, AccountTypeEnum, recordNameLength } from '@/store/types'
import { FormItem } from '@/types/components'
import { getListByEnum } from '@/utils'
import { NDatePicker } from 'naive-ui'
import { h, ref } from 'vue'

//收支搜索
export const SearchIncomeRecordOptions = [
  {
    label: '收支类型',
    key: 'incomeType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(IncomeTypeEnum), {
        placeholder: '收支类型',
        clearable: true,
      }),
  },
  {
    label: '账户类型',
    key: 'accountType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(AccountTypeEnum), {
        placeholder: '账户类型',
        clearable: true,
      }),
  },
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

//创建收支记录
export const CreateIncomeRecordFormOptions = [
  {
    key: 'id',
    value: ref(null),
    hidden: true,
  },
  {
    label: '款项名称',
    key: 'recordName',
    value: ref(null),
    required: true,
    span: 3, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '例如：购买xx设备',
        maxlength: recordNameLength,
        showCount: true,
      }),
  },
  {
    label: '收支类型',
    key: 'incomeType',
    value: ref(null),
    required: true,
    span: 1,
    render: (formItem) => renderSelect(formItem.value, getListByEnum(IncomeTypeEnum)),
  },
  {
    label: '账户类型',
    key: 'accountType',
    value: ref(null),
    required: true,
    span: 1,
    render: (formItem) => renderSelect(formItem.value, getListByEnum(AccountTypeEnum)),
  },
  {
    label: '金额',
    key: 'money',
    value: ref(0.01),
    required: true,
    span: 1,
    reset: function (formItem: any) {
      formItem.value.value = 0.01
    },
    render: (formItem) => {
      return renderMoneyInput(formItem.value, {
        placeholder: '金额',
      })
    },
  },
  {
    label: '付款人',
    key: 'managerUser',
    value: ref(null),
    required: true,
    span: 2, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '付款人 例如：xx 付款',
      }),
  },
  {
    label: '消费日期',
    key: 'recordDate',
    value: ref(null),
    span: 1, //grid-item 生效
    render: (formItem: any) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd 00:00:00',
        defaultFormattedValue: formItem.value.value,
        placeholder: '消费日期，不填为当天',
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
    key: 'remark',
    value: ref(null),
    span: 3, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '款项说明',
        type: 'textarea',
      }),
  },
] as Array<FormItem>

//收款搜索
export const SearchProceedsRecordOptions = [
  {
    label: '客户',
    key: 'supplierClientId',
    value: ref(null),
    render: (formItem) => {
      const useClientCache = useClientCacheStore()
      return renderSelect(formItem.value, useClientCache.cachedItems, {
        placeholder: '请选择客户',
        filterable: true,
        clearable: true,
      })
    },
  },
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

//创建收款记录
export const CreateProceedsRecordFormOptions = [
  {
    key: 'id',
    value: ref(null),
    hidden: true,
  },
  {
    label: '收款名称',
    key: 'proceedsRecordName',
    value: ref(null),
    required: true,
    span: 3, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '例如：xx货款',
        maxlength: recordNameLength,
        showCount: true,
      }),
  },
  {
    label: '客户',
    key: 'supplierClientId',
    value: ref(null),
    required: true,
    span: 2,
    render: (formItem) => {
      const useClientCache = useClientCacheStore()
      return renderSelect(formItem.value, useClientCache.cachedItems, {
        placeholder: '请选择客户',
        filterable: true,
      })
    },
  },
  {
    label: '账户类型',
    key: 'accountType',
    value: ref(null),
    required: true,
    span: 1,
    render: (formItem) => renderSelect(formItem.value, getListByEnum(AccountTypeEnum)),
  },

  {
    label: '金额',
    key: 'money',
    value: ref(0.01),
    required: true,
    span: 1,
    reset: function (formItem: any) {
      formItem.value.value = 0.01
    },
    render: (formItem) => {
      return renderMoneyInput(formItem.value, {
        placeholder: '金额',
      })
    },
  },
  {
    label: '收款日期',
    key: 'recordDate',
    value: ref(null),
    span: 2, //grid-item 生效
    render: (formItem: any) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd 00:00:00',
        defaultFormattedValue: formItem.value.value,
        placeholder: '收款日期，不填为当天',
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
    key: 'remark',
    value: ref(null),
    span: 3, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '款项说明',
        type: 'textarea',
      }),
  },
] as Array<FormItem>
