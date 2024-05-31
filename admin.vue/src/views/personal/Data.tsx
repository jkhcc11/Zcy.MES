import { renderInput, renderSwitch } from '@/hooks/form'
import { FormItem } from '@/types/components'
import { NInput, NSelect } from 'naive-ui'
import { ref, h } from 'vue'

//编辑|新增
export const PersonFormOptions = [
  {
    label: '用户标识',
    key: 'userId',
    value: ref(null),
    render: (formItem: any) => {
      return h(NInput, {
        value: formItem.value.value,
        readonly: true,
        disabled: true,
      })
    },
  },
  {
    label: '到期时间',
    key: 'expirationDateTime',
    value: ref(null),
    render: (formItem: any) => {
      return h(
        NInput,
        {
          value: formItem.value.value,
          readonly: true,
          disabled: true,
          placeholder: '无时间限制',
        },
        {
          suffix: () => '逾期最多7天',
        }
      )
    },
    // renderInput(formItem.value, ),
  },
  {
    label: 'Api密钥',
    key: 'apiToken',
    required: true,
    value: ref(null),
    render: (formItem: any) => {
      return h(
        NInput,
        {
          value: formItem.value.value,
          readonly: true,
          disabled: true,
        },
        {
          suffix: () => 'Json使用',
        }
      )
    },
    // renderInput(formItem.value, ),
  },
  {
    label: '昵称',
    key: 'userNick',
    required: true,
    value: ref(null),
    render: (formItem: any) => {
      return h(
        NInput,
        {
          value: formItem.value.value,
          placeholder: '用户昵称',
          maxlength: 15,
          showCount: true,
          clearable: true,
          onUpdateValue: (val: any) => {
            formItem.value.value = val
          },
        },
        {
          suffix: () => '下次登录生效',
        }
      )
    },
    // renderInput(formItem.value, ),
  },
  {
    label: '是否防盗',
    key: 'isHoldLink',
    value: ref(false),
    render: (formItem: any) => renderSwitch(formItem.value),
  },
  {
    label: '自定义域名',
    key: 'customUrl',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '自由Api域名',
      }),
  },
  {
    label: '防盗域名',
    key: 'holdLinkHost',
    value: ref(null),
    render: (formItem: any) => {
      return h(NSelect, {
        tag: true,
        placeholder: '输入后直接回车后添加下一个',
        show: false,
        showArrow: false,
        multiple: true,
        filterable: true,
        value: formItem.value,
        onChange: function (v: any) {
          formItem.value.value = v
        },
      })
    },
  },
] as Array<FormItem>
