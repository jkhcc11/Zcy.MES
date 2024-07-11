<script setup lang="ts">
import { CommonPageSize, PublicStatusEnum, PublicStatusEnumList } from '@/services/constants'
import { useEmployeeStore } from '@/stores/modules/employeeStore'
import {
  queryPageReportWorkForAdminApi,
  approvedReportWorkApi,
  rejectReportWorkApi,
  getReportWorkTotalsApi,
} from '@/services/production'
import { onMounted, reactive, ref } from 'vue'
import { getBusinessDefaultDateTimeRange } from '@/utils/index'
import ReportWorkItem from './components/ReportWorkItem.vue'
import DaDropdown from '@/components/da-dropdown/index.vue'

const useEmployee = useEmployeeStore()
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
  employeeId: '',
  productProcessId: '',
  reportWorkStatus: '',
  startTime: getBusinessDefaultDateTimeRange(30).sDate,
  endTime: getBusinessDefaultDateTimeRange(30).eDate,
}

//报工记录list
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
  const res = await queryPageReportWorkForAdminApi(queryParams)
  const resTotal = await getReportWorkTotalsApi(queryParams)
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

//驳回
function onReject(item: any) {
  uni.showModal({
    content: `是否驳回【${item.employeeNickName}】->${item.reportWorkDate}->${item.productCraftName}?`,
    confirmColor: '#ff9240',
    success: async (res) => {
      if (res.confirm) {
        const res = await rejectReportWorkApi(item.id)
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
//通过
function onApproved(item: any) {
  uni.showModal({
    content: `是否通过【${item.employeeNickName}】->${item.reportWorkDate}->${item.productCraftName}?`,
    confirmColor: '#18bc37',
    success: async (res) => {
      if (res.confirm) {
        const res = await approvedReportWorkApi(item.id)
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
    title: '员工',
    type: 'cell',
    prop: 'employeeId',
    showAll: true,
    showIcon: true,
    // value: '2', // 默认内容2
    //options: [],
    syncDataFn: useEmployee.getFilterArray,
  },
  // {
  //   title: '单点',
  //   type: 'click',
  //   prop: 'god2',
  //   // value: true, // 默认选中
  // },
  // {
  //   title: '排序',
  //   type: 'sort',
  //   prop: 'god3',
  //   // value: 'asc', // 默认升序
  // },
  // {
  //   title: '筛选',
  //   type: 'filter',
  //   prop: 'god4',
  //   // 默认选中单选2、多选2、3、滑动30
  //   // value: { ft1: '2', ft2: ['2', '3'], ft3: 30 },
  //   options: [
  //     {
  //       title: '单选',
  //       type: 'radio',
  //       prop: 'ft1',
  //       options: [
  //         { label: '单选1', value: '1' },
  //         { label: '单选2', value: '2' },
  //         { label: '单选3', value: '3' },
  //         { label: '单选4', value: '4' },
  //       ],
  //     },
  //     {
  //       title: '多选',
  //       type: 'checkbox',
  //       prop: 'ft2',
  //       options: [
  //         { label: '多选1', value: '1' },
  //         { label: '多选2', value: '2' },
  //         { label: '多选3', value: '3' },
  //         { label: '多选4', value: '4' },
  //         { label: '多选5', value: '5' },
  //       ],
  //     },
  //     {
  //       title: '滑块',
  //       type: 'slider',
  //       prop: 'ft3',
  //       componentProps: {
  //         min: 0,
  //         max: 100,
  //         step: 1,
  //         showValue: true,
  //       },
  //     },
  //   ],
  // },
  // {
  //   title: '工序',
  //   type: 'picker',
  //   prop: 'productProcessId',
  //   showAll: true,
  //   showIcon: true,
  //   field: {
  //     label: 'label',
  //     value: 'value',
  //     children: 'children',
  //   },
  //   // value: ['2', '2-2'], // 默认选中 级联X22
  //   options: [
  //     {
  //       label: '级联列表项1',
  //       value: '1',
  //       children: [
  //         {
  //           label: '级联列表项1-1',
  //           value: '11111111111111',
  //         },
  //         {
  //           label: '级联列表项1-2',
  //           value: '222222222222222222',
  //         },
  //         {
  //           label: '级联列表项1-3',
  //           value: '3333333333333333333',
  //         },
  //       ],
  //     },
  //     {
  //       label: '级联列表项2',
  //       value: '2',
  //       children: [
  //         {
  //           label: '级联列表项2-1',
  //           value: '2-1',
  //         },
  //         {
  //           label: '级联列表项2-2',
  //           value: '2-2',
  //         },
  //         {
  //           label: '级联列表项2-3',
  //           value: '2-3',
  //         },
  //       ],
  //     },
  //     {
  //       label: '级联列表项3',
  //       value: '3',
  //       children: [
  //         {
  //           label: '级联列表项3-1',
  //           value: '3-1',
  //         },
  //         {
  //           label: '级联列表项3-2',
  //           value: '3-2',
  //         },
  //         {
  //           label: '级联列表项3-3',
  //           value: '3-3',
  //         },
  //       ],
  //     },
  //   ],
  // },
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
          <uni-col :span="12">
            <uni-section
              class="mb-10"
              title="计时"
              :sub-title="`${totalRef.timingWordDuration ?? '-'} 小时`"
              type="line"
            ></uni-section>
          </uni-col>
          <uni-col :span="12">
            <uni-section
              class="mb-10"
              title="计件"
              :sub-title="`${totalRef.countingWordDuration ?? '-'} 个`"
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
      <ReportWorkItem v-for="dataItem in dataList" :workReportItem="dataItem">
        <template #actionRight>
          <view
            class="button warning"
            @tap="onReject(dataItem)"
            v-if="dataItem.reportWorkStatus === PublicStatusEnum.待处理"
          >
            驳回
          </view>
          <view
            class="button success"
            @tap="onApproved(dataItem)"
            v-if="dataItem.reportWorkStatus === PublicStatusEnum.待处理"
          >
            通过
          </view>
        </template>
      </ReportWorkItem>

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

.button {
  width: 100rpx;
  height: 40rpx;
  line-height: 40rpx;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-left: 20rpx;
  border-radius: 5rpx;
  border: 1rpx solid #ccc;
  font-size: 20rpx;
  color: #444;
  display: inline;
  padding: 10rpx;
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

.loading-text {
  text-align: center;
  font-size: 28rpx;
  color: #666;
  padding: 20rpx 0;
}
</style>
