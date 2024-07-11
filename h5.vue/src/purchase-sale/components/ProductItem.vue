<script setup lang="ts">
import { ProductTypeEnumList, ProductTypeEnum } from '@/services/constants'
import { queryValidProductAPI } from '@/services/product'
import type { QueryValidProductDto, QueryValidProductInput } from '@/types/product'
import { onMounted, reactive, ref } from 'vue'
import DaDropdown from '@/components/da-dropdown/index.vue'

// 获取屏幕边界到安全区域距离
const { safeAreaInsets } = uni.getSystemInfoSync()

// 定义 emits
const emit = defineEmits(['onChecked'])

// 定义 porps
const props = defineProps<{
  productType?: string | null
}>()

// 请求参数
const queryParams: Required<QueryValidProductInput> = {
  keyWord: '',
  productType: props.productType ?? '',
  productTypeId: '',
}

// 产品列表
const productList = ref<QueryValidProductDto[]>([])
// 是否加载中标记，用于防止滚动触底触发多次请求
const isLoading = ref(false)
const querSaleOrderData = async () => {
  // 如果数据出于加载中，退出函数
  if (isLoading.value) return

  // 发送请求前，标记为加载中
  isLoading.value = true
  // 发送请求
  const res = await queryValidProductAPI(queryParams)
  // 发送请求后，重置标记
  isLoading.value = false
  if (res.isSuccess) {
    productList.value = res.data
  } else {
    uni.showToast({ title: res.msg, icon: 'exception' })
  }
}

//产品类型item
const getProductTypeItem = (productType: ProductTypeEnum) => {
  return ProductTypeEnumList.filter((item) => item.value == productType)[0]
}

// 是否触发下拉刷新
const isTriggered = ref(false)
// 自定义下拉刷新被触发
const onRefresherrefresh = async () => {
  // 开始动画
  isTriggered.value = true
  productList.value = []
  checkedInfos.checkedKeys.length = 0
  checkedInfos.checkedRows.length = 0
  // 加载数据
  await querSaleOrderData()
  // 关闭动画
  isTriggered.value = false
}

//选中值
const checkedInfos = reactive({
  checkedRows: [] as any[],
  checkedKeys: [] as any[],
})
function onRowChange(newKey: any) {
  //这里小程序传递对象不生效，只能传递id 然后再通过id获取items
  checkedInfos.checkedKeys.length = 0
  checkedInfos.checkedRows.length = 0
  const checkedKeys = newKey.detail.value
  const checkedRows = productList.value.filter((item: any) => checkedKeys.includes(item.id))
  // console.log('onRowChange', newKey)
  checkedInfos.checkedRows.push(...checkedRows)
  checkedInfos.checkedKeys.push(...checkedKeys)
  //console.log('onRowChangeed', checkedInfos)
}
function isChecked(key: any) {
  return checkedInfos.checkedKeys.findIndex((e) => e == key) != -1
}

//确认
function onConfim() {
  if (checkedInfos.checkedKeys.length == 0) {
    uni.showToast({ icon: 'none', title: '请至少选择一个产品' })
    return
  }

  emit('onChecked', checkedInfos)
}

//筛选
const filterMenuList = ref([
  {
    type: 'search',
    prop: 'keyWord',
    placeholder: '产品关键字',
  },
  {
    title: '类型',
    type: 'cell',
    prop: 'productType',
    showAll: true,
    showIcon: true,
    value: props.productType, // 默认
    field: { label: 'text', value: 'value', suffix: 'suffix' },
    options: ProductTypeEnumList,
  },
])

//筛选确认
async function onFilterConfirm(v: any, selectedValue: any) {
  //v 当前变更的{employeeId: '2'}
  //selectedValue 所有的变更的{keyWord: undefined, employeeId: '2'}

  //遍历赋值
  Object.keys(selectedValue).forEach((key) => {
    if (key == 'keyWord' || key == 'productType') {
      queryParams[key] = selectedValue[key] ?? ''
    }
  })

  await onRefresherrefresh()
  //console.log('handleConfirm ==>', v, selectedValue)
}

onMounted(() => {
  querSaleOrderData()
})
</script>

<template>
  <view class="viewport">
    <view class="top_filter">
      <view>
        <DaDropdown
          v-model:dropdownMenu="filterMenuList"
          fixedTop
          :fixedTopValue="10"
          @confirm="onFilterConfirm"
        >
        </DaDropdown>
      </view>
    </view>

    <scroll-view
      enable-back-to-top
      scroll-y
      class="orders"
      refresher-enabled
      :refresher-triggered="isTriggered"
      @refresherrefresh="onRefresherrefresh"
      @scrolltolower="querSaleOrderData"
    >
      <checkbox-group @change="onRowChange">
        <view class="card" v-for="productItem in productList" :key="productItem.id">
          <view>
            <checkbox :value="productItem.id" :checked="isChecked(productItem.id)" />
          </view>
          <!-- 产品信息 -->
          <view class="meta">
            <text>{{ productItem.productName }}</text>
          </view>

          <view class="payment">
            <view class="payment_left">
              <uni-tag
                :text="getProductTypeItem(productItem.productType).text"
                :type="getProductTypeItem(productItem.productType).tagType"
                :inverted="true"
                size="small"
              />
            </view>
            <view class="payment_right">
              <text class="quantity" v-if="productItem.unit && productItem.isLoose"
                >单位：{{ productItem.unit }}</text
              >
              <text class="quantity" v-if="productItem.spec && !productItem.isLoose"
                >单位：{{ productItem.specCount }} x {{ productItem.unit }} /{{
                  productItem.spec
                }}</text
              >
            </view>
          </view>
        </view>
        <!-- 底部提示文字 -->
        <view
          class="loading-text"
          :style="{ paddingBottom: (safeAreaInsets?.bottom ?? 0) + 65 + 'px' }"
        >
          {{ isLoading ? '正在加载' : '没有更多数据~' }}
        </view>
      </checkbox-group>
    </scroll-view>

    <!-- 吸底工具栏 -->
    <view class="toolbar" :style="{ paddingBottom: (safeAreaInsets?.bottom ?? 0) + 5 + 'px' }">
      <text class="text">合计:</text>
      <text class="amount">{{ checkedInfos.checkedKeys.length }}个商品</text>
      <view class="button-grounp">
        <view class="button payment-button" @tap="onConfim"> 确认选择 </view>
      </view>
    </view>
  </view>
</template>

<style lang="scss">
.viewport {
  height: 100%;
  display: flex;
  flex-direction: column;
}

// 订单列表
.orders {
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

  .loading-text {
    text-align: center;
    font-size: 28rpx;
    color: #666;
    padding: 20rpx 0;
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
</style>
