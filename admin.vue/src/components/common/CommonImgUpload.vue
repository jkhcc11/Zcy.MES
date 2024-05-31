<template>
  <n-upload
    :data="extData"
    :custom-request="customRequest"
    :default-file-list="defaultImgList"
    :on-remove="onRemove"
    list-type="image-card"
    :max="maxFile"
    accept="image/*"
  >
    {{ tipMsg }}
  </n-upload>
</template>

<script lang="ts">
  import { defineComponent, toRef } from 'vue'
  import { UploadCustomRequestOptions, useMessage } from 'naive-ui'
  import { post } from '@/api/http'
  export default defineComponent({
    name: 'CommonImgUpload',
    props: {
      onUploadFinish: Function,
      /**
       * 移除回调
       */
      onRemove: Function,
      uploadApi: {
        type: String,
        default: null,
      },
      /**
       * 扩展数据 除了文件字段以外的数据
       */
      extData: {
        type: Object,
        default: null,
      },
      tipMsg: {
        type: String,
        default: '请选择图片',
      },
      maxFile: {
        type: Number,
        default: 1,
      },
      defaultImgList: {
        type: Array<Object>,
        default: [],
      },
      fieldName: {
        type: String,
        default: 'file',
      },
    },
    setup(props) {
      //把传入的变量解析出来
      const { onUploadFinish } = props
      const fieldName = toRef(props.fieldName)
      const apiUrl = toRef(props.uploadApi)
      if (apiUrl.value == null || apiUrl.value.length <= 0) {
        throw new Error('请检查组件uploadApi是否设置')
      }

      //列表情况
      //const fileList = ref<UploadFileInfo[]>([
      // {
      //   id: 'a',
      //   name: '我是上传出错的普通文件.png',
      //   status: 'error',
      // },
      // {
      //   id: 'b',
      //   name: '我是普通文本.doc',
      //   status: 'finished',
      //   type: 'text/plain',
      // },
      // {
      //   id: 'c',
      //   name: '我是自带url的图片.png',
      //   status: 'finished',
      //   url: 'https://07akioni.oss-cn-beijing.aliyuncs.com/07akioni.jpeg',
      // },
      // {
      //   id: 'd',
      //   name: '我是上传进度99%的文本.doc',
      //   status: 'uploading',
      //   percentage: 99,
      // },
      //])

      const message = useMessage()
      //自定义上传
      const customRequest = ({ file, data }: UploadCustomRequestOptions) => {
        const formData = new FormData()
        if (data) {
          Object.keys(data).forEach((key) => {
            formData.append(key, data[key as keyof UploadCustomRequestOptions['data']])
          })
        }

        formData.append(fieldName.value, file.file as File)
        post({
          url: apiUrl.value,
          data: formData,
        })
          .then((res) => {
            //请求成功处理
            if (res.success) {
              //console.log(res)
              //这里可以自定义赋值
              file.url = res.result
              file.thumbnailUrl = res.result
              if (onUploadFinish) {
                onUploadFinish(res)
              }
              return
            }

            message.error(res.errorMsg)
          })
          .catch((res) => {
            console.error('CommonImgUpload error', res)
          })
      }

      // onMounted(async () => {
      //   table.tableHeight.value = await useTableHeight()
      //   doRefresh()
      // })
      return {
        customRequest,
      }
    },
  })
</script>
