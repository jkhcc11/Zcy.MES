<script setup lang="ts">
import {
  AccountTypeEnum,
  IncomeTypeEnum,
  AccountTypeEnumList,
  IncomeTypeEnumList,
} from '@/services/constants'
import { createIncomeRecordApi } from '@/services/financial-memo'
import { useUserInfoStore } from '@/stores'
import type { CreateIncomeRecordInput } from '@/types/financial-memo'
import { reactive } from 'vue'
import { getBusinessDefaultDateTimeRange } from '@/utils/index'

// 是否适配底部安全区域
defineProps<{
  safeAreaInsetBottom?: boolean
}>()

const useUserInfo = useUserInfoStore()

//创建订单
const onCreateOrder = async () => {
  if (!useUserInfo.userInfo) {
    return uni.showToast({
      icon: 'none',
      title: '请先登录',
    })
  }

  try {
    // 表单校验
    if (!incomeForm.incomeType) {
      uni.showToast({ icon: 'error', title: '请选择收支类型' })
      return
    }

    if (!incomeForm.accountType) {
      uni.showToast({ icon: 'error', title: '请选择收款账户' })
      return
    }

    if (!incomeForm.recordName) {
      uni.showToast({ icon: 'error', title: '款项名必填' })
      return
    }

    if (!incomeForm.managerUser) {
      uni.showToast({ icon: 'error', title: '经办人必填' })
      return
    }

    if (!incomeForm.money || incomeForm.money <= 0) {
      uni.showToast({ icon: 'error', title: '金额必填' })
      return
    }

    //todo:不能统一消除 小程序真机会把showToast也干掉
    uni.showLoading({
      title: '提交中...',
      mask: true,
    })
    const res = await createIncomeRecordApi(incomeForm)
    uni.hideLoading()

    if (res.isSuccess === false) {
      uni.showToast({ icon: 'none', title: res.msg })
      return
    }

    // 成功提示
    uni.showToast({ icon: 'success', title: res.msg })
    // 返回上一页
    setTimeout(() => {
      uni.navigateTo({ url: 'index' })
      //uni.navigateBack()
    }, 400)
  } catch (error: any) {
    uni.hideLoading()
    uni.showToast({ icon: 'error', title: `系统异常,${error.statusCode}` })
  }
}

//表单数据
const incomeForm = reactive<CreateIncomeRecordInput>({
  accountType: AccountTypeEnum.微信,
  incomeType: IncomeTypeEnum.进账,
  recordName: '',
  money: 0,
  managerUser: null,
  remark: null,
  recordDate: null,
})

//收款账户
const pickerRef = reactive({
  accountTypeShow: '微信',
  incomeTypeShow: '进账',
})

//统一picker变更
const onPickerChanage = (e: any) => {
  //selector 是index date是值
  const currentValue = e.detail.value
  if (e.target.dataset.type) {
    switch (e.target.dataset.type) {
      case 'accountType': {
        incomeForm['accountType'] = AccountTypeEnumList[currentValue].value
        pickerRef.accountTypeShow = AccountTypeEnumList[currentValue].text
        break
      }
      case 'incomeType': {
        incomeForm['incomeType'] = IncomeTypeEnumList[currentValue].value
        pickerRef.incomeTypeShow = IncomeTypeEnumList[currentValue].text
        break
      }
      case 'recordDate': {
        incomeForm['recordDate'] = currentValue
      }
    }
  }
}
</script>

<template>
  <scroll-view enable-back-to-top scroll-y class="scroll-view">
    <!-- 订单基础数据 -->
    <uni-forms :model="incomeForm" calss="form">
      <view class="form-content">
        <!-- 表单内容 -->
        <view name="recordName" class="form-item">
          <text class="label">款项名</text>
          <input
            class="input"
            placeholder="例如：购买xx设备"
            :maxlength="20"
            v-model="incomeForm.recordName"
          />
        </view>
        <view name="incomeType" class="form-item">
          <text class="label">收支类型</text>
          <picker
            class="picker"
            mode="selector"
            :range="IncomeTypeEnumList"
            range-key="text"
            @change="onPickerChanage"
            data-type="incomeType"
            :index="2"
          >
            <view v-if="pickerRef.incomeTypeShow">{{ pickerRef.incomeTypeShow }}</view>
            <view v-else class="placeholder">请选择收支类型</view>
          </picker>
        </view>
        <view name="accountType" class="form-item">
          <text class="label">收款账户</text>
          <picker
            class="picker"
            mode="selector"
            :range="AccountTypeEnumList"
            range-key="text"
            @change="onPickerChanage"
            data-type="accountType"
            :index="2"
          >
            <view v-if="pickerRef.accountTypeShow">{{ pickerRef.accountTypeShow }}</view>
            <view v-else class="placeholder">请选择收款账户</view>
          </picker>
        </view>
        <view name="money" class="form-item">
          <text class="label">金额</text>
          <input
            class="input"
            placeholder="金额"
            type="digit"
            :maxlength="9"
            v-model="incomeForm.money"
          />
        </view>
        <view name="managerUser" class="form-item">
          <text class="label">经办人</text>
          <input
            class="input"
            placeholder="付款人 例如：xx 付款"
            :maxlength="20"
            v-model="incomeForm.managerUser"
          />
        </view>
        <view name="recordDate" class="form-item">
          <text class="label">订单日期</text>
          <picker
            @change="onPickerChanage"
            class="picker"
            mode="date"
            :start="getBusinessDefaultDateTimeRange().sDate"
            :end="getBusinessDefaultDateTimeRange().eDate"
            data-type="recordDate"
          >
            <view v-if="incomeForm.recordDate">{{ incomeForm.recordDate }}</view>
            <view v-else class="placeholder">不选为当天</view>
          </picker>
        </view>

        <view name="remark" class="form-item">
          <text class="label">备注</text>
          <textarea
            placeholder="备注"
            class="input"
            :auto-height="true"
            v-model="incomeForm.remark"
          />
        </view>
      </view>
    </uni-forms>

    <!-- 提交按钮 -->
    <button @tap="onCreateOrder" class="button">保存</button>
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

  &-button {
    height: 80rpx;
    text-align: center;
    line-height: 80rpx;
    margin: 30rpx 20rpx;
    color: #fff;
    border-radius: 80rpx;
    font-size: 30rpx;
    background-color: #27ba9b;
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
