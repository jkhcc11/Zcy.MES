<script setup lang="ts">
import { ClientTypeEnumList } from '@/services/constants'
import { createAndUpdateSupplierClientApi } from '@/services/supplierClient'
import { useUserInfoStore } from '@/stores'
import { getUserInfoAuth } from '@/utils'
import { onLoad } from '@dcloudio/uni-app'
import { reactive, ref } from 'vue'

// 是否适配底部安全区域
defineProps<{
  safeAreaInsetBottom?: boolean
}>()

const isAuth = ref(-1)
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
    if (!submitForm.clientName) {
      uni.showToast({ icon: 'error', title: '请输入客户名' })
      return
    }

    if (!submitForm.clientType) {
      uni.showToast({ icon: 'error', title: '请选择客户类型' })
      return
    }

    uni.showLoading({
      title: '提交中...',
      mask: true,
    })

    const res = await createAndUpdateSupplierClientApi(submitForm)
    uni.hideLoading()

    if (res.isSuccess === false) {
      uni.showToast({ icon: 'none', title: res.msg })
      return
    }

    // 成功提示
    uni.showToast({ icon: 'success', title: res.msg })
    // 返回上一页
    setTimeout(() => {
      uni.navigateTo({ url: 'supplier-client-list' })
      //   uni.navigateBack()
    }, 400)
  } catch (error: any) {
    uni.hideLoading()
    uni.showToast({ icon: 'error', title: `系统异常,${error.statusCode}` })
  }
}

//表单数据
const submitForm = reactive<any>({
  clientName: null,
  clientType: null,
  phoneNumber: null,
  remark: null,
})

//picker显示
const pickerRef = reactive({
  clientTypeShow: '',
})

//统一picker变更
const onPickerChanage = (e: any) => {
  //selector 是index date是值
  const currentValue = e.detail.value
  if (e.target.dataset.type) {
    switch (e.target.dataset.type) {
      case 'clientType': {
        submitForm['clientType'] = ClientTypeEnumList[currentValue].value
        pickerRef.clientTypeShow = ClientTypeEnumList[currentValue].text
        break
      }
    }
  }
}

//用户信息授权
async function userInfoAuth() {
  // 判断是否授权
  const authRes = await getUserInfoAuth(true)
  console.log('authRes', authRes)
  if (authRes) {
    isAuth.value = 1
  } else {
    //拒绝，需要重新唤起 授权
    isAuth.value = 0

    wx.openSetting({
      success: (res) => {
        if (res.authSetting['scope.userInfo']) {
          isAuth.value = 1
        }
      },
    })
  }
}

onLoad(async () => {
  await userInfoAuth()
})
</script>

<template>
  <scroll-view enable-back-to-top scroll-y class="scroll-view">
    <!-- 订单基础数据 -->
    <uni-forms :model="submitForm" calss="form">
      <view class="form-content">
        <!-- 表单内容 -->
        <view class="form-item">
          <text class="label">客户名称</text>
          <input
            class="input"
            placeholder="例如：张总"
            :maxlength="20"
            v-model="submitForm.clientName"
          />
        </view>
        <view class="form-item">
          <text class="label">客户类型</text>
          <picker
            class="picker"
            mode="selector"
            :range="ClientTypeEnumList"
            range-key="text"
            @change="onPickerChanage"
            data-type="clientType"
            :index="2"
          >
            <view v-if="pickerRef.clientTypeShow">{{ pickerRef.clientTypeShow }}</view>
            <view v-else class="placeholder">请选择客户类型</view>
          </picker>
        </view>
        <view class="form-item">
          <text class="label">联系方式</text>
          <input
            class="input"
            placeholder="联系方式"
            :maxlength="20"
            v-model="submitForm.phoneNumber"
          />
        </view>
        <view class="form-item">
          <text class="label">备注</text>
          <textarea
            placeholder="备注"
            class="input"
            :auto-height="true"
            v-model="submitForm.remark"
          />
        </view>
      </view>
    </uni-forms>

    <!-- 提交按钮 -->
    <button @tap="onSubmit" class="button" v-if="isAuth == 1">保存</button>
    <navigator url="/pages/new-index/index" hover-class="navigator-hover" v-if="isAuth !== 1">
      返回
    </navigator>
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
