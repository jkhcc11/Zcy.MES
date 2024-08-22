<script setup lang="ts">
import { useUserInfoStore } from '@/stores'
import { onMounted, ref, watch } from 'vue'
import { getLoginInfoWithWxAsync } from '@/services/new-login'
import type { GetLoginInfoWithWxDto } from '@/types/userInfo'

const useUserInfo = useUserInfoStore()
const loginInfoRef = ref<GetLoginInfoWithWxDto | null>(null)

onMounted(async () => {
  if (!useUserInfo.userInfo) {
    uni.navigateTo({ url: `/pages/login/login` })
  }

  await loginInfo()
})
// 当前下拉刷新状态
const isTriggered = ref(false)
// 自定义下拉刷新被触发
const onRefresherrefresh = async () => {
  // 开始动画
  isTriggered.value = true
  await loginInfo()
  // 关闭动画
  isTriggered.value = false
}

//小程序登录权限
async function loginInfo() {
  const loginInfoResult = await getLoginInfoWithWxAsync()
  if (loginInfoResult.isSuccess) {
    loginInfoRef.value = loginInfoResult.data

    if (loginInfoRef.value.isBan) {
      //禁用返回登陆页
      useUserInfo.clearProfile()
      uni.navigateTo({ url: '/pages/login/login' })
    }
  }
}

//用户信息变化
watch(useUserInfo, async () => {
  await onRefresherrefresh()
})

// //表格点击
// function onGridChange(e: any) {
//   let { index } = e.detail
//   console.log('change', e)
//   uni.showToast({
//     title: `点击第${index}个宫格`,
//     icon: 'none',
//   })
// }
</script>

<template>
  <view class="viewport">
    <!-- 滚动容器 -->
    <scroll-view
      :enable-back-to-top="false"
      refresher-enabled
      @refresherrefresh="onRefresherrefresh"
      :refresher-triggered="isTriggered"
      class="scroll-view"
      scroll-y
    >
      <uni-card title="基础数据" v-if="loginInfoRef?.isShowBaseData">
        <uni-grid :column="4" :showBorder="false" :square="false">
          <uni-grid-item :index="1">
            <navigator url="/products/product-type-index" hover-class="navigator-hover">
              <view class="grid-item-box">
                <image model="scaleToFill" src="/static/icons/product.png" class="image"></image>
                <text class="text">产品分类</text>
              </view>
            </navigator>
          </uni-grid-item>

          <uni-grid-item :index="3">
            <navigator url="/products/product-index" hover-class="navigator-hover">
              <view class="grid-item-box">
                <image model="scaleToFill" src="/static/icons/product.png" class="image"></image>
                <text class="text">产品列表</text>
              </view>
            </navigator>
          </uni-grid-item>

          <!-- 过审使用，只有Boss有 -->
          <uni-grid-item :index="2" v-if="loginInfoRef?.bossReport">
            <navigator url="/person-mgr/supplier-client-list" hover-class="navigator-hover">
              <view class="grid-item-box">
                <image model="scaleToFill" src="/static/icons/sys-info.png" class="image"></image>
                <text class="text">客户列表</text>
              </view>
            </navigator>
          </uni-grid-item>
        </uni-grid>
      </uni-card>

      <uni-card title="采购销售" v-if="loginInfoRef?.isPurchaseSaleShow">
        <uni-grid :column="4" :showBorder="false" :square="false">
          <uni-grid-item>
            <view class="grid-item-box">
              <navigator url="/purchase-sale/sale/edit" hover-class="navigator-hover">
                <image
                  model="scaleToFill"
                  src="/static/icons/create-order.png"
                  class="image"
                ></image>
                <view>
                  <text class="text">新增销售</text>
                </view>
              </navigator>
            </view>
          </uni-grid-item>
          <uni-grid-item>
            <view class="grid-item-box">
              <navigator url="/purchase-sale/sale/index" hover-class="navigator-hover">
                <image model="scaleToFill" src="/static/icons/sale-order.png" class="image"></image>
                <view>
                  <text class="text">销售订单</text>
                </view>
              </navigator>
            </view>
          </uni-grid-item>

          <uni-grid-item>
            <view class="grid-item-box">
              <navigator url="/purchase-sale/purchase/edit" hover-class="navigator-hover">
                <image
                  model="scaleToFill"
                  src="/static/icons/create-order.png"
                  class="image"
                ></image>
                <view>
                  <text class="text">新增采购</text>
                </view>
              </navigator>
            </view>
          </uni-grid-item>
          <uni-grid-item>
            <view class="grid-item-box">
              <navigator url="/purchase-sale/purchase/index" hover-class="navigator-hover">
                <image model="scaleToFill" src="/static/icons/sale-order.png" class="image"></image>
                <view>
                  <text class="text">采购订单</text>
                </view>
              </navigator>
            </view>
          </uni-grid-item>

          <uni-grid-item>
            <view class="grid-item-box">
              <navigator url="/purchase-sale/shipment/edit" hover-class="navigator-hover">
                <image
                  model="scaleToFill"
                  src="/static/icons/create-order.png"
                  class="image"
                ></image>
                <view>
                  <text class="text">新增出货</text>
                </view>
              </navigator>
            </view>
          </uni-grid-item>
          <uni-grid-item>
            <view class="grid-item-box">
              <navigator url="/purchase-sale/shipment/index" hover-class="navigator-hover">
                <image model="scaleToFill" src="/static/icons/sale-order.png" class="image"></image>
                <view>
                  <text class="text">出货订单</text>
                </view>
              </navigator>
            </view>
          </uni-grid-item>
        </uni-grid>
      </uni-card>

      <uni-card title="财务备忘录" v-if="loginInfoRef?.isFinancialMemoShow">
        <uni-grid :column="4" :showBorder="false" :square="false">
          <uni-grid-item :index="1">
            <navigator url="/financial-memo/proceeds/create" hover-class="navigator-hover">
              <view class="grid-item-box">
                <image
                  model="scaleToFill"
                  src="/static/icons/financial-memo.png"
                  class="image"
                ></image>
                <text class="text">创建收款</text>
              </view>
            </navigator>
          </uni-grid-item>
          <uni-grid-item :index="2">
            <navigator url="/financial-memo/proceeds/index" hover-class="navigator-hover">
              <view class="grid-item-box">
                <image
                  model="scaleToFill"
                  src="/static/icons/financial-memo.png"
                  class="image"
                ></image>
                <text class="text">收款列表</text>
              </view>
            </navigator>
          </uni-grid-item>

          <uni-grid-item :index="3">
            <navigator url="/financial-memo/income/create" hover-class="navigator-hover">
              <view class="grid-item-box">
                <image model="scaleToFill" src="/static/icons/income.png" class="image"></image>
                <text class="text">创建收支</text>
              </view>
            </navigator>
          </uni-grid-item>
          <uni-grid-item :index="4">
            <navigator url="/financial-memo/income/index" hover-class="navigator-hover">
              <view class="grid-item-box">
                <image model="scaleToFill" src="/static/icons/income.png" class="image"></image>
                <text class="text">收支列表</text>
              </view>
            </navigator>
          </uni-grid-item>
        </uni-grid>
      </uni-card>

      <uni-card title="生产管理" v-if="loginInfoRef?.meReport">
        <uni-grid :column="4" :showBorder="false" :square="false">
          <uni-grid-item :index="1" v-if="loginInfoRef?.adminReport">
            <navigator url="/production/report-work-admin" hover-class="navigator-hover">
              <view class="grid-item-box">
                <image model="scaleToFill" src="/static/icons/report.png" class="image"></image>
                <text class="text">报工记录</text>
              </view>
            </navigator>
          </uni-grid-item>

          <uni-grid-item :index="2" v-if="loginInfoRef?.bossReport">
            <navigator url="/production/report-work-boss" hover-class="navigator-hover">
              <view class="grid-item-box">
                <image model="scaleToFill" src="/static/icons/report.png" class="image"></image>
                <text class="text">报工统计</text>
              </view>
            </navigator>
          </uni-grid-item>

          <uni-grid-item :index="3">
            <navigator url="/production/report-work-me" hover-class="navigator-hover">
              <view class="grid-item-box">
                <image model="scaleToFill" src="/static/icons/report.png" class="image"></image>
                <text class="text">我的报工</text>
              </view>
            </navigator>
          </uni-grid-item>
        </uni-grid>
      </uni-card>
    </scroll-view>
  </view>
</template>

<style lang="scss">
page {
  background-color: #f7f7f7;
  height: 100%;
  overflow: hidden;
}

.grid-item-box {
  flex: 1;
  // position: relative;
  /* #ifndef APP-NVUE */
  display: flex;
  /* #endif */
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 15px 0;
}
.grid-item-box .image {
  width: 50px;
  height: 50px;
}
.grid-item-box .text {
  font-size: 14px;
  margin-top: 5px;
}

.viewport {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.scroll-view {
  flex: 1;
  overflow: hidden;
}
</style>
