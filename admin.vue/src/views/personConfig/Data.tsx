import { renderInput, renderNumberInput, renderSelect, renderSwitch } from '@/hooks/form'
import useRoleCacheStore from '@/store/modules/system-role'
import useUserStore from '@/store/modules/user'
import { recordNameLength, ClientTypeEnum, PublicStatusEnum } from '@/store/types'
import { FormItem } from '@/types/components'
import { getListByEnum } from '@/utils'
import { ref } from 'vue'

//供应商客户搜索
export const SearchSupplierClientOptions = [
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

//创建供应商客户
export const CreateSupplierClientFormOptions = [
  {
    key: 'id',
    value: ref(null),
    hidden: true,
  },
  {
    label: '客户名称',
    key: 'clientName',
    value: ref(null),
    required: true,
    span: 2, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '例如：张总',
        maxlength: recordNameLength,
        showCount: true,
      }),
  },
  {
    label: '客户类型',
    key: 'clientType',
    value: ref(null),
    required: true,
    span: 1,
    render: (formItem) => renderSelect(formItem.value, getListByEnum(ClientTypeEnum)),
  },
  {
    label: '客户状态',
    key: 'clientStatus',
    value: ref(null),
    required: true,
    span: 1,
    reset: function (formItem: any) {
      formItem.value.value = PublicStatusEnum.正常
    },
    render: (formItem) => renderSelect(formItem.value, getListByEnum(PublicStatusEnum)),
  },
  {
    label: '联系方式',
    key: 'phoneNumber',
    value: ref(null),
    span: 2,
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '联系方式 例如：xxxxx',
      }),
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

//员工用户搜索
export const SearchUserOptions = [
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

//创建员工用户
export const CreateUserFormOptions = [
  {
    key: 'id',
    value: ref(null),
    hidden: true,
  },
  {
    label: '用户名',
    key: 'userName',
    value: ref(null),
    required: true,
    span: 3, //grid-item 生效
    disabled: ref(false),
    reset(formItem: any) {
      formItem.value.value = null
      formItem.disabled.value = false
    },
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '例如：ZhangSan',
        maxlength: recordNameLength,
        showCount: true,
        disabled: formItem.disabled.value,
      }),
  },
  {
    label: '昵称',
    key: 'userNick',
    value: ref(null),
    required: true,
    span: 2, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '例如：张三',
        maxlength: recordNameLength,
        showCount: true,
      }),
  },
  {
    label: '工号',
    key: 'userNo',
    value: ref(null),
    span: 1, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '工号',
        maxlength: 5,
        showCount: true,
      }),
  },
  {
    label: '联系方式',
    key: 'userPhone',
    value: ref(null),
    span: 2, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '联系方式',
        maxlength: recordNameLength,
        showCount: true,
      }),
  },

  {
    label: '启用登录',
    key: 'isEnableLogin',
    value: ref(false),
    span: 1,
    reset(formItem) {
      formItem.value.value = false
    },
    render: (formItem) => renderSwitch(formItem.value),
  },
  {
    label: '结算基数',
    key: 'baseSettlement',
    value: ref(null),
    span: 2,
    render: (formItem: any) =>
      renderNumberInput(formItem.value, {
        placeholder: '员工的薪资结算基数',
        clearable: true,
        max: 1,
        precision: 2,
      }),
  },
] as Array<FormItem>

//设置用户角色
export const SetRoleFormOptions = [
  {
    key: 'userId',
    value: ref(null),
    hidden: true,
  },
  {
    label: '角色列表',
    key: 'roleIds',
    value: ref(null),
    required: true,
    span: 3, //grid-item 生效
    render: (formItem: any) => {
      const useUser = useUserStore()
      if (useUser.isSuperAdmin) {
        const useRoleCache = useRoleCacheStore()
        return renderSelect(formItem.value, useRoleCache.cachedItems, {
          placeholder: '选择角色',
          multiple: true,
        })
      }
    },
  },
] as Array<FormItem>
