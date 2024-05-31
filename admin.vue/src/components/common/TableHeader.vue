<template>
  <div id="tableHeaderContainer" class="relative">
    <n-card title="筛选" v-if="showFilter" size="small">
      <template #header-extra>
        <n-space>
          <n-button
            strong
            secondary
            circle
            :type="isExpansion ? 'info' : 'tertiary'"
            size="small"
            @click="onExpansion"
          >
            <template #icon>
              <n-icon>
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  xmlns:xlink="http://www.w3.org/1999/xlink"
                  viewBox="0 0 512 512"
                >
                  <path
                    d="M464 428L339.92 303.9a160.48 160.48 0 0 0 30.72-94.58C370.64 120.37 298.27 48 209.32 48S48 120.37 48 209.32s72.37 161.32 161.32 161.32a160.48 160.48 0 0 0 94.58-30.72L428 464zM209.32 319.69a110.38 110.38 0 1 1 110.37-110.37a110.5 110.5 0 0 1-110.37 110.37z"
                    fill="currentColor"
                  />
                </svg>
              </n-icon>
            </template>
          </n-button>
          <n-button strong secondary type="warning" size="small" @click="doResetSearch">
            <template #icon>
              <n-icon>
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  xmlns:xlink="http://www.w3.org/1999/xlink"
                  viewBox="0 0 32 32"
                >
                  <path
                    d="M18 28A12 12 0 1 0 6 16v6.2l-3.6-3.6L1 20l6 6l6-6l-1.4-1.4L8 22.2V16a10 10 0 1 1 10 10z"
                    fill="currentColor"
                  />
                </svg>
              </n-icon>
            </template>
            重置</n-button
          >
          <n-button strong secondary type="primary" size="small" @click="doSearch">
            <template #icon>
              <n-icon>
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  xmlns:xlink="http://www.w3.org/1999/xlink"
                  viewBox="0 0 512 512"
                >
                  <path
                    d="M256 80a176 176 0 1 0 176 176A176 176 0 0 0 256 80z"
                    fill="none"
                    stroke="currentColor"
                    stroke-miterlimit="10"
                    stroke-width="32"
                  />
                  <path
                    d="M232 160a72 72 0 1 0 72 72a72 72 0 0 0-72-72z"
                    fill="none"
                    stroke="currentColor"
                    stroke-miterlimit="10"
                    stroke-width="32"
                  />
                  <path
                    fill="none"
                    stroke="currentColor"
                    stroke-linecap="round"
                    stroke-miterlimit="10"
                    stroke-width="32"
                    d="M283.64 283.64L336 336"
                  />
                </svg>
              </n-icon>
            </template>
            搜索</n-button
          >
          <slot name="table-config"></slot>
        </n-space>
      </template>

      <div v-show="isExpansion">
        <slot name="search-content"></slot>
      </div>
    </n-card>

    <n-space>
      <slot name="top-toolbar"></slot>
    </n-space>
  </div>
  <!-- <n-card
      :title="title"
      :content-style="{ padding: '0px' }"
      :bordered="false"
      header-style="font-size: 16px; padding: 5px 5px; border-radius: 0"
    >
      <template #header-extra>
        <n-space>
          <slot name="table-config"></slot>
          <n-tooltip class="ml-2 mr-2" trigger="hover" v-if="showFilter">
            <template #trigger>
              <n-button type="warning" size="small" @click="showSearchContent = !showSearchContent">
                <template #icon>
                  <n-icon>
                    <FilterIcon />
                  </n-icon>
                </template>
                筛选条件
              </n-button>
            </template>
            展开筛选条件
          </n-tooltip>
        </n-space>
      </template>
    </n-card>
    <n-space item-style="margin-bottom:8px;">
      <slot name="top-toolbar"></slot>
    </n-space>
  </div>
  <n-drawer
    v-model:show="showSearchContent"
    placement="top"
    :to="target"
    :height="searchContentHeight"
  >
    <n-drawer-content
      body-content-style="overflow-y: scroll"
      title="搜索条件"
      closable
      header-style="font-size: 16px; padding: 15px"
    >
      <template #default>
        <slot name="search-content"></slot>
      </template>
      <template #footer>
        <div class="flex justify-end">
          <n-space>
            <n-button type="warning" size="small" @click="doResetSearch">重置</n-button>
            <n-button type="primary" size="small" @click="doSearch">搜索</n-button>
          </n-space>
        </div>
      </template>
    </n-drawer-content>
  </n-drawer> -->
</template>

<script lang="ts">
  import { defineComponent, nextTick, onMounted, ref, toRef } from 'vue'

  export default defineComponent({
    name: 'TableHeader',
    props: {
      title: {
        type: String,
        default: '查询条件', //todo:这里要调整 筛选到右边
      },
      showFilter: {
        type: Boolean,
        default: true,
      },
      searchContentHeight: {
        type: String,
        default: '300px',
      },
      /**
       * 是否展开搜索
       */
      isExpansion: {
        type: Boolean,
        default: false,
      },
    },
    emits: ['search', 'reset-search'],
    setup(props, { emit }) {
      const isExpansionRef = toRef(props, 'isExpansion')

      const isExpansion = ref(isExpansionRef.value)

      const showSearchContent = ref(false)
      const target = ref<HTMLElement | null>(null)
      onMounted(() => {
        nextTick(() => {
          target.value = document.getElementById('tableHeaderContainer')
        })
      })
      function doSearch() {
        showSearchContent.value = false
        emit('search')
      }
      function doResetSearch() {
        emit('reset-search')
      }

      function onExpansion() {
        isExpansion.value = !isExpansion.value
      }

      return {
        showSearchContent,
        target,
        isExpansion,
        doSearch,
        doResetSearch,
        onExpansion,
      }
    },
  })
</script>
