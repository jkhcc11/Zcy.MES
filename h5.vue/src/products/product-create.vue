<script setup lang="ts">
import { ProductTypeEnum, ProductTypeEnumList } from '@/services/constants'
import { createUpdateProductApi } from '@/services/product'
import { useUserInfoStore } from '@/stores'
import { useProductTypeStore } from '@/stores/modules/productType'
import { onMounted, reactive } from 'vue'

// 是否适配底部安全区域
defineProps<{
  safeAreaInsetBottom?: boolean
}>()

const useUserInfo = useUserInfoStore()
const useProductType = useProductTypeStore()

onMounted(async () => {
  await useProductType.init()
})

//提交保存
const onSubmit = async () => {
  if (!useUserInfo.userInfo) {
    return uni.showToast({
      icon: 'none',
      title: '请先登录',
    })
  }

  try {
    // 表单校验
    if (!submitForm.productTypeId) {
      uni.showToast({ icon: 'error', title: '请选择产品分类' })
      return
    }

    if (!submitForm.productName) {
      uni.showToast({ icon: 'error', title: '请输入产品名' })
      return
    }

    if (!submitForm.productType) {
      uni.showToast({ icon: 'error', title: '请选择类型' })
      return
    }

    if (submitForm.isLoose) {
      if (!submitForm.unit) {
        uni.showToast({ icon: 'error', title: '散件请输入单位' })
        return
      }
    } else {
      if (!submitForm.spec) {
        uni.showToast({ icon: 'error', title: '非散件请输入规格' })
        return
      }

      if (!submitForm.specCount || submitForm.specCount <= 0) {
        uni.showToast({ icon: 'error', title: '非散件请输入规格数' })
        return
      }
    }

    uni.showLoading({
      title: '提交中...',
      mask: true,
    })

    const res = await createUpdateProductApi(submitForm)
    uni.hideLoading()
    if (res.isSuccess === false) {
      uni.showToast({ icon: 'none', title: res.msg })
      return
    }

    // 成功提示
    uni.showToast({ icon: 'success', title: res.msg })
    // 返回上一页
    setTimeout(() => {
      uni.navigateTo({ url: 'product-index' })
      //   uni.navigateBack()
    }, 400)
  } catch (error: any) {
    uni.hideLoading()
    uni.showToast({ icon: 'error', title: `系统异常,${error.statusCode}` })
  }
}

//表单数据
const submitForm = reactive<any>({
  productTypeId: null,
  productName: null,
  productType: ProductTypeEnum.销售产品,

  //是否散件 散件 -> 没规格  非散件 -> 有规格
  isLoose: true,
  //单位 kg/个
  unit: null,
  //规格 箱/盒/袋
  spec: null,
  //规格数
  specCount: null,
  productRemark: null,
})

//picker显示
const pickerRef = reactive({
  productTypeIdShow: '',
  productTypeShow: '销售产品',
})

//统一picker变更
const onPickerChanage = (e: any) => {
  //selector 是index date是值
  const currentValue = e.detail.value
  if (e.target.dataset.type) {
    switch (e.target.dataset.type) {
      case 'productType': {
        submitForm['productType'] = ProductTypeEnumList[currentValue].value
        pickerRef.productTypeShow = ProductTypeEnumList[currentValue].text
        break
      }
      case 'productTypeId': {
        const currentItem = useProductType.getItem(currentValue)
        submitForm['productTypeId'] = currentItem.id
        pickerRef.productTypeIdShow = currentItem.typeName
        break
      }
    }
  }
}

//是否散件
const onIsLooseChange: UniHelper.SwitchOnChange = (ev) => {
  submitForm.isLoose = ev.detail.value
}
</script>

<template>
  <scroll-view enable-back-to-top scroll-y class="scroll-view">
    <!-- 订单基础数据 -->
    <uni-forms :model="submitForm" calss="form">
      <view class="form-content">
        <!-- 表单内容 -->
        <view class="form-item">
          <text class="label">产品分类</text>
          <picker
            class="picker"
            mode="selector"
            :range="useProductType.valiedProductTypeArray"
            range-key="typeName"
            @change="onPickerChanage"
            data-type="productTypeId"
            :index="2"
          >
            <view v-if="pickerRef.productTypeIdShow">{{ pickerRef.productTypeIdShow }}</view>
            <view v-else class="placeholder">请选择分类</view>
          </picker>
        </view>
        <view class="form-item">
          <text class="label">产品类型</text>
          <picker
            class="picker"
            mode="selector"
            :range="ProductTypeEnumList"
            range-key="text"
            @change="onPickerChanage"
            data-type="productType"
            :index="2"
          >
            <view v-if="pickerRef.productTypeShow">{{ pickerRef.productTypeShow }}</view>
            <view v-else class="placeholder">请选择类型</view>
          </picker>
        </view>

        <view class="form-item">
          <text class="label">产品名</text>
          <input
            class="input"
            placeholder="产品名"
            :maxlength="20"
            v-model="submitForm.productName"
          />
        </view>

        <!-- 规格 -->
        <view class="form-item">
          <text class="label">是否散件</text>
          <switch :checked="submitForm.isLoose" @change="onIsLooseChange" />
        </view>
        <view class="form-item">
          <text class="label">单位</text>
          <input class="input" placeholder="kg/个" :maxlength="5" v-model="submitForm.unit" />
        </view>
        <view class="form-item" v-if="!submitForm.isLoose">
          <text class="label">规格</text>
          <input class="input" placeholder="箱/盒/袋" :maxlength="5" v-model="submitForm.spec" />
        </view>
        <view class="form-item" v-if="!submitForm.isLoose">
          <text class="label">规格数</text>
          <input
            class="input"
            placeholder="规格数数量"
            :maxlength="5"
            type="number"
            v-model="submitForm.specCount"
          />
        </view>

        <view class="form-item">
          <text class="label">备注</text>
          <textarea
            placeholder="备注"
            class="input"
            :auto-height="true"
            v-model="submitForm.productRemark"
          />
        </view>
      </view>
    </uni-forms>

    <!-- 提交按钮 -->
    <button @tap="onSubmit" class="button">保存</button>
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
