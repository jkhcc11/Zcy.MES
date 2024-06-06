import { renderInput, renderMoneyInput, renderSelect } from '@/hooks/form'
import useProductTypeCacheStore from '@/store/modules/product-type'
import {
  BillingTypeEnum,
  BooleanEnum,
  CraftTypeEnum,
  ProductTypeEnum,
  PublicStatusEnum,
  baseRemarkLength,
} from '@/store/types'
import { FormItem } from '@/types/components'
import { getListByEnum } from '@/utils'
import { ref } from 'vue'

//产品分类搜索
export const SearchProductTypeOptions = [
  {
    label: '状态',
    key: 'typeStatus',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(PublicStatusEnum), {
        placeholder: '分类状态',
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
] as Array<FormItem>

//创建产品分类
export const CreateProductTypeFormOptions = [
  {
    key: 'id',
    value: ref(null),
    hidden: true,
  },
  {
    label: '分类名称',
    key: 'typeName',
    value: ref(null),
    required: true,
    span: 3,
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '产品分类名称',
        maxlength: 10,
        showCount: true,
      }),
  },
] as Array<FormItem>

//产品工艺搜索
export const SearchProductCraftOptions = [
  {
    label: '状态',
    key: 'craftStatus',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(PublicStatusEnum), {
        placeholder: '状态',
        clearable: true,
      }),
  },
  {
    label: '工艺类型',
    key: 'craftType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(CraftTypeEnum), {
        placeholder: '工艺类型',
        clearable: true,
      }),
  },
  {
    label: '计费类型',
    key: 'billingType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(BillingTypeEnum), {
        placeholder: '计费类型',
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
] as Array<FormItem>

//创建产品工艺
export const CreateProductCraftFormOptions = [
  {
    key: 'id',
    value: ref(null),
    hidden: true,
  },
  {
    label: '工艺名称',
    key: 'craftName',
    value: ref(null),
    required: true,
    span: 2,
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '工艺名称',
        maxlength: 15,
        showCount: true,
      }),
  },
  {
    label: '基础价格',
    key: 'unitPrice',
    value: ref(0.01),
    required: true,
    span: 1,
    reset: function (formItem: any) {
      formItem.value.value = 0.01
    },
    render: (formItem) => {
      return renderMoneyInput(formItem.value, {
        placeholder: '工序基础价格',
      })
    },
  },
  {
    label: '工艺类型',
    key: 'craftType',
    value: ref(null),
    required: true,
    span: 2,
    render: (formItem) => renderSelect(formItem.value, getListByEnum(CraftTypeEnum)),
  },
  {
    label: '计费类型',
    key: 'billingType',
    value: ref(null),
    required: true,
    span: 1,
    render: (formItem) => renderSelect(formItem.value, getListByEnum(BillingTypeEnum)),
  },

  {
    label: '备注',
    key: 'remark',
    value: ref(null),
    span: 3, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '工艺备注',
        maxlength: baseRemarkLength,
        type: 'textarea',
      }),
  },
] as Array<FormItem>

//产品搜索
export const SearchProductOptions = [
  {
    label: '产品分类',
    key: 'productTypeId',
    value: ref(null),
    render: (formItem) => {
      const useProductTypeCache = useProductTypeCacheStore()
      return renderSelect(formItem.value, useProductTypeCache.cachedItems, {
        placeholder: '产品分类',
        filterable: true,
        clearable: true,
      })
    },
  },
  {
    label: '状态',
    key: 'productStatus',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(PublicStatusEnum), {
        placeholder: '状态',
        clearable: true,
      }),
  },
  {
    label: '产品类型',
    key: 'productType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(ProductTypeEnum), {
        placeholder: '产品类型',
        clearable: true,
      }),
  },
  {
    label: '是否散件',
    key: 'isLoose',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(BooleanEnum), {
        placeholder: '是否散件',
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
] as Array<FormItem>

//产品工艺Item
export interface ProductProcesses {
  orderBy: number
  craftId: string
  craftName: string
  billingType: number
  craftPrice: number
  processingPrice: number
  sumPrice: number
}
