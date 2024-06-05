import { moneyMax } from '@/store/types'
import {
  CheckboxGroupProps,
  CheckboxProps,
  DatePickerProps,
  InputNumberProps,
  InputProps,
  NButton,
  NCheckbox,
  NCheckboxGroup,
  NDatePicker,
  NInput,
  NInputNumber,
  NPopselect,
  NRadioButton,
  NRadioGroup,
  NSelect,
  NSpace,
  NSwitch,
  NTag,
  NTimePicker,
  NTreeSelect,
  PopselectProps,
  RadioButtonProps,
  RadioGroupProps,
  SelectOption,
  SelectProps,
  SwitchProps,
  TagProps,
  TimePickerProps,
  TreeSelectProps,
} from 'naive-ui'
import { Value as DatePickerValue } from 'naive-ui/lib/date-picker/src/interface'
import { SelectGroupOption, Value as SelectValue } from 'naive-ui/lib/select/src/interface'
import { TreeSelectOption, Value } from 'naive-ui/lib/tree-select/src/interface'
import { AllowedComponentProps, createVNode, h, Ref } from 'vue'

export function renderInput(
  value: Ref<string>,
  options: InputProps | AllowedComponentProps = {},
  onUpdate?: Function | null
) {
  return h(NInput, {
    value: value.value,
    onUpdateValue: (newVal: string) => {
      value.value = newVal
      if (onUpdate) {
        onUpdate(newVal)
      }
    },
    ...options,
  })
}

export function renderRadioButtonGroup(
  value: Ref<string | number | null | undefined>,
  options: (RadioButtonProps & { label: string })[],
  optionProps: RadioGroupProps | AllowedComponentProps = {}
) {
  return createVNode(
    NRadioGroup,
    {
      value: value.value,
      ...optionProps,
      onUpdateValue: (newVal: string | number | null | undefined) => {
        value.value = newVal
      },
    },
    {
      default: () => {
        return options.map((it) => {
          return createVNode(
            NRadioButton,
            {
              ...it,
            },
            {
              default: () => it.label,
            }
          )
        })
      },
    }
  )
}

export function renderCheckbox(
  value: Ref<boolean>,
  label: string,
  options: CheckboxProps | AllowedComponentProps = {}
) {
  return h(
    NCheckbox,
    {
      checked: value.value,
      onUpdateChecked: (newVal: boolean) => {
        value.value = newVal
      },
      ...options,
    },
    {
      default: () => label,
    }
  )
}

export function renderTag(label: string, options: TagProps | AllowedComponentProps = {}) {
  return h(NTag, options, {
    default: () => label,
  })
}

export function renderCheckboxGroup(
  value: Ref<(string | number)[]>,
  options: CheckboxProps[],
  optionProps: CheckboxGroupProps | AllowedComponentProps = {}
) {
  return h(
    NCheckboxGroup,
    {
      value: value.value,
      onUpdateValue: (newVal) => {
        value.value = newVal
      },
      ...optionProps,
    },
    {
      default: () => {
        return h(
          NSpace,
          {
            itemStyle: 'diaplay: flex',
          },
          {
            default: () => {
              return options.map((it) => {
                return h(NCheckbox, {
                  value: it.value,
                  label: it.label,
                })
              })
            },
          }
        )
      },
    }
  )
}

export function renderSelect(
  value: Ref<SelectValue>,
  options: SelectOption[],
  optionProps: SelectProps | AllowedComponentProps = {},
  onUpdate?: Function | null
) {
  return h(NSelect, {
    options,
    value: value.value,
    ...optionProps,
    onUpdateValue: (newVal: any) => {
      value.value = newVal
      if (onUpdate) {
        onUpdate(newVal)
      }
    },
  })
}

export function renderTreeSelect(
  value: Ref<Value>,
  options: TreeSelectOption[],
  optionProps: TreeSelectProps | AllowedComponentProps = {}
) {
  return h(NTreeSelect, {
    value: value.value,
    options,
    onUpdateValue: (newVal) => {
      value.value = newVal
    },
    ...optionProps,
  })
}

export function renderSwitch(
  value: Ref<boolean>,
  options: SwitchProps | AllowedComponentProps = {}
) {
  return h(NSwitch, {
    value: value.value,
    onUpdateValue: (newVal: boolean) => {
      value.value = newVal
    },
    ...options,
  })
}

export function renderDatePicker(
  value: Ref<DatePickerValue>,
  options: DatePickerProps | AllowedComponentProps = {}
) {
  return h(NDatePicker, {
    value: value.value,
    onUpdateValue: (newVal: any) => {
      value.value = newVal
    },
    ...options,
  })
}

export function renderTimePicker(value: Ref<number | null>, options: TimePickerProps = {}) {
  return h(NTimePicker, {
    value: value.value,
    onUpdateValue: (newVal: number | null) => {
      value.value = newVal
    },
    ...options,
  })
}

export function renderPopSelect(
  value: Ref<string | number | Array<string | number> | null>,
  options: Array<SelectOption | SelectGroupOption>,
  optionProps: PopselectProps | AllowedComponentProps = {}
) {
  return createVNode(
    NPopselect,
    {
      value: value.value,
      onUpdateValue: (newVal: string | number | Array<string | number> | null) => {
        value.value = newVal
      },
      options,
      ...optionProps,
    },
    {
      default: () => {
        return createVNode(NButton, null, {
          default: () => value.value || '请选择',
        })
      },
    }
  )
}

/**
 * 根据枚举渲染tag
 * @param value 枚举值
 * @param enumObj 枚举
 * @param tagType 枚举值 对应的 tag类型 eg: {1:'info',2:'error'}
 * @param options tag 其他参数
 * @returns
 */
export function renderTagByEnum<T>(
  value: any,
  enumObj: T,
  tagType: any,
  options: TagProps | AllowedComponentProps = {}
) {
  return h(
    NTag,
    {
      type: tagType[value] || 'default',
      ...options,
    },
    {
      default: () => enumObj[value as keyof T],
    }
  )
}

/**
 * 渲染数字Input
 * @param value 值
 * @param options
 * @returns
 */
export function renderNumberInput(
  value: Ref<number>,
  options: InputNumberProps | AllowedComponentProps = {}
) {
  return h(NInputNumber, {
    value: value.value,
    onUpdateValue: (newVal: number) => {
      value.value = newVal
    },
    ...options,
  })
}

/**
 * 渲染金额
 * @param value
 * @param options
 * @returns
 */
export function renderMoneyInput(
  value: Ref<number>,
  options: InputNumberProps | AllowedComponentProps = {}
) {
  return h(NInputNumber, {
    value: value.value,
    min: 0.01,
    max: moneyMax,
    precision: 2,
    onUpdateValue: (newVal: number) => {
      value.value = newVal
    },
    ...options,
  })
}
