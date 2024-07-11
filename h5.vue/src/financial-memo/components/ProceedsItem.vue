<script setup lang="ts">
import { AccountTypeEnumList, CommonPageSize, AccountTypeEnum } from '@/services/constants'
import { queryPageProceedsRecordApi, getProceedsRecordTotalsApi } from '@/services/financial-memo'
import type {
  QueryPageProceedsRecordInput,
  QueryPageProceedsRecordDto,
} from '@/types/financial-memo'
import { onMounted, ref, reactive } from 'vue'
import { getBusinessDefaultDateTimeRange } from '@/utils/index'
import DaDropdown from '@/components/da-dropdown/index.vue'

// 获取屏幕边界到安全区域距离
const { safeAreaInsets } = uni.getSystemInfoSync()
//统计
const totalRef = reactive({
  //总额
  totalMoney: null,
})

// 请求参数
const queryParams: Required<QueryPageProceedsRecordInput> = {
  page: 1,
  pageSize: CommonPageSize,
  keyWord: '',
  startTime: getBusinessDefaultDateTimeRange(30).sDate,
  endTime: getBusinessDefaultDateTimeRange(30).eDate,
}

//收款list
const dataList = ref<QueryPageProceedsRecordDto[]>([])
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
  const res = await queryPageProceedsRecordApi(queryParams)
  const resTotal = await getProceedsRecordTotalsApi(queryParams)

  // 发送请求后，重置标记
  isLoading.value = false

  if (res.isSuccess) {
    //汇总赋值
    totalRef.totalMoney = resTotal.data.totalMoney

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

onMounted(() => {
  queryData()
})

//账号类型item
const getAccountTypeItem = (accountType: AccountTypeEnum) => {
  return AccountTypeEnumList.filter((item) => item.value == accountType)[0]
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

//筛选
const filterMenuList = ref([
  {
    type: 'search',
    prop: 'keyWord',
    placeholder: '款项名、备注等关键字',
  },
  {
    title: '日期',
    type: 'daterange',
    prop: 'timeRange',
    showQuick: true,
    value: {
      start: getBusinessDefaultDateTimeRange(30).sDate,
      end: getBusinessDefaultDateTimeRange(30).eDate,
    },
  },
])

//筛选确认
async function onFilterConfirm(v: any, selectedValue: any) {
  //v 当前变更的{employeeId: '2'}
  //selectedValue 所有的变更的{keyWord: undefined, employeeId: '2'}

  //遍历赋值
  Object.keys(selectedValue).forEach((key) => {
    if (key === 'timeRange') {
      if (selectedValue.timeRange) {
        queryParams.startTime = selectedValue.timeRange.start
        queryParams.endTime = selectedValue.timeRange.end
      } else {
        queryParams.startTime = ''
        queryParams.endTime = ''
      }
    } else {
      queryParams[key] = selectedValue[key] ?? ''
    }
  })

  await onRefresherrefresh()
  //console.log('handleConfirm ==>', v, selectedValue)
}
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
            <uni-section
              class="mb-10"
              title="总额"
              :sub-title="`${totalRef.totalMoney ?? '-'} 元`"
              type="line"
            ></uni-section>
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
      <view class="card" v-for="order in dataList" :key="order.id">
        <!-- 订单信息 -->
        <view class="status">
          <text>创建时间：{{ order.createdTime }} </text>
          <uni-tag
            :text="getAccountTypeItem(order.accountType).text"
            :type="getAccountTypeItem(order.accountType).tagType"
            :inverted="true"
            size="small"
          />
        </view>
        <view class="meta">
          <view class="name ellipsis" v-if="order.proceedsRecordName">
            {{ order.proceedsRecordName }}
          </view>
        </view>
        <view class="payment">
          <view class="payment_left">
            <text class="quantity" v-if="order.remark">备注：{{ order.remark }}</text>
          </view>
          <view class="payment_right">
            <!-- <text class="quantity" v-if="order.managerUser">经办人：{{ order.managerUser }}</text> -->
            <!-- <text></text> -->
            <text class="amount">金额 <text class="symbol">¥</text>{{ order.money }}</text>
          </view>
        </view>

        <!-- 订单操作按钮 -->
        <view class="action">
          <view class="action-left"> 收款日期：{{ order.recordDate }} </view>
          <view class="action-right" v-if="order.supplierClientName">
            客户：{{ order.supplierClientName }}
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
.viewport {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.top_total {
  background: #fff;
  text-align: center;
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
  }

  .loading-text {
    text-align: center;
    font-size: 28rpx;
    color: #666;
    padding: 20rpx 0;
  }
}
</style>
