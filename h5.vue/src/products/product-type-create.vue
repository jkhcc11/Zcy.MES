<script setup lang="ts">
import { createUpdateProductTypeApi } from '@/services/product'
import { useUserInfoStore } from '@/stores'
import { reactive } from 'vue'

// 是否适配底部安全区域
defineProps<{
  safeAreaInsetBottom?: boolean
}>()

const useUserInfo = useUserInfoStore()

//提交保存
const onSubmit = async () => {
  if (!useUserInfo.userInfo) {
    return uni.showToast({
      icon: 'none',
      title: '请先登录',
    })
  }

  try {
    // 表单校验
    if (!submitForm.typeName) {
      uni.showToast({ icon: 'error', title: '请输入客户名' })
      return
    }

    uni.showLoading({
      title: '提交中...',
      mask: true,
    })

    const res = await createUpdateProductTypeApi(submitForm)
    uni.hideLoading()
    if (res.isSuccess === false) {
      uni.showToast({ icon: 'none', title: res.msg })
      return
    }

    // 成功提示
    uni.showToast({ icon: 'success', title: res.msg })
    // 返回上一页
    setTimeout(() => {
      uni.navigateTo({ url: 'product-type-index' })
      //   uni.navigateBack()
    }, 400)
  } catch (error: any) {
    uni.hideLoading()
    uni.showToast({ icon: 'error', title: `系统异常,${error.statusCode}` })
  }
}

//表单数据
const submitForm = reactive<any>({
  typeName: null,
})
</script>

<template>
  <scroll-view enable-back-to-top scroll-y class="scroll-view">
    <!-- 订单基础数据 -->
    <uni-forms :model="submitForm" calss="form">
      <view class="form-content">
        <!-- 表单内容 -->
        <view class="form-item">
          <text class="label">分类名称</text>
          <input
            class="input"
            placeholder="产品分类名称"
            :maxlength="10"
            v-model="submitForm.typeName"
          />
        </view>
      </view>
    </uni-forms>

    <!-- 提交按钮 -->
    <button @tap="onSubmit" class="button">保存</button>
    <!-- 底部占位空盒子 -->
    <view class="toolbar-height"></view>
  </scroll-view>
</template>

<style lang="scss">
// 根元素
:host {
  height: 100vh;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  background-color: #f7f7f8;
}

.form {
  background-color: #f4f4f4;

  &-content {
    margin: 20rpx 20rpx 0;
    padding: 0 20rpx;
    border-radius: 10rpx;
    background-color: #fff;
  }

  &-item {
    display: flex;
    height: 96rpx;
    line-height: 46rpx;
    padding: 25rpx 10rpx;
    background-color: #fff;
    font-size: 28rpx;
    border-bottom: 1rpx solid #ddd;

    &:last-child {
      border: none;
    }

    .label {
      width: 180rpx;
      color: #333;
    }

    .account {
      color: #666;
    }

    .input {
      flex: 1;
      display: block;
      height: 46rpx;
    }

    .radio {
      margin-right: 20rpx;
    }

    .picker {
      flex: 1;
    }
    .placeholder {
      color: #808080;
    }
  }
}

// 滚动容器
.scroll-view {
  flex: 1;
  background-color: #f7f7f8;
}

.button {
  height: 80rpx;
  margin: 30rpx 20rpx;
  color: #fff;
  border-radius: 80rpx;
  font-size: 30rpx;
  background-color: #27ba9b;
}

// 底部占位空盒子
.toolbar-height {
  height: 100rpx;
}
</style>
