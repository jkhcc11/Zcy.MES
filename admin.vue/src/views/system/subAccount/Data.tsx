import { FormItem } from '@/types/components'
import { h, ref } from 'vue'
import { renderInput, renderSelect, renderSwitch } from '@/hooks/form'
// import CustomApiSelecter from '@/components/common/CustomApiSelecter.vue'
// import { parseUserApi } from '@/api/url'
import useCookieTypeStore from '@/store/modules/cookie-type'
import { NButton, NInput } from 'naive-ui'

const isTyRef = ref(false)
const tyResult = ref('')
//编辑|新增
export const EditFormOptions = [
  {
    hidden: true,
    key: 'subAccountId',
    value: ref(null),
  },
  {
    label: '类型',
    key: 'subAccountTypeId',
    required: true,
    value: ref(null),
    span: 2,
    render: (formItem: any) => {
      const useCookieType = useCookieTypeStore()
      return renderSelect(
        formItem.value,
        useCookieType.cachedCookieType,
        {
          placeholder: '请选择类型',
          filterable: true,
        },
        function (newVal: any) {
          isTyRef.value = newVal == useCookieType.tyTypeId
        }
      )
    },
  },
  {
    label: '别名',
    key: 'alias',
    required: true,
    value: ref(null),
    span: 2,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '备注和显示用',
      }),
  },
  {
    label: 'Cookie',
    key: 'cookie',
    required: true,
    value: ref(null),
    span: 4,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: 'Cookie',
        type: 'textarea',
      }),
  },
  {
    label: '旧账号信息',
    key: 'oldSubAccountInfo',
    value: ref(null),
    span: 2,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '新增可不填',
      }),
  },
  {
    label: '业务Id',
    key: 'businessId',
    value: ref(null),
    span: 2,
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '适用于跨网盘切换资源 非必要不要填',
      }),
  },
  {
    label: '同步更新服务器Cookie',
    key: 'isSyncServerCookie',
    value: ref(false),
    span: 2,
    render: (formItem) => renderSwitch(formItem.value),
  },
] as Array<FormItem>

//弹窗搜索
export const SearchFormOptions = [
  {
    label: '类型',
    key: 'subAccountTypeId',
    value: ref(null),
    render: (formItem: any) => {
      const useCookieType = useCookieTypeStore()
      return renderSelect(formItem.value, useCookieType.cachedCookieType, {
        placeholder: '请选择类型',
        filterable: true,
        clearable: true,
      })
      // return h(CustomApiSelecter, {
      //   searchApiUrl: parseUserApi.getAllCookieType,
      //   placeholder: '请选择类型',
      //   selectedValues: formItem.value.value,
      //   onValueChage: function (v: any, item: any) {
      //     formItem.value.value = v
      //   },
      // })
    },
  },
  {
    label: '关键字',
    key: 'keyWord',
    value: ref(null),
    render: (formItem) =>
      renderInput(formItem.value, {
        placeholder: '别名或子账号信息',
      }),
  },
] as Array<FormItem>
