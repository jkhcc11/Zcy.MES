<script setup lang="ts">
import {
  PublicStatusEnumList,
  ClientTypeEnumList,
  CommonPageSize,
  ClientTypeEnum,
  PublicStatusEnum,
} from '@/services/constants'
import {
  queryPageSupplierClientApi,
  banSupplierClientApi,
  openSupplierClientApi,
} from '@/services/supplierClient'
import { onMounted, ref } from 'vue'
import { onUnload } from '@dcloudio/uni-app'
import DaDropdown from '@/components/da-dropdown/index.vue'

// 获取屏幕边界到安全区域距离
const { safeAreaInsets } = uni.getSystemInfoSync()

// 请求参数
const queryParams: Required<any> = {
  page: 1,
  pageSize: CommonPageSize,
  keyWord: '',
}

// #ifdef MP-WEIXIN
onUnload(() => {
  //因为小程序无法监听onBackPress 控制返回，目前监听卸载 然后直接跳转到首页
  uni.switchTab({ url: '/pages/new-index/index' })
})
// #endif

//客户|供应商list
const dataList = ref<any[]>([])
// 是否加载中标记，用于防止滚动触底触发多次请求
const isLoading = ref(false)
const queryData = async () => {
  // 如果数据出于加载中，退出函数
  if (isLoading.value) return
  // 退出分页判断
  if (isFinish.value === true) {
    return uni.showToast({ icon: 'none', title: '没有更多数据~' })
  }

  // 发送请求前，标记为加载中
  isLoading.value = true
  // 发送请求
  const res = await queryPageSupplierClientApi(queryParams)
  // 发送请求后，重置标记
  isLoading.value = false

  if (res.isSuccess) {
    // 数组追加
    dataList.value.push(...res.data.items)

    const totalPages = res.data.total / CommonPageSize
    //   console.log('totalPages', totalPages)
    // 分页条件
    if (queryParams.page < totalPages) {
      // 页码累加
      queryParams.page++
    } else {
      // 分页已结束
      isFinish.value = true
    }
  } else {
    uni.showToast({ title: res.msg, icon: 'exception' })
  }
}

//枚举显示
const getClientTypeItem = (enumItem: ClientTypeEnum) => {
  return ClientTypeEnumList.filter((item) => item.value == enumItem)[0]
}
const getStatusItem = (enumItem: PublicStatusEnum) => {
  return PublicStatusEnumList.filter((item) => item.value == enumItem)[0]
}

// 是否分页结束
const isFinish = ref(false)
// 是否触发下拉刷新
const isTriggered = ref(false)
// 自定义下拉刷新被触发
const onRefresherrefresh = async () => {
  // 开始动画
  isTriggered.value = true
  // 重置数据
  queryParams.page = 1
  dataList.value = []
  isFinish.value = false
  // 加载数据
  await queryData()
  // 关闭动画
  isTriggered.value = false
}

//禁用
function onBan(item: any) {
  uni.showModal({
    content: `是否禁用【${item.clientName}】?`,
    confirmColor: '#ff9240',
    success: async (res) => {
      if (res.confirm) {
        const res = await banSupplierClientApi(item.id)
        if (res.isSuccess) {
          uni.showToast({ icon: 'success', title: res.msg })
          await onRefresherrefresh()
          return
        }

        uni.showToast({ icon: 'none', title: res.msg })
      }
    },
  })
}
//启用
function onOpen(item: any) {
  uni.showModal({
    content: `是否启用【${item.clientName}】?`,
    confirmColor: '#18bc37',
    success: async (res) => {
      if (res.confirm) {
        const res = await openSupplierClientApi(item.id)
        if (res.isSuccess) {
          uni.showToast({ icon: 'success', title: res.msg })
          await onRefresherrefresh()
          return
        }

        uni.showToast({ icon: 'none', title: res.msg })
      }
    },
  })
}

//创建
function onCreate() {
  uni.navigateTo({ url: `supplier-client-create` })
}

//筛选
const filterMenuList = ref([
  {
    type: 'search',
    prop: 'keyWord',
    placeholder: '关键字',
  },
])

//筛选确认
async function onFilterConfirm(v: any, selectedValue: any) {
  //v 当前变更的{employeeId: '2'}
  //selectedValue 所有的变更的{keyWord: undefined, employeeId: '2'}

  //遍历赋值
  Object.keys(selectedValue).forEach((key) => {
    queryParams[key] = selectedValue[key] ?? ''
  })

  await onRefresherrefresh()
  //console.log('handleConfirm ==>', v, selectedValue)
}

onMounted(() => {
  queryData()
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
      <view class="top_total">
        <uni-row class="demo-uni-row">
          <uni-col :span="24">
            <view class="top_create">
              <button type="primary" :plain="true" @click="onCreate()">创建</button>
            </view>
          </uni-col>
        </uni-row>
      </view>
    </view>

    <scroll-view
      enable-back-to-top
      scroll-y
      class="orders"
      refresher-enabled
      :refresher-triggered="isTriggered"
      @refresherrefresh="onRefresherrefresh"
      @scrolltolower="queryData"
    >
      <view class="card" v-for="dataItem in dataList" :key="dataItem.id">
        <!-- 订单信息 -->
        <view class="status">
          <!-- <text>{{ dataItem.clientName }}</text> -->
          <uni-tag
            :text="getStatusItem(dataItem.clientStatus).text"
            :type="getStatusItem(dataItem.clientStatus).tagType"
            :inverted="true"
            size="small"
          />
        </view>
        <view class="meta">
          <view class="name ellipsis" v-if="dataItem.clientName"> {{ dataItem.clientName }} </view>
          <!-- <view class="type" v-if="order.recordDate">记录日期：{{ order.recordDate }}</view> -->
        </view>
        <view class="payment">
          <view class="payment_left">
            <text class="quantity" v-if="dataItem.remark">备注：{{ dataItem.remark }}</text>
          </view>
          <view class="payment_right">
            <uni-tag
              :text="getClientTypeItem(dataItem.clientType).text"
              :type="getClientTypeItem(dataItem.clientType).tagType"
              :inverted="true"
              size="small"
            />
          </view>
        </view>

        <!-- 订单操作按钮 -->
        <view class="action">
          <view class="action-left"> </view>
          <view class="action-right">
            <view
              class="button warning"
              @tap="onBan(dataItem)"
              v-if="dataItem.clientStatus === PublicStatusEnum.正常"
            >
              禁用
            </view>
            <view
              class="button success"
              @tap="onOpen(dataItem)"
              v-if="dataItem.clientStatus === PublicStatusEnum.禁用"
            >
              启用
            </view>
          </view>
        </view>
      </view>
      <!-- 底部提示文字 -->
      <view class="loading-text" :style="{ paddingBottom: safeAreaInsets?.bottom + 'px' }">
        {{ isFinish ? '没有更多数据~' : '正在加载...' }}
      </view>
    </scroll-view>
  </view>
</template>

<style lang="scss">
page {
  background-color: #f7f7f7;
  height: 100%;
  overflow: hidden;
}

.viewport {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.top_filter {
  .top_create {
    background-color: #fff;
    padding: 14rpx;
  }
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

    .button {
      width: 80rpx;
      height: 40rpx;
      display: flex;
      justify-content: center;
      align-items: center;
      margin-left: 20rpx;
      border-radius: 60rpx;
      border: 1rpx solid #ccc;
      font-size: 20rpx;
      color: #444;
    }

    .warning {
      color: #fff;
      background-color: #ff9240;
      border-color: #ff9240;
    }

    .error {
      color: #fff;
      background-color: #e43d33;
      border-color: #e43d33;
    }

    .success {
      color: #fff;
      background-color: #18bc37;
      border-color: #18bc37;
    }
  }

  .loading-text {
    text-align: center;
    font-size: 28rpx;
    color: #666;
    padding: 20rpx 0;
  }
}
</style>
