import { FormItem } from '@/types/components'
import { NInput } from 'naive-ui'
import { ref, h } from 'vue'

//修改密码
export const ModifyPwdFormOptions = [
  {
    label: '昵称',
    key: 'userNick',
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
    label: '旧密码',
    key: 'oldPwd',
    required: true,
    value: ref(null),
    render: (formItem: any) => {
      return h(NInput, {
        value: formItem.value.value,
        placeholder: '旧密码',
        maxlength: 20,
        showCount: true,
        clearable: true,
        type: 'password',
        onUpdateValue: (val: any) => {
          formItem.value.value = val
        },
      })
    },
    // renderInput(formItem.value, ),
  },
  {
    label: '新密码',
    key: 'newPwd',
    required: true,
    value: ref(null),
    render: (formItem: any) => {
      return h(
        NInput,
        {
          value: formItem.value.value,
          placeholder: '新密码',
          maxlength: 20,
          showCount: true,
          clearable: true,
          type: 'password',
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
    label: '确认新密码',
    key: 'newPwd1',
    required: true,
    value: ref(null),
    render: (formItem: any) => {
      return h(
        NInput,
        {
          value: formItem.value.value,
          placeholder: '确认新密码',
          maxlength: 20,
          showCount: true,
          clearable: true,
          type: 'password',
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
] as Array<FormItem>
