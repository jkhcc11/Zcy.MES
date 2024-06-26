import { renderInput, renderSelect } from '@/hooks/form'
import useClientCacheStore from '@/store/modules/client-cache'
import { AccountTypeEnum, ProductTypeEnum, PublicStatusEnum } from '@/store/types'
import { FormItem } from '@/types/components'
import { getListByEnum } from '@/utils'
import { NDatePicker } from 'naive-ui'
import { h, ref } from 'vue'

//销售订单搜索
export const SearchSaleOrderOptions = [
  {
    label: '账号类型',
    key: 'accountType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(AccountTypeEnum), {
        placeholder: '账号类型',
        clearable: true,
      }),
  },

  {
    label: '客户',
    key: 'businessId',
    value: ref(null),
    render: (formItem) => {
      const useClientCache = useClientCacheStore()
      return renderSelect(formItem.value, useClientCache.cachedItems, {
        placeholder: '客户',
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

//产品筛选表格头
export const ProductFilterTableColumns = [
  {
    type: 'selection',
  },
  {
    title: '产品分类',
    key: 'productTypeName',
    fixed: 'left',
  },
  {
    title: '产品名',
    key: 'productName',
    fixed: 'left',
  },
  {
    title: '单位/规格',
    key: 'unitSpec',
    render: (rowData: any) => {
      if (rowData.isLoose) {
        //散件
        return rowData.unit
      }

      return `${rowData.specCount}${rowData.unit}/${rowData.spec}`
    },
  },
]

//产品筛选表单
export const ProductFilterForm = [
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
  {
    label: '状态',
    key: 'productStatus',
    value: ref(PublicStatusEnum.正常),
    hidden: true,
  },
  {
    label: '产品类型',
    key: 'productType',
    value: ref(ProductTypeEnum.销售产品),
    hidden: true,
  },
] as Array<FormItem>

//出货订单搜索
export const SearchShipmentOrderOptions = [
  {
    label: '客户',
    key: 'businessId',
    value: ref(null),
    render: (formItem) => {
      const useClientCache = useClientCacheStore()
      return renderSelect(formItem.value, useClientCache.cachedItems, {
        placeholder: '客户',
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

//出货订单产品筛选表单
export const ShipmentProductFilterForm = [
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
  {
    label: '状态',
    key: 'productStatus',
    value: ref(PublicStatusEnum.正常),
    hidden: true,
  },
  {
    label: '产品类型',
    key: 'productType',
    value: ref(ProductTypeEnum.加工产品),
    render: (formItem) => renderSelect(formItem.value, getListByEnum(ProductTypeEnum)),
  },
] as Array<FormItem>

//采购订单产品筛选表单
export const PurchaseProductFilterForm = [
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
  {
    label: '状态',
    key: 'productStatus',
    value: ref(PublicStatusEnum.正常),
    hidden: true,
  },
  {
    label: '产品类型',
    key: 'productType',
    value: ref(ProductTypeEnum.销售产品),
    render: (formItem) => renderSelect(formItem.value, getListByEnum(ProductTypeEnum)),
  },
] as Array<FormItem>

//采购订单搜索
export const SearchPurchaseOrderOptions = [
  {
    label: '账号类型',
    key: 'accountType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(AccountTypeEnum), {
        placeholder: '账号类型',
        clearable: true,
      }),
  },

  {
    label: '供应商',
    key: 'businessId',
    value: ref(null),
    render: (formItem) => {
      const useClientCache = useClientCacheStore()
      return renderSelect(formItem.value, useClientCache.supplierCacheItems, {
        placeholder: '供应商',
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
//销售订单Item
export interface CreateSaleOrderDetailInput {
  productId: string
  productName: string
  isLoose: boolean
  unit: string
  spec: string
  specCount: number
  count: number
  unitPrice: number
  remark: string
  sumPrice: number
}

//出货订单Item
export interface CreateShipmentOrderDetailInput {
  productId: string
  productName: string
  unit: string
  count: number
  remark: string
}
