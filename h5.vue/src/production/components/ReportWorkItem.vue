<script setup lang="ts">
import {
  PublicStatusEnumList,
  BillingTypeEnumList,
  BillingTypeEnum,
  PublicStatusEnum,
} from '@/services/constants'

// 定义 porps
defineProps<{
  workReportItem: any
}>()

//枚举显示
const getStatusItem = (enumItem: PublicStatusEnum) => {
  return PublicStatusEnumList.filter((item) => item.value == enumItem)[0]
}
const getBillingTypeEnumItem = (enumItem: BillingTypeEnum) => {
  return BillingTypeEnumList.filter((item) => item.value == enumItem)[0]
}
</script>

<template>
  <view class="card" :key="workReportItem.id">
    <!-- 报工记录 -->
    <view class="status">
      <text> {{ workReportItem.productName }}/{{ workReportItem.productCraftName }}</text>
      <uni-tag
        :text="getStatusItem(workReportItem.reportWorkStatus).text"
        :type="getStatusItem(workReportItem.reportWorkStatus).tagType"
        :inverted="true"
        size="small"
      />
    </view>
    <view class="meta">
      <view class="name ellipsis" v-if="workReportItem.billingType == BillingTypeEnum.计件">
        工作量：{{ workReportItem.wordDuration }} 个
      </view>
      <view class="name ellipsis" v-if="workReportItem.billingType == BillingTypeEnum.计时">
        工作量：{{ workReportItem.wordDuration }} 小时
      </view>
      <!-- <view class="type" v-if="dataItem.productRemark">备注：{{ dataItem.productRemark }}</view> -->
    </view>
    <view class="payment">
      <view class="payment_left">
        <text class="quantity" v-if="workReportItem.employeeNickName"
          >员工名：{{ workReportItem.employeeNickName }}</text
        >

        <text class="quantity" v-if="workReportItem.remark">备注：{{ workReportItem.remark }}</text>
      </view>
      <view class="payment_right">
        <uni-tag
          :text="getBillingTypeEnumItem(workReportItem.billingType).text"
          :type="getBillingTypeEnumItem(workReportItem.billingType).tagType"
          :inverted="true"
          size="small"
        />
      </view>
    </view>

    <!-- 操作按钮 -->
    <view class="action">
      <view class="action-left">
        <text>日期：{{ workReportItem.reportWorkDate }}</text>
      </view>
      <view class="action-right">
        <slot name="actionRight"></slot>
      </view>
    </view>
  </view>
</template>

<style lang="scss">
.card {
  min-height: 100rpx;
  padding: 20rpx;
  margin: 20rpx 20rpx 0;
  border-radius: 10rpx;
  background-color: #fff;

  &:last-child {
    padding-bottom: 40rpx;
  }
}

.status {
  display: flex;
  align-items: center;
  justify-content: space-between;
  font-size: 28rpx;
  color: #999;
  margin-bottom: 15rpx;

  .date {
    color: #666;
    flex: 1;
  }

  .primary {
    color: #ff9240;
  }

  .icon-delete {
    line-height: 1;
    margin-left: 10rpx;
    padding-left: 10rpx;
    border-left: 1rpx solid #e3e3e3;
  }
}

.payment {
  display: flex;
  justify-content: space-between;
  align-items: center;
  line-height: 1;
  padding: 20rpx 0;
  text-align: right;
  color: #999;
  font-size: 28rpx;
  border-bottom: 1rpx solid #eee;

  .payment_left {
    justify-content: flex-start;
  }
  .payment_right {
    justify-content: flex-end;
  }

  .quantity {
    font-size: 24rpx;
    margin-right: 16rpx;
  }

  .amount {
    color: #444;
    margin-left: 6rpx;
  }

  .symbol {
    font-size: 20rpx;
  }
}

.action {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 10rpx;

  .action-right {
    justify-content: flex-end;
  }

  .action-left {
    justify-content: flex-start;
  }
}
</style>
