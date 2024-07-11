<script setup lang="ts">
import { ClientTypeEnum, AccountTypeEnum, AccountTypeEnumList } from '@/services/constants'
import { createProceedsRecordApi } from '@/services/financial-memo'
import { useUserInfoStore } from '@/stores'
import { useSupplierClientStore } from '@/stores/modules/supplierClient'
import type { CreateProceedsRecordInput } from '@/types/financial-memo'
import { watch, reactive, onMounted } from 'vue'
import { getBusinessDefaultDateTimeRange } from '@/utils/index'

// 是否适配底部安全区域
defineProps<{
  safeAreaInsetBottom?: boolean
}>()

const useUserInfo = useUserInfoStore()
//客户
const useSupplierClient = useSupplierClientStore()

// 加载时
onMounted(() => {
  useSupplierClient.init(ClientTypeEnum.客户)
})

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
    if (!proceedsRecordForm.accountType) {
      uni.showToast({ icon: 'error', title: '请选择收款账户' })
      return
    }

    if (!proceedsRecordForm.proceedsRecordName) {
      uni.showToast({ icon: 'error', title: '款项名必填' })
      return
    }

    if (!proceedsRecordForm.supplierClientId) {
      uni.showToast({ icon: 'error', title: '请选择客户' })
      return
    }

    if (!proceedsRecordForm.money || proceedsRecordForm.money <= 0) {
      uni.showToast({ icon: 'error', title: '金额必填' })
      return
    }

    uni.showLoading({
      title: '提交中...',
      mask: true,
    })

    const res = await createProceedsRecordApi(proceedsRecordForm)
    uni.hideLoading()

    if (res.isSuccess === false) {
      uni.showToast({ icon: 'none', title: res.msg })
      return
    }

    // 成功提示
    uni.showToast({ icon: 'success', title: res.msg }).finally(() => {
      // 返回上一页
      setTimeout(() => {
        uni.navigateTo({ url: 'index' })
        //uni.navigateBack()
      }, 400)
    })
  } catch (error: any) {
    uni.hideLoading()
    uni.showToast({ icon: 'error', title: `系统异常,${error.statusCode}` })
  }
}

//表单数据
const proceedsRecordForm = reactive<CreateProceedsRecordInput>({
  accountType: AccountTypeEnum.微信,
  proceedsRecordName: '',
  supplierClientId: null,
  money: null,
  remark: null,
  recordDate: null,
})

//picker显示
const pickerRef = reactive({
  accountTypeShow: '微信',
  supplierClientShow: '',
})

//统一picker变更
const onPickerChanage = (e: any) => {
  //selector 是index date是值
  const currentValue = e.detail.value
  if (e.target.dataset.type) {
    switch (e.target.dataset.type) {
      case 'accountType': {
        proceedsRecordForm['accountType'] = AccountTypeEnumList[currentValue].value
        pickerRef.accountTypeShow = AccountTypeEnumList[currentValue].text
        break
      }
      case 'supplierClientId': {
        const clientItem = useSupplierClient.getItem(currentValue)
        proceedsRecordForm['supplierClientId'] = clientItem.id
        pickerRef.supplierClientShow = clientItem.clientName
        break
      }
      case 'recordDate': {
        proceedsRecordForm['recordDate'] = currentValue
        break
      }
    }
  }
}

//用户信息变化
watch(useUserInfo, () => {
  useSupplierClient.init(ClientTypeEnum.客户)
})
</script>

<template>
  <scroll-view enable-back-to-top scroll-y class="scroll-view">
    <!-- 订单基础数据 -->
    <uni-forms :model="proceedsRecordForm" calss="form">
      <view class="form-content">
        <!-- 表单内容 -->
        <view name="proceedsRecordName" class="form-item">
          <text class="label">款项名</text>
          <input
            class="input"
            placeholder="例如：xx货款"
            :maxlength="20"
            v-model="proceedsRecordForm.proceedsRecordName"
          />
        </view>
        <view name="supplierClientId" class="form-item">
          <text class="label">客户</text>
          <picker
            class="picker"
            mode="selector"
            :range="useSupplierClient.valiedClientArray"
            range-key="clientName"
            @change="onPickerChanage"
            data-type="supplierClientId"
            :index="2"
          >
            <view v-if="pickerRef.supplierClientShow">{{ pickerRef.supplierClientShow }}</view>
            <view v-else class="placeholder">请选择客户</view>
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
            v-model="proceedsRecordForm.money"
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
            <view v-if="proceedsRecordForm.recordDate">{{ proceedsRecordForm.recordDate }}</view>
            <view v-else class="placeholder">不选为当天</view>
          </picker>
        </view>

        <view name="remark" class="form-item">
          <text class="label">备注</text>
          <textarea
            placeholder="备注"
            class="input"
            :auto-height="true"
            v-model="proceedsRecordForm.remark"
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
