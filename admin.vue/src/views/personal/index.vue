<template>
  <div class="main-container">
    <n-card title="修改密码">
      <n-skeleton v-if="submitLoading" text :repeat="6" />
      <template v-else>
        <DataForm ref="editForm" :form-config="formConfig" :options="ModifyPwdFormOptions" />
      </template>
      <n-button type="primary" @click="onConfirm" :loading="submitLoading"> 保存 </n-button>
      <n-gradient-text type="warning"> 注：保存后将直接退出，需要重新登录 </n-gradient-text>
    </n-card>
  </div>
</template>

<script lang="ts">
  import { post } from '@/api/http'
  import { userApi } from '@/api/url'
  import { DataFormType, ModalDialogType } from '@/types/components'
  import { FormProps, useDialog, useMessage } from 'naive-ui'
  import { defineComponent, onMounted, ref } from 'vue'
  import { ModifyPwdFormOptions } from './Data'
  import useUserStore from '@/store/modules/user'
  export default defineComponent({
    name: 'PersonalInfo',
    setup() {
      const submitLoading = ref(false)
      const naiveDialog = useDialog()
      const message = useMessage()
      const useUser = useUserStore()

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
            url: userApi.modifyPwd,
            data: pd,
          })
            .then((res) => {
              message.success(res.msg)
              //变更后重新获取
              useUser.logout()

              naiveDialog.warning({
                title: '提示',
                content: '即将前往登录页面？',
                positiveText: '确认',
                onPositiveClick: () => {
                  location.reload()
                },
              })
            })
            .finally(() => {
              submitLoading.value = false
            })
        }
      }

      //初始化数据
      function initData() {
        ModifyPwdFormOptions.forEach((it: any) => {
          if (it.key == 'userNick') {
            it.value.value = useUser.userNick
          }
        })
      }

      onMounted(() => {
        initData()
      })
      return {
        submitLoading,
        ModifyPwdFormOptions,
        modalDialog,
        editForm,
        formConfig,
        onConfirm,
      }
    },
  })
</script>
