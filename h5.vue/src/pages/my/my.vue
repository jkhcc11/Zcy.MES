<script setup lang="ts">
import { useUserInfoStore } from '@/stores'

const useUserInfo = useUserInfoStore()
// 退出登录
const onLogout = () => {
  // 模态弹窗
  uni.showModal({
    content: '是否退出登录？',
    confirmColor: '#27BA9B',
    success: (res) => {
      if (res.confirm) {
        // 清理用户信息
        useUserInfo.clearProfile()
        // 返回上一页
        uni.navigateBack()
      }
    },
  })
}
</script>

<template>
  <view class="viewport">
    <view class="list" v-if="useUserInfo.userInfo">
      <view class="item"> 昵称：{{ useUserInfo.userInfo.userNick }} </view>
    </view>
    <!-- 列表1 -->
    <view class="list" v-if="useUserInfo.userInfo">
      <navigator url="/user-center/modify-pwd" hover-class="none" class="item arrow">
        修改密码
      </navigator>
    </view>
    <!-- #ifdef MP-WEIXIN -->
    <!-- 列表2 -->
    <view class="list">
      <button hover-class="none" class="item arrow" open-type="openSetting">授权管理</button>
      <button hover-class="none" class="item arrow" open-type="feedback">问题反馈</button>
      <button hover-class="none" class="item arrow" open-type="contact">联系我们</button>
    </view>
    <!-- #endif -->
    <!-- 列表3 -->
    <view class="list">
      <button hover-class="none" class="item arrow">关于ZcyMES</button>
    </view>
    <!-- 操作按钮 -->
    <view class="action" v-if="useUserInfo.userInfo">
      <view @tap="onLogout" class="button">退出登录</view>
    </view>
    <view class="list" v-else>
      <navigator url="/pages/login/login" hover-class="none" class="item arrow"> 去登陆 </navigator>
    </view>
  </view>
</template>

<style lang="scss">
page {
  background-color: #f4f4f4;
}

.viewport {
  padding: 20rpx;
}

/* 列表 */
.list {
  padding: 0 20rpx;
  background-color: #fff;
  margin-bottom: 20rpx;
  border-radius: 10rpx;
  .item {
    line-height: 90rpx;
    padding-left: 10rpx;
    font-size: 30rpx;
    color: #333;
    border-top: 1rpx solid #ddd;
    position: relative;
    text-align: left;
    border-radius: 0;
    background-color: #fff;
    &::after {
      width: auto;
      height: auto;
      left: auto;
      border: none;
    }
    &:first-child {
      border: none;
    }
    &::after {
      right: 5rpx;
    }
  }
  .arrow::after {
    content: '\e6c2';
    position: absolute;
    top: 50%;
    color: #ccc;
    font-family: 'erabbit' !important;
    font-size: 32rpx;
    transform: translateY(-50%);
  }
}

/* 操作按钮 */
.action {
  text-align: center;
  line-height: 90rpx;
  margin-top: 40rpx;
  font-size: 32rpx;
  color: #333;
  .button {
    background-color: #fff;
    margin-bottom: 20rpx;
    border-radius: 10rpx;
  }
}
</style>
