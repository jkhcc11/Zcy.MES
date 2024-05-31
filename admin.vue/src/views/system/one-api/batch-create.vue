<template>
  <div class="main-container">
    <n-card>
      <n-form
        ref="stepOneForm"
        :model="formDataInfo"
        :rules="formDataInfoRule"
        label-width="120"
        label-placement="left"
      >
        <n-form-item label="系统密钥" path="apiToken">
          <n-input v-model:value="formDataInfo.apiToken" placeholder="系统密钥" />
        </n-form-item>
        <n-form-item label="数量" path="tokenCount">
          <n-input-number
            v-model:value="formDataInfo.tokenCount"
            placeholder="创建数量"
            :min="1"
            :max="20"
            clearable
          />
        </n-form-item>
        <n-form-item label="令牌名前缀" path="tokenNamePrefix">
          <n-input v-model:value="formDataInfo.tokenNamePrefix" placeholder="令牌名前缀" />
        </n-form-item>
        <n-form-item label="额度" path="remain_quota">
          <n-input-number
            v-model:value="formDataInfo.remain_quota"
            placeholder="额度"
            clearable
            :min="1"
            :max="50"
          />
        </n-form-item>
        <n-form-item label="过期时间" path="expiredTime">
          <n-date-picker
            v-model:formatted-value="formDataInfo.expiredTime"
            value-format="yyyy-MM-dd"
            :is-date-disabled="dateDisabled"
            type="date"
            placeholder="过期时间"
          />
        </n-form-item>

        <n-form-item label="结果" path="result">
          <n-input
            v-model:value="formDataInfo.result"
            placeholder="结果 提交成功后自动复制到剪切板"
            :readonly="true"
            type="textarea"
          />
        </n-form-item>

        <n-form-item class="flex justify-end mt-2 mb-2">
          <n-space>
            <n-button type="primary" size="small" @click="onSubmit" :loading="nextLoading"
              >提交</n-button
            >
          </n-space>
        </n-form-item>
      </n-form>
    </n-card>
  </div>
</template>

<script lang="ts">
  import Clipboard from 'clipboard'
  import { post } from '@/api/http'
  import { systemApi } from '@/api/url'
  import { NForm, useMessage } from 'naive-ui'
  import { defineComponent, reactive, ref } from 'vue'
  import useOneApiStore from '@/store/modules/one-api'
  export default defineComponent({
    name: 'OneApiBatchCreate',
    setup() {
      const oneApiStore = useOneApiStore()
      const message = useMessage()
      const nextLoading = ref(false)
      const stepOneForm = ref<typeof NForm | null>(null)
      const formDataInfo = reactive({
        apiToken: oneApiStore.apiToken,
        tokenCount: null,
        tokenNamePrefix: null,
        remain_quota: null,
        expiredTime: null,
        result: '',
      })

      const formDataInfoRule = {
        apiToken: [{ required: true, message: '密钥必填', trigger: 'blur' }],
        expiredTime: [{ required: true, message: '时间必填', trigger: 'blur' }],
        tokenNamePrefix: [{ required: true, message: '令牌名前缀必填', trigger: 'blur' }],
        tokenCount: [
          {
            required: true,
            message: '数量必填',
            trigger: 'blur',
            type: 'number',
          },
        ],
        remain_quota: [
          {
            required: true,
            message: '额度必填',
            trigger: 'blur',
            type: 'number',
          },
        ],
      }

      function onSubmit() {
        stepOneForm.value?.validate(async (error: any) => {
          if (!error) {
            nextLoading.value = true

            post({
              url: systemApi.batchCreateToken,
              data: formDataInfo,
            })
              .then((res) => {
                oneApiStore.save(formDataInfo.apiToken)
                formDataInfo.result = res.data
                  .map((item) => {
                    return item.token
                  })
                  .join('\r\n')

                formDataInfo.tokenNamePrefix = null
                formDataInfo.remain_quota = null
                if (Clipboard.isSupported()) {
                  Clipboard.copy(formDataInfo.result)
                  message.success('复制成功，请查看剪切板')
                  return
                }
              })
              .finally(() => {
                nextLoading.value = false
              })
          }
        })
      }

      function dateDisabled(ts: number) {
        return ts < Date.now()
      }

      //   onMounted(async () => {
      //     await useVodSeries.init()
      //   })

      return {
        nextLoading,
        stepOneForm,
        formDataInfo,
        formDataInfoRule,
        dateDisabled,
        onSubmit,
      }
    },
  })
</script>
