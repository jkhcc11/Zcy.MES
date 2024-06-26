<template>
  <div class="main-container">
    <n-card title="工作台" :content-style="{ padding: '10px' }" :header-style="{ padding: '10px' }">
      <n-grid :cols="4" :y-gap="15" item-responsive responsive="screen">
        <n-grid-item class="flex" span="4 s:2 m:2 l:2 xl:2 2xl:2">
          <div class="flex flex-col justify-around ml-3.5 flex-1">
            <div class="text-lg">ZCYMES Admin</div>

            <div class="text-sm text-gray-500"
              >Beta版本,有问题及时反馈 <br />
              <a
                href="https://docs.qq.com/doc/DQU9BbG11d2N3ZEND"
                style="color: blue; font-size: larger; font-weight: bold"
                target="_blank"
                >ZCYMES教程</a
              ></div
            >
          </div>
        </n-grid-item>

        <n-grid-item class="flex justify-end" span="4 s:2 m:2 l:2 xl:2 2xl:2">
          <div class="flex flex-col justify-around align-end item-action">
            <div class="text-gray">当前日期</div>
            <div class="text-xl">{{ currentDate }}</div>
          </div>
        </n-grid-item>
      </n-grid>
    </n-card>
  </div>
</template>

<script lang="ts">
  import ProjectItem from './components/ProjectItem.vue'
  import TodoItem from './components/TodoItem.vue'
  import { computed, defineComponent } from 'vue'
  import { DeviceType } from '@/store/types'
  import useAppConfigStore from '@/store/modules/app-config'
  const date = new Date()
  export default defineComponent({
    name: 'WorkPlaceVod',
    components: {
      ProjectItem,
      TodoItem,
    },
    setup() {
      const appConfigStore = useAppConfigStore()
      const isMobileScreen = computed(() => {
        return appConfigStore.deviceType === DeviceType.MOBILE
      })

      return {
        isMobileScreen,
        currentDate: date.getFullYear() + '/' + (date.getMonth() + 1) + '/' + date.getDate(),
      }
    },
  })
</script>

<style lang="scss" scoped>
  .avatar-wrapper {
    width: 3rem;
    height: 3rem;
    max-width: 3rem;
    max-height: 3rem;
    min-width: 3rem;
    min-height: 3rem;
    & > img {
      width: 100%;
      height: 100%;
      border-radius: 50%;
      border: 2px solid var(--primary-color);
    }
  }
  .item-action {
    position: relative;
    padding: 0 30px;
  }
  .item-action::after {
    position: absolute;
    top: 20%;
    right: 0;
    height: 60%;
    content: '';
    display: block;
    width: 1px;
    background-color: #e0e0e0;
  }
  div.item-action:last-child::after {
    width: 0;
  }
  .fast-item-wrapper {
    border-right: 1px solid #f7f7f7;
    border-bottom: 1px solid #f7f7f7;
    height: 80px;
  }
  .fast-item-wrapper:hover {
    cursor: pointer;
    box-shadow: 0px 0px 10px #ddd;
  }
  .el-link + .el-link {
    margin-bottom: 10px;
  }
</style>
