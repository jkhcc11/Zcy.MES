<template>
  <view class="da-dropdown-filter">
    <view class="da-dropdown-filter-box" v-for="(item, index) in filterList" :key="index">
      <view class="da-dropdown-filter--title">{{ item.title }}</view>
      <view class="da-dropdown-filter-content">
        <!-- 单选类型 -->
        <block v-if="item.type === 'radio'">
          <view
            v-for="(opt, optIdx) in item.options"
            class="da-dropdown-filter-item da-dropdown-tag"
            :class="item.value === opt.value ? 'is-actived' : ''"
            :key="optIdx"
            @click="handleRadioChange(item, opt, optIdx, index)">
            <text class="da-dropdown-tag--text">{{ opt.label }}</text>
          </view>
        </block>
        <!-- 多选类型 -->
        <block v-else-if="item.type === 'checkbox'">
          <view
            v-for="(opt, optIdx) in item.options"
            class="da-dropdown-filter-item da-dropdown-tag"
            :class="opt.isActived ? 'is-actived' : ''"
            :key="optIdx"
            @click="handleCheckboxChange(item, opt, optIdx, index)">
            <text class="da-dropdown-tag--text">{{ opt.label }}</text>
          </view>
        </block>
        <!-- 滑块类型 -->
        <block v-else-if="item.type === 'slider'">
          <slider
            style="width: 100%"
            :value="item.value"
            :min="item.componentProps.min || 0"
            :max="item.componentProps.max || 100"
            :step="item.componentProps.step || 1"
            :activeColor="item.componentProps.activeColor"
            :show-value="item.componentProps.showValue"
            @change="(e) => handleSliderChange(e, item, index)" />
        </block>
      </view>
    </view>
    <PartDropdownFooter
      :resetText="dropdownItem.resetText"
      :confirmText="dropdownItem.confirmText"
      @reset="handleReset()"
      @confirm="handleConfirm()"></PartDropdownFooter>
  </view>
</template>

<script>
import { defineComponent, ref, watch } from 'vue'
import { deepClone } from '../utils'
import PartDropdownFooter from './part-dropdown-footer.vue'

export default defineComponent({
  components: { PartDropdownFooter },
  props: {
    dropdownItem: {
      type: Object,
      default: null,
    },
    dropdownIndex: {
      type: Number,
    },
  },
  emits: ['success', 'change'],
  setup(props, { emit }) {
    const filterList = ref(null)

    function initData(dropdownItem, clearValue = false) {
      const { options = [], value = {} } = dropdownItem
      if (options?.length) {
        const list = deepClone(options)
        for (let i = 0; i < list.length; i++) {
          const k = list[i]
          if (clearValue !== true && (value[k.prop] || value[k.prop] === 0)) {
            k.value = value[k.prop]
          }

          // 多选
          if (k.type === 'checkbox' && k.value?.length) {
            if (k.options?.length) {
              k.options.forEach((x) => {
                x.isActived = k.value?.includes(x.value)
              })
            }
          }
        }
        filterList.value = list
      } else {
        filterList.value = []
      }
    }

    function handleRadioChange(item, opt, _optIdx, _index) {
      item.value = opt.value
    }
    function handleCheckboxChange(item, opt, _optIdx, _index) {
      if (opt.isActived) {
        opt.isActived = false
        if (item.value?.length) {
          const idx = item.value.findIndex((k) => k === opt.value)
          item.value.splice(idx, 1)
        } else {
          item.value = []
        }
      } else {
        if (item.value?.length) {
          item.value.push(opt.value)
        } else {
          item.value = [opt.value]
        }
        opt.isActived = true
      }
    }
    function handleSliderChange(event, item, _index) {
      const v = event.detail.value
      item.value = v
    }
    function handleReset() {
      initData(props.dropdownItem || [], true)
    }
    function handleConfirm() {
      const list = deepClone(filterList.value)

      if (props.dropdownItem?.prop) {
        const obj = {}
        for (let i = 0; i < list.length; i++) {
          const k = list[i]
          if (k.value || k.value === 0) {
            obj[k.prop] = k.value
          }
        }
        const res = { [props.dropdownItem.prop]: obj }
        emit('success', res, obj, props.dropdownIndex)
      } else {
        console.error(`菜单项${props.dropdownItem.title}未定义prop，返回内容失败`)
      }
    }

    watch(
      () => props.dropdownItem,
      (v) => {
        initData(v || null)
      },
      { immediate: true },
    )

    return {
      filterList,
      handleRadioChange,
      handleCheckboxChange,
      handleSliderChange,
      handleReset,
      handleConfirm,
    }
  },
})
</script>

<style lang="scss" scoped>
// 多条件筛选
.da-dropdown-filter {
  &-box {
    padding: 0 24rpx;
    line-height: 1;
  }

  &--title {
    flex-shrink: 0;
    padding: 20rpx 0;
    font-size: 26rpx;
    color: var(--dropdown-text-color);
    white-space: nowrap;
  }

  &-content {
    display: flex;
    flex-wrap: wrap;
    justify-content: flex-start;
  }
}

.da-dropdown-tag {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20rpx 40rpx;
  margin-right: 20rpx;
  margin-bottom: 20rpx;
  overflow: hidden;
  font-size: 28rpx;
  color: var(--dropdown-text-color);
  background-color: #f5f5f5;
  border-radius: 999rpx;

  &--text {
    position: relative;
    z-index: 1;
  }

  &.is-actived {
    color: var(--dropdown-theme-color);
    background-color: #fff;

    &::after {
      position: absolute;
      top: 0;
      right: 0;
      bottom: 0;
      left: 0;
      z-index: 0;
      content: '';
      background-color: var(--dropdown-theme-color);
      opacity: 0.05;
    }
  }
}
</style>
