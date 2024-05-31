<template>
  <div class="main-container">
    <n-grid :cols="3">
      <n-grid-item :offset="1">
        <n-card title="提交申请">
          <n-form>
            <n-form-item-row label="当前用户Id">
              <n-input :value="userIdRef" :disabled="true" />
            </n-form-item-row>
          </n-form>
          <n-button type="primary" block secondary strong @click="onSubmit" :loading="submitLoaing">
            提交申请
          </n-button>
        </n-card>
      </n-grid-item>
    </n-grid>
  </div>
</template>
<script lang="ts">
  import { get } from '@/api/http'
  import { baseUserInfoApi } from '@/api/url'
  import { useMessage } from 'naive-ui'
  import { defineComponent, h, onMounted, ref } from 'vue'
  export default defineComponent({
    name: 'RegIndex',
    setup() {
      const userIdRef = ref('')
      const submitLoaing = ref(false)
      const message = useMessage()

      async function getUserInfo() {
        const userInfo = await get({
          url: baseUserInfoApi.getLoginInfo,
        })

        userIdRef.value = userInfo.data.userId
      }

      async function onSubmit() {
        submitLoaing.value = true
        const createResult = await get({
          url: baseUserInfoApi.create,
        }).finally(() => {
          submitLoaing.value = false
        })

        message.success(createResult.msg)
      }

      onMounted(async () => {
        await getUserInfo()
      })
      return {
        userIdRef,
        submitLoaing,
        onSubmit,
      }
    },
  })
</script>
