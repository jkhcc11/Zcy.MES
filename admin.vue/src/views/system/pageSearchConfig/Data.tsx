import { renderInput, renderSelect } from '@/hooks/form'
import { ConfigHttpMethodEnum, SearchConfigStatusEnum, ServiceFullNameEnum } from '@/store/types'
import { FormItem } from '@/types/components'
import { getListByEnum } from '@/utils'
import { NImage, NSelect } from 'naive-ui'
import { h, ref } from 'vue'

//编辑|新增
export const CreatePageSearchConfigFormOptions = [
  {
    hidden: true,
    key: 'configId',
    value: ref(null),
  },
  {
    label: 'BaseHost',
    key: 'baseHost',
    required: true,
    value: ref(null),
    span: 2,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: 'BaseHost eg:https://www.baidu.com',
      }),
  },
  {
    label: 'Http类型',
    key: 'configHttpMethod',
    value: ref(null),
    required: true,
    validator(value, message) {
      if (value) {
        return true
      }

      if (value == 0) {
        return true
      }

      message.error('请选择状态')
    },
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(ConfigHttpMethodEnum), {
        placeholder: 'Http类型',
        clearable: true,
      }),
  },

  {
    label: '站点名',
    key: 'hostName',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '站点名 前端展示用',
      }),
  },
  {
    label: '实例方法',
    key: 'serviceFullName',
    value: ref(null),
    required: true,
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(ServiceFullNameEnum), {
        placeholder: '实例方法',
        clearable: true,
      }),
  },
  {
    label: '状态',
    key: 'searchConfigStatus',
    required: true,
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(SearchConfigStatusEnum), {
        placeholder: '状态',
        clearable: true,
      }),
  },

  {
    label: 'UserAgent',
    key: 'userAgent',
    span: 3,
    value: ref(null),
    required: true,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: 'userAgent',
      }),
  },

  {
    label: '搜索路径',
    key: 'searchPath',
    value: ref(null),
    required: true,
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '搜索路径',
      }),
  },
  {
    label: 'Post数据',
    key: 'searchData',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: 'Post数据',
      }),
  },
  {
    label: '搜索Xpath',
    key: 'searchXpath',
    value: ref(null),
    required: true,
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '搜索页结果 匹配Xpath',
      }),
  },
  {
    label: '结束Xpath',
    key: 'detailEndXpath',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '搜索结果页 结束标记匹配Xpath',
      }),
  },

  {
    label: '备用Host',
    key: 'otherHost',
    value: ref(null),
    span: 3,
    render: (formItem: any) => {
      const tempArry =
        formItem.value.value
          ?.split(/[,，]/)
          .filter((it) => it && it.length > 0)
          .map((item: any) => item.trim()) ?? []
      return h(NSelect, {
        filterable: true,
        multiple: true,
        tag: true,
        placeholder: '其他BaseHost，按回车确认',
        show: false,
        value: tempArry,
        onUpdateValue: (newVal: any) => {
          formItem.value.value = newVal.join(',')
        },
      })
    },
    // render: (formItem) =>
    //   renderInput(formItem.value, {
    //     placeholder: '其他BaseHost 对应的也是这个站点',
    //     type: 'textarea',
    //   }),
  },

  {
    label: '图片属性Attr',
    key: 'imgAttr',
    value: ref(null),
    span: 3,
    render: (formItem: any) => {
      return h(NSelect, {
        filterable: true,
        multiple: true,
        tag: true,
        placeholder: '图片所在A标签属性name 回车',
        show: false,
        value: formItem.value.value,
        onUpdateValue: (newVal: any) => {
          formItem.value.value = newVal
        },
      })
    },
    // render: (formItem) =>
    //   renderInput(formItem.value, {
    //     placeholder: '图片所在A标签属性name 多个逗号隔开',
    //   }),
  },
  {
    label: '名字属性Attr',
    key: 'nameAttr',
    value: ref(null),
    span: 3,
    render: (formItem: any) => {
      return h(NSelect, {
        filterable: true,
        multiple: true,
        tag: true,
        placeholder: '名称所在A标签属性name 回车',
        show: false,
        value: formItem.value.value,
        onUpdateValue: (newVal: any) => {
          formItem.value.value = newVal
        },
      })
    },
    // render: (formItem) =>
    //   renderInput(formItem.value, {
    //     placeholder: '名称所在A标签属性name 多个逗号隔开',
    //   }),
  },

  {
    label: '详情播放Xpath',
    key: 'detailXpath',
    value: ref(null),
    span: 3,
    required: true,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder:
          '详情页播放地址 匹配Xpath | 资源站Json这里是拼接详情 /api.php/provide/vod/?ac=detail&ids= | 磁力站随意 ',
      }),
  },
  {
    label: '详情图片Xpath',
    key: 'detailImgXpath',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '详情页图片 匹配Xpath',
      }),
  },

  {
    label: '详情名称Xpath',
    key: 'detailNameXpath',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '详情名称匹配Xpath',
      }),
  },

  {
    label: '详情完结Xpath',
    key: 'detailEndXpath',
    value: ref(null),
    span: 2,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '详情页 完结标识匹配Xpath',
      }),
  },
  {
    label: '完结标记',
    key: 'notEndKey',
    value: ref(null),
    span: 1,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '完结标记 仅用于搜索结果页面',
      }),
  },

  {
    label: '年份Xpath',
    key: 'yearXpath',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '详情页 年份Xpath',
      }),
  },
  {
    label: '播放地址后缀',
    key: 'playUrlSuffix',
    value: ref(null),
    span: 3,
    render: (formItem: any) => {
      return h(NSelect, {
        filterable: true,
        multiple: true,
        tag: true,
        placeholder: '适用于资源站(从前往后匹配) 哪些才是有效播放地址后缀',
        show: false,
        value: formItem.value.value,
        onUpdateValue: (newVal: any) => {
          formItem.value.value = newVal
        },
      })
    },
  },
  {
    label: '采集地址',
    key: 'captureDetailUrl',
    value: ref(null),
    span: 3,
    render: (formItem: any) => {
      return h(NSelect, {
        filterable: true,
        multiple: true,
        tag: true,
        placeholder: '待采集地址 回车',
        show: false,
        value: formItem.value.value,
        onUpdateValue: (newVal: any) => {
          formItem.value.value = newVal
        },
      })
    },
    // render: (formItem) =>
    //   renderInput(formItem.value, {
    //     placeholder: '待采集地址 多个用,逗号隔开',
    //   }),
  },
  {
    label: '采集地址Xpath',
    key: 'captureDetailXpath',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '采集详情页匹配Xpath',
      }),
  },
  {
    label: '采集详情名称后缀',
    key: 'captureDetailNameSplit',
    value: ref(null),
    span: 3,
    render: (formItem: any) => {
      return h(NSelect, {
        filterable: true,
        multiple: true,
        tag: true,
        placeholder: '分割关键字 eg: xxx更新 => 更新 回车',
        show: false,
        value: formItem.value.value,
        onUpdateValue: (newVal: any) => {
          formItem.value.value = newVal
        },
      })
    },
    // render: (formItem) =>
    //   renderInput(formItem.value, {
    //     placeholder: '待采集地址 多个用,逗号隔开',
    //   }),
  },
  {
    label: '备注',
    key: 'hostRemark',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '备注',
        type: 'textarea',
      }),
  },
] as Array<FormItem>

//搜索页面配置
export const SearchPageSearchConfigOptions = [
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(ServiceFullNameEnum), {
        clearable: true,
        placeholder: '关键字',
        filterable: true,
        multiple: false,
        tag: true,
      }),
    // render: (formItem) =>
    //   renderInput(formItem.value, {
    //     placeholder: '关键字',
    //     clearable: true,
    //   }),
  },
] as Array<FormItem>

//搜索测试
export const TestSearchPageSearchConfigOptions = [
  {
    label: '配置ID',
    key: 'configId',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '配置ID',
        clearable: true,
        readonly: true,
        disabled: true,
      }),
  },
  {
    label: '详情页地址',
    key: 'detail',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '详情页地址',
        clearable: true,
      }),
  },
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
  {
    label: '搜索结果',
    key: 'result',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '搜索结果',
        clearable: true,
        type: 'textarea',
        disabled: true,
      }),
  },
  {
    label: '结果录入',
    key: 'result1',
    value: ref(null),
    span: 3,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '结果录入',
        clearable: true,
        type: 'textarea',
        disabled: true,
      }),
  },
  {
    label: '结果封面',
    key: 'resultImg',
    value: ref(null),
    span: 2,
    render: (formItem) => {
      if (formItem.value && formItem.value.value) {
        return h(
          NImage,
          {
            width: 45,
            src: `https://gimg3.baidu.com/gimg/app=2028&src=${formItem.value.value
              .replace('https://', '')
              .replace('http://', '')}`,
            referrerpolicy: 'no-referrer',
          },
          {
            placeholder: () => {
              return '无封面'
            },
          }
        )
      }

      return '暂无'
    },
  },
  {
    label: '结果名称',
    key: 'resultName',
    value: ref(null),
    span: 1,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '结果名称',
        clearable: true,
        disabled: true,
      }),
  },
] as Array<FormItem>
