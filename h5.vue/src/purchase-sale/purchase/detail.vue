<script setup lang="ts">
import { AccountTypeEnum, AccountTypeEnumList } from '@/services/constants'
import { getPurchaseOrderDetailApi } from '@/services/new-order'
import { onLoad } from '@dcloudio/uni-app'
import { ref } from 'vue'

// 获取屏幕边界到安全区域距离
const { safeAreaInsets } = uni.getSystemInfoSync()
// 复制内容
const onCopy = (id: string) => {
  // 设置系统剪贴板的内容
  uni.setClipboardData({ data: id })
}

// 获取页面参数
const query = defineProps<{
  id: string
}>()

// 获取采购订单详情
const orderDetail = ref<any>()
const getOrderDetail = async () => {
  const res = await getPurchaseOrderDetailApi(query.id)
  orderDetail.value = res.data
}

//账号类型item
const getAccountTypeItem = (accountType: AccountTypeEnum) => {
  return AccountTypeEnumList.filter((item) => item.value == accountType)[0]
}

onLoad(() => {
  getOrderDetail()
})
</script>

<template>
  <view class="viewport">
    <template v-if="orderDetail">
      <!-- 客户信息 -->
      <view class="shipment">
        <view class="locate">
          <view class="user"> 客户名：{{ orderDetail.supplierClientName }} </view>
          <view class="address"> 订单日期：{{ orderDetail.orderDate }} </view>

          <uni-tag
            :text="getAccountTypeItem(orderDetail.accountType).text"
            :type="getAccountTypeItem(orderDetail.accountType).tagType"
            :inverted="true"
            size="small"
          />
        </view>
      </view>

      <!-- 产品信息 -->
      <view class="goods">
        <view class="item">
          <view class="navigator" v-for="item in orderDetail.orderDetails" :key="item.id">
            <!-- <image class="cover" :src="item.image"></image> -->
            <view class="meta">
              <view class="name ellipsis">{{ item.productName }}</view>
              <!-- 散件 -->
              <view class="type" v-if="item.unit && item.isLoose">
                单价：{{ item.unitPrice }} , 规格：{{ item.unit }}
              </view>

              <!-- 非散件 -->
              <view class="type" v-if="item.unit && !item.isLoose">
                单价：{{ item.unitPrice }} , 规格：{{ item.spec }} ({{ item.specCount }}
                {{ item.unit }})
              </view>
              <view class="type" v-if="item.remark"> 备注：{{ item.remark }} </view>
              <view class="price">
                <view class="actual">
                  <text class="symbol">¥</text>
                  <text>{{ item.sumPrice }}</text>
                </view>
              </view>

              <!-- 单价 -->
              <text class="quantity">数量 x {{ item.count }}</text>
            </view>
          </view>
          <!-- 待评价状态:展示按钮 -->
          <!-- <view class="action" v-if="order.orderState === OrderState.DaiPingJia">
            <view class="button primary">申请售后</view>
            <navigator url="" class="button"> 去评价 </navigator>
          </view> -->
        </view>
        <!-- 合计 -->
        <view class="total">
          <view class="row">
            <view class="text">采购总价: </view>
            <view class="symbol">{{ orderDetail.orderPrice }}</view>
          </view>
          <!-- <view class="row">
            <view class="text">运费: </view>
            <view class="symbol">{{ orderDetail.freightPrice }}</view>
          </view> -->
          <view class="row">
            <view class="text">订单总价: </view>
            <view class="symbol primary">{{ orderDetail.orderPrice }}</view>
          </view>
        </view>
      </view>

      <!-- 订单信息 -->
      <view class="detail">
        <view class="title">订单信息</view>
        <view class="row">
          <view class="item">
            订单号: {{ orderDetail.orderNo }}
            <text class="copy" @tap="onCopy(orderDetail.orderNo)">复制</text>
          </view>
          <view class="item" v-if="orderDetail.orderRemark"
            >备注: {{ orderDetail.orderRemark }}</view
          >
          <view class="item">创建时间: {{ orderDetail.createdTime }}</view>
          <view class="item">经办人: {{ orderDetail.managerUser }}</view>
        </view>
      </view>
    </template>
  </view>
</template>

<style lang="scss">
page {
  display: flex;
  flex-direction: column;
  height: 100%;
  overflow: hidden;
  background-color: #f7f7f7;
}

.navbar {
  width: 750rpx;
  color: #000;
  position: fixed;
  top: 0;
  left: 0;
  z-index: 9;
  /* background-color: #f8f8f8; */
  background-color: transparent;

  .wrap {
    position: relative;

    .title {
      height: 44px;
      display: flex;
      justify-content: center;
      align-items: center;
      font-size: 32rpx;
      /* color: #000; */
      color: transparent;
    }

    .back {
      position: absolute;
      left: 0;
      height: 44px;
      width: 44px;
      font-size: 44rpx;
      display: flex;
      align-items: center;
      justify-content: center;
      /* color: #000; */
      color: #fff;
    }
  }
}

.shipment {
  line-height: 1.4;
  padding: 0 20rpx;
  margin: 20rpx 20rpx 0;
  border-radius: 10rpx;
  background-color: #fff;

  .locate,
  .item {
    min-height: 120rpx;
    padding: 30rpx 30rpx 25rpx 25rpx;
    background-size: 50rpx;
    background-repeat: no-repeat;
    background-position: 6rpx center;
  }

  .locate {
    .user {
      font-size: 26rpx;
      color: #444;
    }

    .address {
      font-size: 24rpx;
      color: #444;
      // color: #666;
    }
  }

  .item {
    border-bottom: 1rpx solid #eee;
    position: relative;

    .message {
      font-size: 26rpx;
      color: #444;
    }

    .date {
      font-size: 24rpx;
      color: #666;
    }
  }
}

.goods {
  margin: 20rpx 20rpx 0;
  padding: 0 20rpx;
  border-radius: 10rpx;
  background-color: #fff;

  .item {
    padding: 30rpx 0;
    border-bottom: 1rpx solid #eee;

    .navigator {
      display: flex;
      margin: 10rpx 0;
      padding: 10rpx;
    }

    .navigator:not(:last-child) {
      border-bottom: 1rpx solid #eee;
    }

    .cover {
      width: 170rpx;
      height: 170rpx;
      border-radius: 10rpx;
      margin-right: 20rpx;
    }

    .meta {
      flex: 1;
      display: flex;
      flex-direction: column;
      justify-content: center;
      position: relative;
    }

    .name {
      // height: 40rpx;
      font-size: 30rpx;
      color: #444;
    }

    .type {
      line-height: 1.8;
      padding: 0 15rpx;
      margin-top: 6rpx;
      font-size: 24rpx;
      align-self: flex-start;
      border-radius: 4rpx;
      color: #888;
      background-color: #f7f7f8;
    }

    .price {
      display: flex;
      margin-top: 6rpx;
      font-size: 24rpx;
    }

    .symbol {
      font-size: 20rpx;
    }

    .original {
      color: #999;
      text-decoration: line-through;
    }

    .actual {
      margin-left: 10rpx;
      color: #0c0b0b;
      font-weight: bolder;
    }

    .text {
      font-size: 22rpx;
    }

    .quantity {
      position: absolute;
      bottom: 0;
      right: 0;
      font-size: 24rpx;
      color: #444;
    }

    .action {
      display: flex;
      flex-direction: row-reverse;
      justify-content: flex-start;
      padding: 30rpx 0 0;

      .button {
        width: 200rpx;
        height: 60rpx;
        text-align: center;
        justify-content: center;
        line-height: 60rpx;
        margin-left: 20rpx;
        border-radius: 60rpx;
        border: 1rpx solid #ccc;
        font-size: 26rpx;
        color: #444;
      }

      .primary {
        color: #27ba9b;
        border-color: #27ba9b;
      }
    }
  }

  .total {
    line-height: 1;
    font-size: 26rpx;
    padding: 20rpx 0;
    color: #666;

    .row {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 10rpx 0;
    }

    .symbol::before {
      content: '¥';
      font-size: 80%;
      margin-right: 3rpx;
    }

    .primary {
      color: #cf4444;
      font-size: 36rpx;
    }
  }
}

.detail {
  line-height: 1;
  padding: 30rpx 20rpx 0;
  margin: 20rpx 20rpx 0;
  font-size: 26rpx;
  color: #666;
  border-radius: 10rpx;
  background-color: #fff;

  .title {
    font-size: 30rpx;
    color: #444;
  }

  .row {
    padding: 20rpx 0;

    .item {
      padding: 10rpx 0;
      display: flex;
      align-items: center;
    }

    .copy {
      border-radius: 20rpx;
      font-size: 20rpx;
      border: 1px solid #ccc;
      padding: 5rpx 10rpx;
      margin-left: 10rpx;
    }
  }
}
</style>
