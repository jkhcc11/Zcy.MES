import { renderInput, renderSelect } from '@/hooks/form'
import { FormItem } from '@/types/components'
import { getListByEnum } from '@/utils'
import { NInputNumber } from 'naive-ui'
import { ref, h } from 'vue'
import { CmsTypeEnum } from './types'

//操作区item
export interface CloudListOperateItem {
  /**
   * 是否原始名称
   */
  isOriginalName: boolean
  /**
   * 是否移除host前缀
   */
  isRemoveHost: boolean
  /**
   * 是否强制2位 占位
   */
  isFource2Placeholder: boolean
  /**
   * 外链类型
   */
  linkType: 'id' | 'name'
  /**
   * 复制类型
   */
  copyType: 'player' | 'json' | 'jump'
}

//重命名
export const ReNameFormOptions = [
  {
    label: '文件名前缀',
    key: 'namePrefix',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '文件名前缀',
      }),
  },
  {
    label: '起始值',
    key: 'startIndex',
    required: true,
    value: ref(1),
    reset: function (formItem: any) {
      formItem.value.value = 1
    },
    render: (formItem) => {
      return h(NInputNumber, {
        //属性和方法
        value: formItem.value.value,
        placeholder: '起始值',
        min: 0,
        onUpdateValue: (val: any) => {
          formItem.value.value = val ?? 1
        },
      })
    },
  },
] as Array<FormItem>

//查找替换
export const ReplaceFormOptions = [
  {
    label: '查找关键字',
    key: 'findStr',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '查找关键字',
      }),
  },
  {
    label: '替换为',
    key: 'newName',
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '替换为',
      }),
  },
] as Array<FormItem>

//入库
export const SendVodFormOptions = [
  {
    label: 'CMS类型',
    key: 'sendType',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(CmsTypeEnum), {
        placeholder: '类型',
      }),
  },
  {
    label: '入库Url',
    key: 'apiUrl',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: 'CMS入库地址',
      }),
  },
  {
    label: '入库密钥',
    key: 'sendPassWord',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '入库密钥',
      }),
  },
  {
    label: '类型名',
    key: 'vodTypeName',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '类型名称 类型和影片名确认唯一',
      }),
  },
  {
    label: '影片名',
    key: 'vodName',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '影片名 如果一样 就更新播放链接',
      }),
  },
  {
    label: '播放器名',
    key: 'playFrom',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '播放器名 不能写错',
      }),
  },
  {
    label: '播放链接',
    key: 'playUrl',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '播放链接',
        type: 'textarea',
      }),
  },
] as Array<FormItem>
