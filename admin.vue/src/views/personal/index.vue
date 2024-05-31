<template>
  <div class="main-container">
    <n-card title="基础信息">
      <n-skeleton v-if="submitLoading" text :repeat="6" />
      <template v-else>
        <DataForm ref="editForm" :form-config="formConfig" :options="PersonFormOptions" />
      </template>
      <n-button type="primary" @click="onConfirm" :loading="submitLoading"> 保存 </n-button>
    </n-card>
  </div>
</template>

<script lang="ts">
  import { get, post } from '@/api/http'
  import { baseUserInfoApi } from '@/api/url'
  import { DataFormType, ModalDialogType } from '@/types/components'
  import { FormProps, useDialog, useMessage } from 'naive-ui'
  import { defineComponent, h, onMounted, ref } from 'vue'
  import { PersonFormOptions } from './Data'
  import useUserParseConfigStore from '@/store/modules/user-parse-config'
  export default defineComponent({
    name: 'PersonalInfo',
    setup() {
      const submitLoading = ref(false)
      const naiveDialog = useDialog()
      const message = useMessage()
      const userParseConfigStore = useUserParseConfigStore()

      //新增或编辑
      const modalDialog = ref<ModalDialogType | null>(null)
      const editForm = ref<DataFormType | null>(null)
      const formConfig = {
        labelWidth: 100,
        size: 'medium',
        labelAlign: 'right',
      } as FormProps

      function onConfirm() {
        if (editForm.value?.validator()) {
          const pd = editForm.value?.generatorParams()
          submitLoading.value = true
          post({
            url: baseUserInfoApi.saveInfo,
            data: pd,
          })
            .then((res) => {
              if (res.isSuccess) {
                message.success(res.msg)
              } else {
                message.error(res.msg)
              }
            })
            .finally(() => {
              submitLoading.value = false
              //变更后重新获取
              userParseConfigStore.reset()
              userParseConfigStore.init()
            })
        }
      }

      //初始化数据
      async function initData() {
        const req = await get({
          url: baseUserInfoApi.getLoginInfo,
        })

        PersonFormOptions.forEach((it: any) => {
          it.value.value = req.data[it.key] || null
        })
      }

      onMounted(async () => {
        await initData()
      })
      return {
        submitLoading,
        PersonFormOptions,
        modalDialog,
        editForm,
        formConfig,
        onConfirm,
      }
    },
  })
</script>
