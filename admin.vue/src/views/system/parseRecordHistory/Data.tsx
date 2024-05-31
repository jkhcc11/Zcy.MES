import { FormItem } from '@/types/components'
import { h, ref } from 'vue'
import { renderInput, renderSelect } from '@/hooks/form'
import useSubAccountListStore from '@/store/modules/sub-account-list'
import { RecordHistoryTypeEnum } from '@/store/types'
import { getListByEnum } from '@/utils'
import { NDatePicker, NSlider } from 'naive-ui'

const today = new Date()
const startDate = new Date(today)
startDate.setDate(startDate.getDate() - 7)

//弹窗搜索
export const SearchFormOptions = [
  {
    label: '子账号',
    key: 'subAccountId',
    value: ref(null),
    render: (formItem: any) => {
      const useSubAccountStore = useSubAccountListStore()
      return renderSelect(formItem.value, useSubAccountStore.subAccountList, {
        placeholder: '请选择子账号',
        filterable: true,
        clearable: true,
      })
    },
  },
  {
    label: '类型',
    key: 'recordHistoryType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(RecordHistoryTypeEnum), {
        clearable: true,
      }),
  },
  {
    label: '开始',
    key: 'startTime',
    value: ref(
      startDate
        .toLocaleDateString('zh-CN', {
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
        })
        .replace(/\//g, '-')
    ),
    render: (formItem) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd',
        defaultFormattedValue: formItem.value.value,
        onUpdateFormattedValue: function (newVal: any) {
          formItem.value.value = newVal
        },
      })
    },
  },
  {
    label: '结束',
    key: 'endTime',
    value: ref(
      today
        .toLocaleDateString('zh-CN', {
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
        })
        .replace(/\//g, '-')
    ),
    render: (formItem) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd',
        defaultFormattedValue: formItem.value.value,
        onUpdateFormattedValue: function (newVal: any) {
          formItem.value.value = newVal
        },
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
      }),
  },
] as Array<FormItem>

//Top弹窗搜索
export const TopSearchFormOptions = [
  {
    label: '子账号',
    key: 'subAccountId',
    value: ref(null),
    render: (formItem: any) => {
      const useSubAccountStore = useSubAccountListStore()
      return renderSelect(formItem.value, useSubAccountStore.subAccountList, {
        placeholder: '请选择子账号',
        filterable: true,
        clearable: true,
      })
    },
  },
  {
    label: '类型',
    key: 'recordHistoryType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(RecordHistoryTypeEnum), {
        clearable: true,
      }),
  },
  {
    label: 'Top',
    key: 'top',
    value: ref(10),
    render: (formItem) => {
      return h(NSlider, {
        value: formItem.value.value,
        step: 5,
        min: 1,
        max: 100,
        onUpdateValue: function (nVal: any) {
          formItem.value.value = nVal
        },
      })
    },
  },
  {
    label: '开始',
    key: 'startTime',
    value: ref(
      startDate
        .toLocaleDateString('zh-CN', {
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
        })
        .replace(/\//g, '-')
    ),
    render: (formItem) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd',
        defaultFormattedValue: formItem.value.value,
        onUpdateFormattedValue: function (newVal: any) {
          formItem.value.value = newVal
        },
      })
    },
  },
  {
    label: '结束',
    key: 'endTime',
    value: ref(
      today
        .toLocaleDateString('zh-CN', {
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
        })
        .replace(/\//g, '-')
    ),
    render: (formItem) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd',
        defaultFormattedValue: formItem.value.value,
        onUpdateFormattedValue: function (newVal: any) {
          formItem.value.value = newVal
        },
      })
    },
  },
] as Array<FormItem>

//天汇总搜索
export const DaySumSearchFormOptions = [
  {
    label: '子账号',
    key: 'subAccountId',
    value: ref(null),
    render: (formItem: any) => {
      const useSubAccountStore = useSubAccountListStore()
      return renderSelect(formItem.value, useSubAccountStore.subAccountList, {
        placeholder: '请选择子账号',
        filterable: true,
        clearable: true,
      })
    },
  },
  {
    label: '类型',
    key: 'recordHistoryType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(RecordHistoryTypeEnum), {
        clearable: true,
      }),
  },
  {
    label: '开始',
    key: 'startTime',
    value: ref(
      startDate
        .toLocaleDateString('zh-CN', {
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
        })
        .replace(/\//g, '-')
    ),
    render: (formItem) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd',
        defaultFormattedValue: formItem.value.value,
        onUpdateFormattedValue: function (newVal: any) {
          formItem.value.value = newVal
        },
      })
    },
  },
  {
    label: '结束',
    key: 'endTime',
    value: ref(
      today
        .toLocaleDateString('zh-CN', {
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
        })
        .replace(/\//g, '-')
    ),
    render: (formItem) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd',
        defaultFormattedValue: formItem.value.value,
        onUpdateFormattedValue: function (newVal: any) {
          formItem.value.value = newVal
        },
      })
    },
  },
] as Array<FormItem>
