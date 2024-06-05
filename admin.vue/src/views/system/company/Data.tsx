import { renderInput } from '@/hooks/form'
import { recordNameLength } from '@/store/types'
import { FormItem } from '@/types/components'
import { Ref, ref } from 'vue'

//公司搜索
export const SearchCompanyOptions = [
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

//创建公司
export const CreateCompanyFormOptions = [
  {
    key: 'id',
    value: ref(null),
    hidden: true,
  },
  {
    label: '公司名称',
    key: 'companyName',
    value: ref(null),
    required: true,
    span: 3, //grid-item 生效
    disabled: ref(false),
    reset: function (formItem: any) {
      ;(formItem.disabled as Ref<boolean>).value = false
      formItem.value.value = null
    },
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '公司名称',
        maxlength: recordNameLength,
        showCount: true,
        disabled: (formItem.disabled as Ref<boolean>).value,
      }),
  },
  {
    label: '公司缩写',
    key: 'shortName',
    value: ref(null),
    required: true,
    span: 3,
    disabled: ref(false),
    reset: function (formItem: any) {
      ;(formItem.disabled as Ref<boolean>).value = false
      formItem.value.value = null
    },
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '例如：AB 用于订单拼接',
        maxlength: 4,
        minlength: 2,
        showCount: true,
        disabled: (formItem.disabled as Ref<boolean>).value,
      }),
  },
  {
    label: '公司抬头',
    key: 'companyShowName',
    value: ref(null),
    span: 3,
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '公司抬头名 缩写等 一般用于打印',
        maxlength: recordNameLength,
        showCount: true,
      }),
  },
  {
    label: '备注',
    key: 'remark',
    value: ref(null),
    span: 3, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '公司备注',
        type: 'textarea',
      }),
  },
] as Array<FormItem>
