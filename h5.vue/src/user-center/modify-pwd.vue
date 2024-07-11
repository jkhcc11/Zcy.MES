<script setup lang="ts">
import { modifyUserPwdApi } from '@/services/user-center'
import { reactive, ref } from 'vue'

// 表单数据
const form = reactive({
  oldPwd: '', // 旧密码
  newPwd: '', // 新密码
  newPwd1: '', // 确认新密码
})

// 动态设置标题
uni.setNavigationBarTitle({ title: '修改密码' })

// 定义校验规则
const rules: UniHelper.UniFormsRules = {
  oldPwd: {
    rules: [{ required: true, errorMessage: '旧密码不能为空' }],
  },
  newPwd: {
    rules: [{ required: true, errorMessage: '新密码不能为空' }],
  },
  newPwd1: {
    rules: [
      { required: true, errorMessage: '请确认密码' },
      {
        validateFunction: function (rule, value, data, callback) {
          if (data.newPwd != value) {
            callback('两次密码不一致')
            return false
          }

          //   console.log('rule', rule)
          //   //form表单数据
          //   console.log('data', data)
          return true
        },
      },
    ],
  },
}

// 表单组件实例
const formRef = ref<UniHelper.UniFormsInstance>()
// 提交表单
const onSubmit = async () => {
  try {
    // 表单校验
    await formRef.value?.validate?.()

    uni.showModal({
      content: `确认后将退出登录，重新使用新密码登录?`,
      confirmColor: '#18bc37',
      success: async (res) => {
        if (res.confirm) {
          const res = await modifyUserPwdApi(form)
          if (!res.isSuccess) {
            // 成功提示
            uni.showToast({ icon: 'error', title: res.msg })
            return
          }

          // 成功提示
          uni.showToast({ icon: 'success', title: '修改成功' })
          // 返回上一页
          setTimeout(() => {
            // uni.navigateBack()
            uni.navigateTo({ url: `/pages/login/login` })
          }, 400)
        }
      },
    })
  } catch (error) {
    uni.showToast({ icon: 'error', title: '请填写完整信息' })
  }
}
</script>

<template>
  <view class="content">
    <uni-forms :rules="rules" :model="form" ref="formRef">
      <!-- 表单内容 -->
      <uni-forms-item name="oldPwd" class="form-item">
        <text class="label">旧密码</text>
        <input
          class="input"
          placeholder="旧密码"
          :maxlength="20"
          type="password"
          v-model="form.oldPwd"
        />
      </uni-forms-item>
      <uni-forms-item name="newPwd" class="form-item">
        <text class="label">新密码</text>
        <input
          class="input"
          placeholder="新密码"
          :maxlength="20"
          type="password"
          v-model="form.newPwd"
        />
      </uni-forms-item>
      <uni-forms-item name="newPwd1" class="form-item">
        <text class="label">确认密码</text>
        <input
          class="input"
          placeholder="确认密码"
          :maxlength="20"
          type="password"
          v-model="form.newPwd1"
        />
      </uni-forms-item>
    </uni-forms>
  </view>
  <!-- 提交按钮 -->
  <button @tap="onSubmit" class="button">确认修改</button>
</template>

<style lang="scss">
// 深度选择器修改 uni-data-picker 组件样式
:deep(.selected-area) {
  flex: 0 1 auto;
  height: auto;
}

page {
  background-color: #f4f4f4;
}

.content {
  margin: 20rpx 20rpx 0;
  padding: 0 20rpx;
  border-radius: 10rpx;
  background-color: #fff;

  .form-item,
  .uni-forms-item {
    display: flex;
    align-items: center;
    min-height: 96rpx;
    padding: 25rpx 10rpx;
    background-color: #fff;
    font-size: 28rpx;
    border-bottom: 1rpx solid #ddd;
    position: relative;
    margin-bottom: 0;

    // 调整 uni-forms 样式
    .uni-forms-item__content {
      display: flex;
    }

    .uni-forms-item__error {
      margin-left: 200rpx;
    }

    &:last-child {
      border: none;
    }

    .label {
      width: 200rpx;
      color: #333;
    }

    .input {
      flex: 1;
      display: block;
      height: 46rpx;
    }

    .switch {
      position: absolute;
      right: -20rpx;
      transform: scale(0.8);
    }

    .picker {
      flex: 1;
    }

    .placeholder {
      color: #808080;
    }
  }
}

.button {
  height: 80rpx;
  margin: 30rpx 20rpx;
  color: #fff;
  border-radius: 80rpx;
  font-size: 30rpx;
  background-color: #27ba9b;
}
</style>
