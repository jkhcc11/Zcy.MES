import { renderInput, renderMoneyInput, renderSelect } from '@/hooks/form'
import useCompanyCacheStore from '@/store/modules/company'
import useProductCacheStore from '@/store/modules/product'
import { baseRemarkLength } from '@/store/types'
import { FormItem } from '@/types/components'
import { CascaderOption, NCascader, NDatePicker } from 'naive-ui'
import { h, ref } from 'vue'

//报工搜索
export const SearchReportWorkOptions = [
  {
    label: '员工',
    key: 'employeeId',
    value: ref(null),
    render: (formItem) => {
      const useCompanyCache = useCompanyCacheStore()
      return renderSelect(formItem.value, useCompanyCache.cachedItems, {
        placeholder: '请选择员工',
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
    label: '产品工序',
    key: 'productProcessId',
    value: ref(null),
    render: (formItem) => {
      const useProductCache = useProductCacheStore()
      return h(NCascader, {
        options: useProductCache.cachedItems,
        clearable: true,
        showPath: true,
        placeholder: '选择产品工序',
        value: formItem.value.value,
        expandTrigger: 'click',
        remote: true,
        onUpdateValue: function (newVal: any) {
          formItem.value.value = newVal
          //console.log('工序', newVal)
        },
        onLoad: async function (option: CascaderOption) {
          //console.log('工序c', option)
          option.children = await useProductCache.getProductProcesses(option.value + '')
          // return new Promise<void>(async (resolve) => {
          //   resolve()
          //   // window.setTimeout(() => {
          //   //   option.children = getChildren(option)
          //   //   resolve()
          //   // }, 1000)
          // })
        },
      })
    },
  },
  {
    label: '开始日期',
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
    label: '结束日期',
    key: 'endTime',
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
] as Array<FormItem>

//创建报工
export const CreateReportWorkFormOptions = [
  {
    key: 'id',
    value: ref(null),
    hidden: true,
  },
  {
    label: '员工',
    key: 'employeeId',
    value: ref(null),
    required: true,
    span: 2,
    render: (formItem) => {
      const useCompanyCache = useCompanyCacheStore()
      return renderSelect(formItem.value, useCompanyCache.cachedItems, {
        placeholder: '员工',
        filterable: true,
      })
    },
  },
  {
    label: '报工日期',
    key: 'reportWorkDate',
    value: ref(null),
    span: 1, //grid-item 生效
    render: (formItem: any) => {
      return h(NDatePicker, {
        type: 'date',
        valueFormat: 'yyyy-MM-dd 00:00:00',
        defaultFormattedValue: formItem.value.value,
        placeholder: '报工日期，不填为当天',
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
    label: '产品工序',
    key: 'productProcessId',
    value: ref(null),
    required: true,
    span: 2,
    render: (formItem) => {
      const useProductCache = useProductCacheStore()
      return h(NCascader, {
        options: useProductCache.cachedItems,
        clearable: true,
        showPath: true,
        placeholder: '选择产品工序',
        value: formItem.value.value,
        expandTrigger: 'click',
        remote: true,
        onUpdateValue: function (newVal: any) {
          formItem.value.value = newVal
          //console.log('工序', newVal)
        },
        onLoad: async function (option: CascaderOption) {
          //console.log('工序c', option)
          option.children = await useProductCache.getProductProcesses(option.value + '')
          // return new Promise<void>(async (resolve) => {
          //   resolve()
          //   // window.setTimeout(() => {
          //   //   option.children = getChildren(option)
          //   //   resolve()
          //   // }, 1000)
          // })
        },
      })
      // const useClientCache = useClientCacheStore()
      // return renderSelect(formItem.value, useClientCache.cachedItems, {
      //   placeholder: '产品工序',
      //   filterable: true,
      // })
    },
  },
  {
    label: '工作时长',
    key: 'wordDuration',
    value: ref(1.0),
    required: true,
    span: 1,
    reset: function (formItem: any) {
      formItem.value.value = 1.0
    },
    render: (formItem) => {
      return renderMoneyInput(formItem.value, {
        placeholder: '工作时长',
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
        placeholder: '报工说明',
        type: 'textarea',
        maxlength: baseRemarkLength,
        showCount: true,
      }),
  },
] as Array<FormItem>

//调整价格
export const UpdateReportWorkFormOptions = [
  {
    key: 'id',
    value: ref(null),
    hidden: true,
  },
  {
    label: '实际结算',
    key: 'actualSettlementPrice',
    value: ref(null),
    required: true,
    span: 3,
    render: (formItem) => {
      return renderMoneyInput(formItem.value, {
        placeholder: '实际结算',
        precision: 2,
        min: 0.01,
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
        placeholder: '报工说明',
        type: 'textarea',
      }),
  },
] as Array<FormItem>

//批量创建报工Item
export interface BatchCreateReportWorkItem {
  productProcessId: string
  productName: string
  craftName: string
  wordDuration: number
  remark: string
  billingType: number
}
