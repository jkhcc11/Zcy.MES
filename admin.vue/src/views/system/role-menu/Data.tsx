import { FormItem } from '@/types/components'
import { h, ref } from 'vue'
import {
  renderInput,
  renderNumberInput,
  renderSelect,
  renderSwitch,
  renderTreeSelect,
} from '@/hooks/form'
import IconSelector from '@/components/common/IconSelector.vue'
import { getListByEnum, transformTreeSelect } from '@/utils'
import useAllMenuTreeStore from '@/store/modules/all-menu-tree'
import { SysRoleNameEnum } from '@/store/types'

//编辑|新增
export const EditFormOptions = [
  {
    hidden: true,
    key: 'id',
    value: ref(null),
  },
  {
    label: '父菜单',
    key: 'parentMenuId',
    value: ref(null),
    span: 2,
    validator: (formItem, message) => {
      if (!formItem.value.value) {
        message.error('请选择上级菜单')
        return false
      }
      return true
    },
    render: (formItem) => {
      const useAllMenuTree = useAllMenuTreeStore()
      return renderTreeSelect(
        formItem.value,
        transformTreeSelect(useAllMenuTree.dataList, 'menuName', 'id'),
        {
          showPath: true,
          clearable: true,
        }
      )
    },
  },
  {
    label: '菜单名称',
    key: 'menuName',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '请输入菜单名称',
      }),
  },
  {
    label: '路由名',
    key: 'routeName',
    required: true,
    value: ref(null),
    span: 2,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: ' 对应vue中的RouteName',
      }),
  },

  {
    label: '菜单图标',
    key: 'icon',
    value: ref(null),
    render: (formItem) => {
      return h(IconSelector, {
        defaultIcon: formItem.value.value,
        onUpdateIcon: (newIcon: any) => {
          formItem.value.value = newIcon.name
        },
      })
    },
  },
  {
    label: '菜单地址',
    key: 'menuUrl',
    required: true,
    value: ref(null),
    span: 2,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '请输入菜单地址',
      }),
  },
  {
    label: '排序',
    key: 'orderBy',
    required: true,
    value: ref(null),
    reset(formItem) {
      formItem.value.value = 1
    },
    render: (formItem) =>
      renderNumberInput(formItem.value, {
        placeholder: '越大越靠前',
        min: 1,
        max: 1000,
      }),
  },
  {
    label: '本地路径映射',
    key: 'localFilePath',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '如果菜单对应不上时 本地文件路径 不包含views',
      }),
  },

  // {
  //   label: '外链地址',
  //   key: 'outLink',
  //   value: ref(null),
  //   render: (formItem) =>
  //     renderInput(formItem.value, {
  //       placeholder: '请输入外链地址',
  //     }),
  // },
  {
    label: '页面缓存',
    key: 'isCache',
    value: ref(false),
    reset(formItem) {
      formItem.value.value = true
    },
    render: (formItem) => renderSwitch(formItem.value),
  },
  {
    label: '是否根路径',
    key: 'isRootPath',
    value: ref(false),
    render: (formItem) => renderSwitch(formItem.value),
  },
] as Array<FormItem>

//搜索
export const SearchFormOptions = [
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    // render: (formItem) =>
    //   renderSelect(formItem.value, getListByEnum(SysRoleNameEnum), {
    //     clearable: true,
    //     placeholder: '关键字',
    //     filterable: true,
    //     multiple: false,
    //     tag: true,
    //   }),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
] as Array<FormItem>

//角色 编辑|新增
export const EditRoleFormOptions = [
  {
    hidden: true,
    key: 'id',
    value: ref(null),
  },
  {
    label: '显示名',
    key: 'roleShowName',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '请输入角色名',
      }),
  },
  {
    label: '角色标识',
    key: 'roleName',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(SysRoleNameEnum), {
        clearable: true,
        placeholder: '角色名',
        filterable: true,
        multiple: false,
        tag: true,
      }),
    // render: (formItem) =>
    //   renderInput(formItem.value, {
    //     placeholder: ' 对应Ids角色名',
    //   }),
  },
  {
    label: '是否默认',
    key: 'isDefault',
    required: true,
    value: ref(false),
    reset(formItem) {
      formItem.value.value = false
    },
    render: (formItem) => renderSwitch(formItem.value),
  },
  {
    label: '角色备注',
    key: 'remark',
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: ' 角色备注',
        type: 'textarea',
      }),
  },
] as Array<FormItem>
