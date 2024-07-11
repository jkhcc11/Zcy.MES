<script setup lang="ts">
import {
  AccountTypeEnum,
  SaleOrderStatusEnum,
  AccountTypeEnumList,
  SaleOrderStatusEnumList,
  ClientTypeEnum,
  ProductTypeEnum,
} from '@/services/constants'
import { createSaleOrderApi } from '@/services/new-order'
import { useUserInfoStore } from '@/stores'
import type { CreateSaleOrderInput, CreateSaleOrderDetailInput } from '@/types/new-order'
import { onShow } from '@dcloudio/uni-app'
import { onMounted, reactive, watch } from 'vue'
import { useProductSelectedStore } from '@/stores/modules/productSelected'
import { useSupplierClientStore } from '@/stores/modules/supplierClient'
import { getBusinessDefaultDateTimeRange } from '@/utils/index'

// 是否适配底部安全区域
defineProps<{
  safeAreaInsetBottom?: boolean
}>()

// 获取屏幕边界到安全区域距离
const { safeAreaInsets } = uni.getSystemInfoSync()

// 用户信息
const useUserInfo = useUserInfoStore()
//产品选择持久化
const useProductSelected = useProductSelectedStore()
//客户
const useSupplierClient = useSupplierClientStore()

//产品列表
const productList = reactive<CreateSaleOrderDetailInput[]>([])
const totalRef = reactive({
  totalCount: 0,
  totalPrice: '0',
  productCount: 0,
})

// 初始化调用: 页面显示触发
onShow(() => {
  if (useProductSelected.selectedProducts && useProductSelected.selectedProducts.length > 0) {
    // 获取去重后的产品列表
    const uniqueProducts = useProductSelected.selectedProducts.filter((item: any) => {
      return !productList.some((product) => product.productId === item.id)
    })

    if (uniqueProducts.length > 0) {
      productList.push(
        ...uniqueProducts.map((item: any) => {
          return {
            productId: item.id,
            count: 1,
            unitPrice: 0.01,
            productName: item.productName,
            isLoose: item.isLoose,
            unit: item.unit,
            spec: item.spec,
            specCount: item.specCount,
          } as CreateSaleOrderDetailInput
        }),
      )
    }
  }
})

// 加载时
onMounted(() => {
  useSupplierClient.init(ClientTypeEnum.客户)
})

// 点击删除按钮
const onDeleteCart = (productItem: any) => {
  // 弹窗二次确认
  uni.showModal({
    content: `是否移除【${productItem.productName}】?`,
    confirmColor: '#27BA9B',
    success: (res) => {
      if (res.confirm) {
        //
        const newProducts = productList.filter((item) => item.productId != productItem.productId)
        productList.length = 0
        productList.push(...newProducts)
      }
    },
  })
}

//创建订单
const onCreateOrder = async () => {
  if (productList.length === 0) {
    return uni.showToast({
      icon: 'none',
      title: '请选择产品',
    })
  }

  try {
    // 表单校验
    if (!orderForm.supplierId) {
      uni.showToast({ icon: 'error', title: '请选择客户' })
      return
    }

    if (!orderForm.accountType) {
      uni.showToast({ icon: 'error', title: '请选择收款账户' })
      return
    }

    if (!orderForm.saleOrderStatus) {
      uni.showToast({ icon: 'error', title: '请选择状态' })
      return
    }

    orderForm.orderDetails = productList
    uni.showLoading({
      title: '提交中...',
      mask: true,
    })

    const res = await createSaleOrderApi(orderForm)
    uni.hideLoading()
    if (res.isSuccess === false) {
      uni.showToast({ icon: 'none', title: res.msg })
      return
    }

    // 成功提示
    uni.showToast({ icon: 'success', title: res.msg })
    useProductSelected.clearInfo()
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

// 订单表单数据
const orderForm = reactive<CreateSaleOrderInput>({
  accountType: AccountTypeEnum.微信,
  supplierId: null,
  freightPrice: 0,
  saleOrderStatus: SaleOrderStatusEnum.已完成,
  orderDetails: null,
  managerUserId: null,
  orderRemark: null,
  orderDate: null,
})

//收款账户
const pickerRef = reactive({
  accountTypeShow: '微信',
  saleOrderStatusShow: '已完成',
  supplierIdShow: '',
})

//统一picker变更
const onPickerChanage = (e: any) => {
  //selector 是index date是值
  const currentValue = e.detail.value
  if (e.target.dataset.type) {
    switch (e.target.dataset.type) {
      case 'accountType': {
        orderForm['accountType'] = AccountTypeEnumList[currentValue].value
        pickerRef.accountTypeShow = AccountTypeEnumList[currentValue].text
        break
      }
      case 'saleOrderStatus': {
        orderForm['saleOrderStatus'] = SaleOrderStatusEnumList[currentValue].value
        pickerRef.saleOrderStatusShow = SaleOrderStatusEnumList[currentValue].text
        break
      }
      case 'orderDate': {
        orderForm['orderDate'] = currentValue
        break
      }
      case 'supplierId': {
        const clientItem = useSupplierClient.getItem(currentValue)
        orderForm['supplierId'] = clientItem.id
        pickerRef.supplierIdShow = clientItem.clientName
        break
      }
    }
  }
}

//选择产品
function openSelectedProduct() {
  // 跳转到结算页
  uni.navigateTo({ url: `../product?productType=${ProductTypeEnum.销售产品}` })
  //this.$refs.popup.open(type)
}

//监听产品列表变化
watch(productList, () => {
  totalRef.totalCount = productList.reduce((sum, item) => sum + item.count, 0)
  totalRef.totalPrice = (
    productList.reduce((sum, item) => {
      if (item.isLoose) {
        return sum + item.count * item.unitPrice
      }

      return sum + item.count * item.specCount * item.unitPrice
    }, 0) + Number(orderForm.freightPrice)
  ).toFixed(2)
  totalRef.productCount = productList.length
})

//监听产品列表变化
watch(orderForm, () => {
  if (orderForm.freightPrice) {
    totalRef.totalPrice = (
      productList.reduce((sum, item) => {
        if (item.isLoose) {
          return sum + item.count * item.unitPrice
        }

        return sum + item.count * item.specCount * item.unitPrice
      }, 0) + Number(orderForm.freightPrice)
    ).toFixed(2)
  }
})

//用户信息变化
watch(useUserInfo, () => {
  useSupplierClient.init(ClientTypeEnum.客户)
})
</script>

<template>
  <scroll-view enable-back-to-top scroll-y class="scroll-view">
    <!-- 订单基础数据 -->
    <uni-forms :model="orderForm" calss="form">
      <view class="form-content">
        <!-- 表单内容 -->
        <view name="supplierId" class="form-item">
          <text class="label">客户</text>
          <picker
            class="picker"
            mode="selector"
            :range="useSupplierClient.valiedClientArray"
            range-key="clientName"
            @change="onPickerChanage"
            data-type="supplierId"
          >
            <view v-if="pickerRef.supplierIdShow">{{ pickerRef.supplierIdShow }}</view>
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
        <view name="saleOrderStatus" class="form-item">
          <text class="label">收款状态</text>
          <picker
            @change="onPickerChanage"
            :range="SaleOrderStatusEnumList"
            range-key="text"
            class="picker"
            mode="selector"
            data-type="saleOrderStatus"
            :index="1"
          >
            <view v-if="pickerRef.saleOrderStatusShow">{{ pickerRef.saleOrderStatusShow }}</view>
            <view v-else class="placeholder">请选择收款状态</view>
          </picker>
        </view>
        <view name="freightPrice" class="form-item">
          <text class="label">运费</text>
          <input
            class="input"
            placeholder="运费：元"
            type="digit"
            :maxlength="5"
            v-model="orderForm.freightPrice"
          />
        </view>
        <view name="orderDate" class="form-item">
          <text class="label">订单日期</text>
          <picker
            @change="onPickerChanage"
            class="picker"
            mode="date"
            :start="getBusinessDefaultDateTimeRange().sDate"
            :end="getBusinessDefaultDateTimeRange().eDate"
            data-type="orderDate"
          >
            <view v-if="orderForm.orderDate">{{ orderForm.orderDate }}</view>
            <view v-else class="placeholder">不选为当天</view>
          </picker>
        </view>

        <view name="orderRemark" class="form-item">
          <text class="label">备注</text>
          <textarea
            placeholder="订单备注"
            class="input"
            :auto-height="true"
            v-model="orderForm.orderRemark"
          />
        </view>
      </view>
    </uni-forms>

    <view class="add_product">
      <button class="form-button" @click="openSelectedProduct()">添加产品</button>
    </view>

    <!-- 产品列表 -->
    <view class="cart-list" v-if="productList && productList.length > 0">
      <!-- 头部提示 -->
      <!-- <view class="tips">
        <text class="label">满减</text>
        <text class="desc">满1件, 即可享受9折优惠</text>
      </view> -->
      <!-- 滑动操作分区 -->
      <uni-swipe-action>
        <!-- 滑动操作项 -->
        <uni-swipe-action-item v-for="item in productList" :key="item.productId" class="cart-swipe">
          <!-- 商品信息 -->
          <view class="goods">
            <view class="top_row">
              <view class="meta">
                <view class="name ellipsis">{{ item.productName }}</view>
                <text class="attrsText ellipsis" v-if="item.unit && item.isLoose"
                  >单价：{{ item.unitPrice }}元/{{ item.unit }} 数量：{{ item.count
                  }}{{ item.unit }}</text
                >
                <text class="attrsText ellipsis" v-if="item.spec && !item.isLoose"
                  >单价：{{ item.unitPrice }}元/{{ item.unit }} 数量：{{ item.count
                  }}{{ item.spec }}({{ item.specCount }}{{ item.unit }})</text
                >
                <!-- <view class="attrsText ellipsis">{{ item.attrsText }}</view> -->
                <!-- <view class="price">{{ item.nowPrice }}</view> -->
                <view class="price">
                  <input
                    class="input"
                    type="digit"
                    placeholder="单价：元"
                    :maxlength="6"
                    v-model="item.unitPrice"
                  />
                </view>
              </view>
              <!-- </navigator> -->
              <!-- 商品数量 -->
              <view class="count">
                <vk-data-input-number-box v-model="item.count" :min="1" :max="99999" />
              </view>
            </view>

            <view class="goods_remark">
              <textarea v-model="item.remark" placeholder="产品备注" :maxlength="50" />
            </view>
          </view>

          <!-- 右移除除按钮 -->
          <template #right>
            <view class="cart-swipe-right">
              <button @tap="onDeleteCart(item)" class="button delete-button">移除</button>
            </view>
          </template>
        </uni-swipe-action-item>
      </uni-swipe-action>
    </view>
    <!-- 产品空状态 -->
    <view class="cart-blank" v-else>
      <image src="/static/images/blank_cart.png" class="image" />
      <text class="text">产品列表为空，请添加</text>
    </view>
    <!-- 吸底工具栏 -->
    <view
      class="toolbar"
      :style="{ paddingBottom: safeAreaInsetBottom ? safeAreaInsets?.bottom + 'px' : 0 }"
    >
      <text class="text">合计:</text>
      <text class="amount">{{ totalRef.totalPrice }}</text>
      <view class="button-grounp">
        <view
          @tap="onCreateOrder"
          class="button payment-button"
          :class="{ disabled: productList.length === 0 }"
        >
          创建订单({{ totalRef.totalCount }})
        </view>
      </view>
    </view>
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

// .zcy_content {
//   margin: 20rpx 20rpx 0;
//   padding: 0 20rpx;
//   border-radius: 10rpx;
//   background-color: #fff;
// }

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

.add_product {
  margin: 20rpx 20rpx 0;
  padding: 0 20rpx;
  border-radius: 10rpx;
  background-color: #fff;

  button {
    background-color: #fff;
    color: #000;
  }
}

// 购物车列表
.cart-list {
  padding: 0 20rpx;

  // 购物车商品
  .goods {
    display: flex;
    flex-direction: column; /* 确保容器内的元素垂直排列 */
    padding: 20rpx 20rpx 20rpx 20rpx;
    border-radius: 10rpx;
    background-color: #fff;
    position: relative;

    .navigator {
      display: flex;
    }

    .checkbox {
      position: absolute;
      top: 0;
      left: 0;

      display: flex;
      align-items: center;
      justify-content: center;
      width: 80rpx;
      height: 100%;

      &::before {
        content: '\e6cd';
        font-family: 'erabbit' !important;
        font-size: 40rpx;
        color: #444;
      }

      &.checked::before {
        content: '\e6cc';
        color: #27ba9b;
      }
    }

    .top_row {
      display: flex;
      justify-content: space-between; /* 确保 .meta 和 .count 水平排列并在两侧 */
      width: 100%;
    }

    .meta {
      flex: 1;
      display: flex;
      flex-direction: column;
      //   justify-content: space-between;
      margin-left: 20rpx;
    }

    .name {
      height: 72rpx;
      font-size: 26rpx;
      color: #444;
    }

    .attrsText {
      line-height: 1.8;
      padding: 0 15rpx;
      font-size: 24rpx;
      align-self: flex-start;
      border-radius: 4rpx;
      color: #888;
      background-color: #f7f7f8;
    }

    .price {
      line-height: 1;
      font-size: 26rpx;
      color: #444;
      margin-bottom: 2rpx;
      margin-top: 2rpx;

      input {
        color: rgb(50, 50, 51);
        font-size: 0.8125rem;
        background: rgb(242, 243, 245);
        height: 1.5625rem;
        width: 8rem;
      }

      text {
        width: 2rem;
      }

      //   input::after {
      //     content: '￥';
      //     font-size: 80%;
      //     color: #cf4444;
      //   }
    }

    // 商品数量
    .count {
      //   position: absolute;
      //   bottom: 20rpx;
      //   right: 5rpx;

      display: flex;
      justify-content: flex-end;
      align-items: center;
      width: 220rpx;
      height: 48rpx;
      margin-top: 120rpx;

      .text {
        height: 100%;
        padding: 0 20rpx;
        font-size: 32rpx;
        color: #444;
      }

      .input {
        height: 100%;
        text-align: center;
        border-radius: 4rpx;
        font-size: 24rpx;
        color: #444;
        background-color: #f6f6f6;
      }
    }

    .goods_remark {
      margin-top: 20rpx; /* 给出顶部间距，确保在 .top-row 下面 */
      display: flex;
      justify-content: flex-start;
      align-items: center;
      margin-left: 20rpx;

      textarea {
        color: rgb(50, 50, 51);
        font-size: 0.8125rem;
        background: rgb(242, 243, 245);
        height: 1.5625rem;
        width: 80rem;
      }
    }
  }

  .cart-swipe {
    display: block;
    margin: 20rpx 0;
  }

  .cart-swipe-right {
    display: flex;
    height: 100%;

    .button {
      display: flex;
      justify-content: center;
      align-items: center;
      width: 50px;
      padding: 6px;
      line-height: 1.5;
      color: #fff;
      font-size: 26rpx;
      border-radius: 0;
    }

    .delete-button {
      background-color: #cf4444;
    }
  }
}

// 空状态
.cart-blank,
.login-blank {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
  height: 60vh;
  .image {
    width: 400rpx;
    height: 281rpx;
  }
  .text {
    color: #444;
    font-size: 26rpx;
    margin: 20rpx 0;
  }
  .button {
    width: 240rpx !important;
    height: 60rpx;
    line-height: 60rpx;
    margin-top: 20rpx;
    font-size: 26rpx;
    border-radius: 60rpx;
    color: #fff;
    background-color: #27ba9b;
  }
}

// 吸底工具栏
.toolbar {
  position: fixed;
  left: 0;
  right: 0;
  bottom: calc(var(--window-bottom));
  z-index: 1;

  height: 100rpx;
  padding: 0 20rpx;
  display: flex;
  align-items: center;
  border-top: 1rpx solid #ededed;
  border-bottom: 1rpx solid #ededed;
  background-color: #fff;
  box-sizing: content-box;

  .all {
    margin-left: 25rpx;
    font-size: 14px;
    color: #444;
    display: flex;
    align-items: center;
  }

  .all::before {
    font-family: 'erabbit' !important;
    content: '\e6cd';
    font-size: 40rpx;
    margin-right: 8rpx;
  }

  .checked::before {
    content: '\e6cc';
    color: #27ba9b;
  }

  .text {
    margin-right: 8rpx;
    margin-left: 32rpx;
    color: #444;
    font-size: 14px;
  }

  .amount {
    font-size: 20px;
    color: #cf4444;

    .decimal {
      font-size: 12px;
    }

    &::before {
      content: '￥';
      font-size: 12px;
    }
  }

  .button-grounp {
    margin-left: auto;
    display: flex;
    justify-content: space-between;
    text-align: center;
    line-height: 72rpx;
    font-size: 13px;
    color: #fff;

    .button {
      width: 240rpx;
      margin: 0 10rpx;
      border-radius: 72rpx;
    }

    .payment-button {
      background-color: #27ba9b;

      &.disabled {
        opacity: 0.6;
      }
    }
  }
}
// 底部占位空盒子
.toolbar-height {
  height: 100rpx;
}
</style>
