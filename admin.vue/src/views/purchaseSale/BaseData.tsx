import { renderInput, renderSelect } from '@/hooks/form'
import useCompanyCacheStore from '@/store/modules/company'
import { baseRemarkLength } from '@/store/types'
import { FormItem } from '@/types/components'
import { NButton, NDatePicker, NInput, NInputNumber } from 'naive-ui'
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

//销售产品编辑表格头
export const SeleProductEditTableColumns = (onRemoveItem: (rowData: any) => void) => [
  {
    title: '产品名',
    key: 'productName',
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
  {
    title: '数量',
    key: 'count',
    width: 200,
    render(rowData: any) {
      return h(
        NInputNumber,
        {
          value: rowData.count,
          min: 1,
          max: 9999,
          precision: 0,
          placeholder: '数量',
          onUpdateValue(newVal: any) {
            rowData.count = newVal
          },
        },
        {
          suffix: () => {
            if (rowData.isLoose) {
              //散件
              return rowData.unit
            }

            return `${rowData.spec}`
          },
        }
      )
    },
  },
  {
    title: '单价（元）',
    key: 'unitPrice',
    width: 200,
    render(rowData: any) {
      return h(
        NInputNumber,
        {
          value: rowData.unitPrice,
          min: 0.01,
          max: 9999,
          precision: 2,
          placeholder: '单价',
          onUpdateValue(newVal: any) {
            rowData.unitPrice = newVal
          },
        },
        {
          prefix: () => '￥',
          suffix: () => {
            return `/${rowData.unit}`
          },
        }
      )
    },
  },
  {
    title: '总数',
    key: 'sumCount',
    render: (rowData: any) => {
      if (rowData.isLoose) {
        //散件
        return `${rowData.count}${rowData.unit}`
      }

      return `${rowData.count * rowData.specCount}${rowData.unit}`
    },
  },
  {
    title: '总价（单价*数量）',
    key: 'sumPrice',
    render: (rowData: any) => {
      if (rowData.isLoose) {
        //散件
        rowData.sumPrice = (rowData.unitPrice * rowData.count).toFixed(2)
      } else {
        rowData.sumPrice = (rowData.unitPrice * rowData.specCount * rowData.count).toFixed(2)
      }

      return rowData.sumPrice
    },
  },
  {
    title: '产品备注',
    key: 'remark',
    render(row: any) {
      return h(NInput, {
        value: row.remark,
        placeholder: '产品备注',
        onUpdateValue(newVal: any) {
          row.remark = newVal
        },
      })
    },
  },
  {
    title: '操作',
    key: 'actions',
    render(rowData: any) {
      return h(
        NButton,
        {
          type: 'error',
          size: 'small',
          onClick: () => onRemoveItem(rowData),
        },
        { default: () => '移除' }
      )
    },
  },
]

//订单详情表头
export const OrderDetailTableColumns = [
  {
    title: '产品名',
    key: 'productName',
  },
  {
    title: '数量',
    key: 'count',
    render: (rowData: any) => {
      if (rowData.isLoose) {
        //散件
        return `${rowData.count}${rowData.unit}`
      }

      return `${rowData.count}${rowData.spec}`
    },
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
  {
    title: '单价（元）',
    key: 'unitPrice',
  },
  {
    title: '总价（单价*数量）',
    key: 'sumPrice',
  },
  {
    title: '产品备注',
    key: 'remark',
  },
]
