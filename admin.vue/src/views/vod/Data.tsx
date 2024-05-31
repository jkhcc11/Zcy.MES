import { renderInput, renderNumberInput, renderSelect, renderSwitch, renderTag } from '@/hooks/form'
import { TableActionModel, useRenderAction } from '@/hooks/table'
import useVodSeriesStore from '@/store/modules/vod-series'
import {
  BooleanEnum,
  DouBanInfoStatusEnum,
  EpisodeGroupTypeEnum,
  FeedBackInfoStatusEnum,
  UserDemandTypeEnum,
  VideoMainStatusEnum,
  VodManagerRecordTypeEnum,
  VodSearchTypeEnum,
  VodSubtypeEnum,
  constSystemInput,
} from '@/store/types'
import { FormItem } from '@/types/components'
import { getListByEnum } from '@/utils'
import { DataTableColumns, NButton, NImage, NInput, NInputNumber, NSelect } from 'naive-ui'
import { h, ref } from 'vue'
import { ConstVideoCountries } from '@/store/types'

//影片编辑列
export const createVodColumns = ({
  down,
  remove,
}: {
  down: (row: any) => void
  remove: (row: any) => void
}): DataTableColumns<any> => {
  return [
    {
      type: 'selection',
      multiple: false,
    },
    {
      title: '影片名',
      key: 'keyWord',
      width: 100,
      ellipsis: {
        tooltip: true,
      },
      render(row: any) {
        return h(
          NButton,
          {
            type: 'info',
            href: `https://www.douban.com/search?cat=1002&q=${row.keyWord}`,
            target: '_blank',
            tag: 'a',
            text: true,
            rel: 'noreferrer',
          },
          {
            default: () => {
              return row.keyWord
            },
          }
        )
      },
    },
    {
      title: '年份',
      key: 'videoYear',
    },
    {
      title: '评分',
      key: 'videoDouBan',
    },
    {
      title: '封面',
      key: 'videoImg',
      render: (rowData: any) => {
        return h(
          NImage,
          {
            width: 25,
            src: rowData.videoImg,
          },
          {
            placeholder: () => {
              return '无封面'
            },
          }
        )
        // return h(NAvatar, {
        //   circle: true,
        //   size: 'small',
        //   src: firstOrDefault(
        //     rowData.weGymImgItems,
        //     (item: any) => item.imgType == ImgItemTypeEnum.封面
        //   )?.imgUrl,
        // })
      },
    },
    {
      title: '人工',
      key: 'isDeal',
      render: (rowData: any) => {
        const isDeal = rowData.videoContentFeature === constSystemInput
        return renderTag(isDeal ? '是' : '否', {
          type: isDeal ? 'success' : 'default',
        })
      },
    },
    {
      title: '创建时间',
      key: 'createdTime',
      width: 80,
      ellipsis: {
        tooltip: true,
      },
    },
    {
      title: '修改时间',
      key: 'modifyTime',
      width: 80,
      ellipsis: {
        tooltip: true,
      },
    },
    {
      title: '操作',
      key: 'actions',
      width: 120,
      render: (rowData) => {
        const normalAction = useRenderAction([
          {
            label: '删除',
            type: 'error',
            onClick: () => remove(rowData),
          },
          {
            label: '下架',
            type: 'warning',
            onClick: () => down(rowData),
          },
        ] as TableActionModel[])

        return normalAction
      },
    },
  ]
}

//剧集编辑列表
export const createEpColumns = ({
  remove,
}: {
  remove: (row: any) => void
}): DataTableColumns<any> => {
  return [
    {
      title: '剧集名',
      key: 'episodeName',
      render(row) {
        return h(NInput, {
          //属性和方法
          value: row.episodeName,
          placeholder: '剧集名',
          onUpdateValue: (val: any) => {
            row.episodeName = val
          },
        })
      },
    },
    {
      title: '播放链接',
      key: 'episodeUrl',
      render(row) {
        return h(NInput, {
          //属性和方法
          value: row.episodeUrl,
          placeholder: '链接',
          onUpdateValue: (val: any) => {
            row.episodeUrl = val
          },
        })
      },
    },
    {
      title: '操作',
      key: 'actions',
      render: (rowData) => {
        const normalAction = useRenderAction([
          {
            label: '移除',
            type: 'error',
            onClick: () => remove(rowData),
          },
        ] as TableActionModel[])

        return normalAction
      },
    },
  ]
}

//管理记录搜索
export const SearchManagerRecordOptions = [
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '关键字 或 用户名',
        clearable: true,
      }),
  },
  {
    label: '记录类型',
    key: 'recordType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(VodManagerRecordTypeEnum), {
        placeholder: '记录类型',
        clearable: true,
      }),
  },
  {
    label: '是否结算',
    key: 'isCheckout',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(BooleanEnum), {
        placeholder: '是否结算',
        clearable: true,
      }),
  },
  {
    label: '是否有效',
    key: 'isValid',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(BooleanEnum), {
        placeholder: '是否有效',
        clearable: true,
      }),
  },
] as Array<FormItem>

//反馈信息管理
export const SearchFeedbackInfoOptions = [
  {
    label: '关键字',
    key: 'key',
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
  {
    label: '反馈类型',
    key: 'userDemandType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(UserDemandTypeEnum), {
        placeholder: '反馈类型',
        clearable: true,
      }),
  },
  {
    label: '反馈状态',
    key: 'feedBackInfoStatus',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(FeedBackInfoStatusEnum), {
        placeholder: '反馈状态',
        clearable: true,
      }),
  },
] as Array<FormItem>

//发送邮件
export const SendEmailFormOptions = [
  {
    label: '邮箱',
    key: 'email',
    value: ref(null),
    required: true,
    span: 2, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '用户邮箱',
        readonly: true,
        disabled: true,
      }),
  },
  {
    label: '主题',
    key: 'subject',
    value: ref(null),
    span: 2, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '邮箱主题',
      }),
  },
  {
    label: '内容',
    key: 'content',
    value: ref(null),
    span: 2, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        type: 'textarea',
        placeholder: '内容',
      }),
  },
] as Array<FormItem>

//影片系列搜索
export const SearchVodSeriesOptions = [
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

//创建影片系列
const seriesImgRef = ref('')
export const CreateVodSeriesFormOptions = [
  {
    key: 'id',
    value: ref(null),
    hidden: true,
  },
  {
    label: '系列名',
    key: 'seriesName',
    value: ref(null),
    required: true,
    span: 2, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '系列名',
      }),
  },
  {
    label: '排序',
    key: 'orderBy',
    value: ref(1),
    required: true,
    reset: function (formItem: any) {
      formItem.value.value = 1
    },
    render: (formItem) => {
      return h(NInputNumber, {
        //属性和方法
        value: formItem.value.value,
        placeholder: '越大越靠前',
        min: 1,
        onUpdateValue: (val: any) => {
          formItem.value.value = val ?? 1
        },
      })
    },
  },
  {
    label: '海报链接',
    key: 'seriesImg',
    value: ref(null),
    required: true,
    span: 2, //grid-item 生效
    render: (formItem: any) => {
      seriesImgRef.value = formItem.value.value
      return h(NInput, {
        //属性和方法
        value: formItem.value.value,
        placeholder: '海报链接',
        onUpdateValue: (val: any) => {
          formItem.value.value = val
          seriesImgRef.value = val
        },
      })
      // return renderInput(formItem.value, {
      //   placeholder: '海报链接',
      // },)
    },
  },
  {
    label: '海报预览',
    key: 'seriesImgPreview',
    value: ref(null),
    span: 2, //grid-item 生效
    render: (formItem: any) => {
      return h(
        NImage,
        {
          width: 25,
          src: seriesImgRef.value,
        },
        {
          placeholder: () => {
            return '无'
          },
        }
      )
    },
  },
  {
    label: '直播地址',
    key: 'liveUrl',
    value: ref(null),
    span: 2, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '直播地址 eg：/TvPlay/SingleVodPlay/8999',
      }),
  },

  {
    label: '介绍视频',
    key: 'seriesDesUrl',
    value: ref(null),
    span: 2, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder:
          '系列介绍视频，如：//player.bilibili.com/player.html?aid=36509588&cid=64109776&page=1',
      }),
  },
  {
    label: '系列备注',
    key: 'seriesRemark',
    value: ref(null),
    span: 3, //grid-item 生效
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '系列备注',
        type: 'textarea',
      }),
  },
] as Array<FormItem>

//影片系列资源搜索
export const SearchVodSeriesListOptions = [
  {
    label: '影片系列',
    key: 'seriesId',
    value: ref(null),
    render: (formItem) => {
      const useVodSeries = useVodSeriesStore()
      return renderSelect(formItem.value, useVodSeries.cachedVodSeries, {
        placeholder: '影片系列',
        filterable: true,
        clearable: true,
      })
    },
    // renderInput(formItem.value, {
    //   placeholder: '关键字',
    //   clearable: true,
    // }),
  },
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
] as Array<FormItem>

//影片影视库搜索
export const SearchVodMainOptions = [
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
  {
    label: '搜索类型',
    key: 'searchType',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(VodSearchTypeEnum), {
        placeholder: '搜索类型',
        clearable: true,
      }),
  },
  {
    label: '影片类型',
    key: 'subtype',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(VodSubtypeEnum), {
        placeholder: '影片类型',
        clearable: true,
      }),
  },
  {
    label: '状态',
    key: 'videoMainStatus',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(VideoMainStatusEnum), {
        placeholder: '状态',
        clearable: true,
      }),
  },
] as Array<FormItem>

//影片信息编辑
const videoImgUrlRef = ref<any>(null)
export const VodMainEditFormOptions = [
  {
    label: '主键',
    key: 'id',
    value: ref(null),
    hidden: true,
  },
  {
    label: '影片名',
    key: 'keyWord',
    value: ref(null),
    required: true,
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '影片名',
      }),
  },
  {
    label: '又名',
    key: 'aka',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '又名',
      }),
  },
  {
    label: '是否完结',
    key: 'isEnd',
    value: ref(null),
    render: (formItem: any) => renderSwitch(formItem.value),
  },
  {
    label: '海报Url',
    key: 'videoImg',
    value: ref(null),
    required: true,
    span: 2,
    render: (formItem: any) => {
      videoImgUrlRef.value = formItem.value.value
      return renderInput(
        formItem.value,
        {
          placeholder: '海报Url',
        },
        function (newVal: any) {
          videoImgUrlRef.value = newVal
        }
      )
    },
  },
  {
    label: '海报预览',
    key: 'videoImgPreview',
    value: ref(null),
    render: (formItem: any) => {
      return h(
        NImage,
        {
          width: 25,
          src: videoImgUrlRef.value,
        },
        {
          placeholder: () => {
            return '无图'
          },
        }
      )
    },
  },
  {
    label: '分类',
    key: 'subtype',
    value: ref(null),
    required: true,
    render: (formItem: any) =>
      renderSelect(formItem.value, getListByEnum(VodSubtypeEnum), {
        placeholder: '请选择分类',
      }),
  },
  {
    label: '状态',
    key: 'videoMainStatus',
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
    render: (formItem: any) =>
      renderSelect(formItem.value, getListByEnum(VideoMainStatusEnum), {
        placeholder: '请选择状态',
      }),
  },
  {
    label: '年份',
    key: 'videoYear',
    value: ref(null),
    required: true,
    render: (formItem: any) =>
      renderNumberInput(formItem.value, {
        placeholder: '年份',
        min: 1000,
        max: 5000,
      }),
  },
  {
    label: '排序',
    key: 'orderBy',
    value: ref(null),
    required: true,
    validator(value, message) {
      if (value) {
        return true
      }

      if (value == 0) {
        return true
      }

      message.error('请输入排序')
    },
    render: (formItem: any) =>
      renderNumberInput(formItem.value, {
        placeholder: '排序 越大越靠前',
        min: 0,
        max: 999,
      }),
  },
  {
    label: '源',
    key: 'sourceUrl',
    value: ref(null),
    required: true,
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '源地址 影片来源',
      }),
  },
  {
    label: '特征码',
    key: 'videoContentFeature',
    value: ref(null),
    required: true,
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '特征码',
      }),
  },
  {
    label: '影片类型',
    key: 'videoGenres',
    value: ref(null),
    required: true,
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
        showArrow: false,
        placeholder: '输入或选择类型',
        options: [],
        value: tempArry,
        onUpdateValue: (newVal: any) => {
          formItem.value.value = newVal.join(',')
        },
      })
    },
  },
  {
    label: '描述',
    key: 'videoSummary',
    value: ref(null),
    required: true,
    span: 2,
    render: (formItem: any) =>
      renderInput(formItem.value, {
        type: 'textarea',
        placeholder: '影片描述',
      }),
  },
  {
    label: '演员',
    key: 'videoCasts',
    value: ref(null),
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
        placeholder: '输入，按回车确认',
        show: false,
        value: tempArry,
        onUpdateValue: (newVal: any) => {
          formItem.value.value = newVal.join(',')
        },
      })
    },
    // render: (formItem: any) =>
    //   renderInput(formItem.value, {
    //     placeholder: '演员 多个,逗号隔开',
    //   }),
  },
  {
    label: '导演',
    key: 'videoDirectors',
    value: ref(null),
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
        placeholder: '输入，按回车确认',
        show: false,
        value: tempArry,
        onUpdateValue: (newVal: any) => {
          formItem.value.value = newVal.join(',')
        },
      })
    },
    // render: (formItem: any) =>
    //   renderInput(formItem.value, {
    //     placeholder: '导演  多个,逗号隔开',
    //   }),
  },
  {
    label: '国家',
    key: 'videoCountries',
    value: ref(null),
    required: true,
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
        placeholder: '输入或选择类型',
        options: ConstVideoCountries.map((it) => {
          return { value: it, label: it }
        }),
        value: tempArry,
        onUpdateValue: (newVal: any) => {
          formItem.value.value = newVal.join(',')
        },
      })
    },
  },
  {
    label: '版权地址',
    key: 'banVideoJumpUrl',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '版权跳转地址',
      }),
  },
  {
    label: '解说地址',
    key: 'narrateUrl',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '解说地址',
      }),
  },
  {
    label: '影片信息Url',
    key: 'videoInfoUrl',
    value: ref(null),
    required: true,
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '豆瓣详情 或 其他详情页',
      }),
  },
  {
    label: '豆瓣评分',
    key: 'videoDouBan',
    value: ref(0),
    validator(value, message) {
      if (value) {
        return true
      }

      if (value == 0) {
        return true
      }

      message.error('请输入豆瓣评分')
    },
    render: (formItem: any) =>
      renderNumberInput(formItem.value, {
        placeholder: '豆瓣评分',
        min: 0,
        max: 10,
        precision: 1,
      }),
  },
  // {
  //   label: '下载地址',
  //   key: 'downUrl',
  //   value: ref(null),
  //   span: 2,
  //   render: (formItem: any) =>
  //     renderInput(formItem.value, {
  //       type: 'textarea',
  //       placeholder: '多个换行或者用#号。格式：百度云$xxxxxxx 百度云1$xxxxxxx',
  //     }),
  // },
] as Array<FormItem>

//豆瓣搜索
export const SearchDouBanOptions = [
  {
    label: '关键字',
    key: 'key',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
  {
    label: '状态',
    key: 'douBanInfoStatus',
    value: ref(null),
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(DouBanInfoStatusEnum), {
        placeholder: '状态',
        clearable: true,
      }),
  },
] as Array<FormItem>

//豆瓣创建影片
export const CreateByDouBanFormOptions = [
  {
    label: '豆瓣Id',
    key: 'douBanInfoId',
    value: ref(null),
    required: true,
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '豆瓣Id',
        readonly: true,
        disabled: true,
      }),
  },
  {
    label: '组类型',
    key: 'episodeGroupType',
    value: ref(1),
    span: 2, //grid-item 生效
    required: true,
    reset: function (formItem: any) {
      formItem.value.value = 1
    },
    render: (formItem) =>
      renderSelect(formItem.value, getListByEnum(EpisodeGroupTypeEnum), {
        placeholder: '组类型',
      }),
  },
  {
    label: '播放链接',
    key: 'playUrls',
    value: ref(null),
    span: 3, //grid-item 生效
    required: true,
    validator(value, message) {
      if (!value) {
        message.error('播放链接必填')
        return false
      } else if (!/^([^\$\n]+\$[^\$\n]+)(\n[^\$\n]+\$[^\$\n]+)*$/.test(value.value.value)) {
        message.error('播放链接格式错误')
        return false
      }

      return true
    },
    render: (formItem: any) =>
      renderInput(formItem.value, {
        type: 'textarea',
        placeholder: '内容 播放链接格式： xxx$xxxxx 换行',
      }),
  },
] as Array<FormItem>

//豆瓣创建信息
export const CreateDouBanFormOptions = [
  {
    label: '豆瓣链接',
    key: 'doubanUrl',
    value: ref(null),
    span: 3, //grid-item 生效
    required: true,
    validator(value, message) {
      if (!value.value.value) {
        message.error('豆瓣链接必填')
        return false
      } else if (value.value.value.indexOf('/subject/') == -1) {
        message.error('豆瓣链接错误')
        return false
      }

      return true
    },
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '链接 https://movie.douban.com/subject/xxxxxxxx/',
      }),
  },
] as Array<FormItem>

//豆瓣关键字搜索
export const SearchKeyWordDouBanOptions = [
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
] as Array<FormItem>

//弹幕搜索
export const SearchDanMuOptions = [
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem: any) =>
      renderInput(formItem.value, {
        placeholder: '关键字',
        clearable: true,
      }),
  },
] as Array<FormItem>
