<script setup lang="ts">
import { CommonPageSize, PublicStatusEnumList } from '@/services/constants'
import { queryPageReportWorkForNormalApi, getMeReportWorkTotalsApi } from '@/services/production'
import { onMounted, ref, reactive } from 'vue'
import { getBusinessDefaultDateTimeRange } from '@/utils/index'
import ReportWorkItem from './components/ReportWorkItem.vue'
import DaDropdown from '@/components/da-dropdown/index.vue'

import { onUnload } from '@dcloudio/uni-app'
// #ifdef MP-WEIXIN
onUnload(() => {
  //因为小程序无法监听onBackPress 控制返回，目前监听卸载 然后直接跳转到首页
  uni.switchTab({ url: '/pages/new-index/index' })
})
// #endif

// 获取屏幕边界到安全区域距离
const { safeAreaInsets } = uni.getSystemInfoSync()

//统计
const totalRef = reactive({
  //计件总工作量
  timingWordDuration: null,
  //计件总个数
  countingWordDuration: null,
})
// 请求参数
const queryParams: Required<any> = {
  page: 1,
  pageSize: CommonPageSize,
  keyWord: '',
  reportWorkStatus: '',
  startTime: getBusinessDefaultDateTimeRange(30).sDate,
  endTime: getBusinessDefaultDateTimeRange(30).eDate,
}

//我的报工记录list
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
  const res = await queryPageReportWorkForNormalApi(queryParams)
  const resTotal = await getMeReportWorkTotalsApi(queryParams)
  // 发送请求后，重置标记
  isLoading.value = false
  if (res.isSuccess) {
    //汇总赋值
    totalRef.countingWordDuration = resTotal.data.countingWordDuration
    totalRef.timingWordDuration = resTotal.data.timingWordDuration

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

//创建
function onCreate() {
  uni.navigateTo({ url: `report-work-create` })
}

//筛选
const filterMenuList = ref([
  {
    type: 'search',
    prop: 'keyWord',
    placeholder: '产品名、工艺名 等关键字',
  },
  {
    title: '状态',
    type: 'cell',
    prop: 'reportWorkStatus',
    showAll: true,
    showIcon: true,
    // value: '2', // 默认内容2
    field: { label: 'text', value: 'value', suffix: 'suffix' },
    options: PublicStatusEnumList,
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
    } else if (key === 'productProcessId') {
      if (selectedValue.productProcessId) {
        queryParams.productProcessId = selectedValue.productProcessId[1]
      } else {
        queryParams.productProcessId = ''
      }
    } else {
      queryParams[key] = selectedValue[key] ?? ''
    }
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
          <uni-col :span="8">
            <uni-section
              class="mb-10"
              title="计时"
              :sub-title="`${totalRef.timingWordDuration ?? '-'} 小时`"
              type="line"
            ></uni-section>
          </uni-col>
          <uni-col :span="8">
            <uni-section
              class="mb-10"
              title="计件"
              :sub-title="`${totalRef.countingWordDuration ?? '-'} 个`"
              type="line"
            ></uni-section>
          </uni-col>
          <uni-col :span="8">
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
      <ReportWorkItem v-for="dataItem in dataList" :workReportItem="dataItem"> </ReportWorkItem>

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

.top_create {
  background-color: #fff;
  padding: 14rpx;
}

.viewport {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.loading-text {
  text-align: center;
  font-size: 28rpx;
  color: #666;
  padding: 20rpx 0;
}
</style>
