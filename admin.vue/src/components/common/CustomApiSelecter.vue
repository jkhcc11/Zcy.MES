<template>
    <n-select
      v-model:value="selectedValues"
      :multiple="isMultiple"
      :filterable="isFilterable"
      :placeholder="placeholder"
      :options="options"
      :loading="loading"
      :clearable="isClearable"
      remote
      :clear-filter-after-select="false"
      @search="handleSearch"
      :on-update:value="valueChange"
    />
  </template>
  
  <script lang="ts">
    import { defineComponent, ref, h, onMounted, watch } from 'vue'
    import { SelectOption } from 'naive-ui'
    import { get } from '@/api/http'
    export default defineComponent({
      name: 'CustomApiSelecter',
      props: {
        onValueChage: Function,
        onRequestSuccessFormat: Function,
        placeholder: {
          type: String,
          default: '请输入搜索关键字',
        },
        /**
         * 搜索Api地址
         */
        searchApiUrl: {
          type: String,
          default: null,
        },
        selectedValues: {
          type: Object,
          default: null,
        },
        /**
         * 是否实时搜索 默认不是(会一次性拉取服务器数据，然后本地搜索)
         */
        isRealTimeSearch: {
          type: Boolean,
          default: false,
        },
        /**
         * 是否多选
         */
        isMultiple: {
          type: Boolean,
          default: false,
        },
        /**
         * 是否可过滤
         */
        isFilterable: {
          type: Boolean,
          default: false,
        },
        /**
         * 是否可清空
         */
        isClearable: {
          type: Boolean,
          default: false,
        },
      },
      // emits: ['selected'],
      setup(props) {
        const { onValueChage, onRequestSuccessFormat } = props
        const isRealTimeSearch = props.isRealTimeSearch
        const loading = ref(false)
        const options = ref<SelectOption[]>([])
        const remoteData = ref<SelectOption[]>([])
        //const localSelectedValues = ref<any>(null)
  
        //刷新
        function handleSearch(query: string) {
          if (!query.length) {
            options.value = []
            return
          }
  
          loading.value = true
          if (isRealTimeSearch) {
            queryList(query)
          } else {
            //~相当于 item.label.indexOf(query) !== -1
            options.value = remoteData.value.filter(
              (item: SelectOption) => ~item.label.indexOf(query)
            )
          }
  
          loading.value = false
        }
  
        function valueChange(v: any, item: any) {
          //console.log(v, item)
          if (onValueChage) {
            onValueChage(v, item)
          }
        }
  
        function queryList(query: string) {
          if (!props.searchApiUrl) {
            throw new Error('请检查组件searchApiUrl是否设置')
          }
  
          //获取服务器
          get({
            url: props.searchApiUrl,
            data: () => {
              return {
                keyWord: query,
                page: 1,
                maxResultCount: 100,
              }
            },
          })
            .then((res) => {
              //请求成功处理
              if (onRequestSuccessFormat) {
                remoteData.value = onRequestSuccessFormat(res.data)
                options.value = remoteData.value
              } else {
                remoteData.value = res.data.map((item: any) => {
                  return {
                    label: item.showText,
                    value: item.id,
                  } as SelectOption
                })
                options.value = remoteData.value
              }
  
              loading.value = false
            })
            .catch(console.log)
        }
  
        onMounted(async () => {
          if (isRealTimeSearch == false && remoteData.value.length == 0) {
            //组件加载是 拉取所有
            queryList('')
          }
  
          // if (props.selectedValues) {
          //   localSelectedValues.value = props.selectedValues
          // }
        })
  
        //独立开子组件和父组件的列表关系 非第一次加载时监听到变化后赋值给当前子组件
        // watch(
        //   () => props.selectedValues,
        //   (newValue) => {
        //     localSelectedValues.value = newValue
        //   }
        // )
  
        return {
          //localSelectedValues,
          loading,
          options,
          valueChange,
          handleSearch,
        }
      },
    })
  </script>
  