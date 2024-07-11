<script setup lang="ts">
import { ProductTypeEnum, BillingTypeEnum } from '@/services/constants'
import { useProductPickerStore } from '@/stores/modules/productPicker'
import { onMounted, reactive } from 'vue'

//pickerRef会全部通过onSelected回调
const emit = defineEmits(['onSelected'])
const useProductPicker = useProductPickerStore()
// // 定义 props 接收
// defineProps<{
//   list: BannerItem[]
// }>()

//picker显示
const pickerRef = reactive({
  //显示文案
  showText: null as string | null,
  //产品Id
  productId: null as string | null,
  //工序Id
  productProcessId: null as string | null,
  //工艺计费类型
  billingType: null as BillingTypeEnum | null,
  pickerArray: [[] as any[], [] as any[]],
})

onMounted(async () => {
  //初始化默认一级和二级
  await useProductPicker.init(ProductTypeEnum.加工产品)
  pickerRef.pickerArray[0] = useProductPicker.currentProducts.map((item) => {
    return {
      showName: item.productName,
    }
  })

  await useProductPicker.getProductProcesses(useProductPicker.currentProducts[0].productId)
  pickerRef.pickerArray[1] = useProductPicker.currentProductProcesses.map((item) => {
    return {
      showName: item.craftName,
    }
  })
})

//值确定的
const onPickerChanage = (e: any) => {
  //selector 是index date是值  [0, 1]
  const currentValue = e.detail.value
  const currentProductItem = useProductPicker.getProductItem(currentValue[0])
  const currentProductProcessesItem = useProductPicker.getProductProcessesItem(currentValue[1])
  pickerRef.showText = `${currentProductItem.productName}/${currentProductProcessesItem.craftName}`
  pickerRef.productId = currentProductItem.productId
  pickerRef.productProcessId = currentProductProcessesItem.productProcessId
  pickerRef.billingType = currentProductProcessesItem.billingType

  emit('onSelected', pickerRef)
}

//picker列变更
const onPickerColumnChanage = async (e: any) => {
  //e.detail.column  第几列变动
  //e.detail.value  选中index
  //产品列值变化
  if (e.detail.column == 0) {
    const productItem = useProductPicker.getProductItem(e.detail.value)
    // console.log('colum-productItem', productItem)
    if (productItem) {
      await useProductPicker.getProductProcesses(productItem.productId)
      pickerRef.pickerArray[1] = useProductPicker.currentProductProcesses.map((item) => {
        return {
          showName: item.craftName,
        }
      })
    }
    // console.log('colum-pickerRef', pickerRef)
  }
  //   console.log('colum--e', e)
}
</script>

<template>
  <picker
    class="picker"
    mode="multiSelector"
    :range="pickerRef.pickerArray"
    range-key="showName"
    @change="onPickerChanage"
    @columnchange="onPickerColumnChanage"
    data-type="productTypeId"
  >
    <view v-if="pickerRef.showText">{{ pickerRef.showText }}</view>
    <view v-else class="placeholder">请选择工序</view>
  </picker>
</template>
