<template>
  <n-button round size="small" @click="onExport">
    <template #icon>
      <n-icon>
        <svg
          xmlns="http://www.w3.org/2000/svg"
          xmlns:xlink="http://www.w3.org/1999/xlink"
          viewBox="0 0 32 32"
        >
          <path
            d="M28 24v4H4v-4H2v4l.008-.005A1.998 1.998 0 0 0 4 30h24a2 2 0 0 0 2-2v-4z"
            fill="currentColor"
          />
          <path
            d="M27.6 14.6L24 18.2V4h-2v14.2l-3.6-3.6L17 16l6 6l6-6l-1.4-1.4z"
            fill="currentColor"
          />
          <path d="M9 4l-6 6l1.4 1.4L8 7.8V22h2V7.8l3.6 3.6L15 10L9 4z" fill="currentColor" />
        </svg>
      </n-icon>
    </template>
    <slot name="default"></slot>
  </n-button>
</template>

<script lang="ts">
  import { defineComponent } from 'vue'
  export default defineComponent({
    name: 'DownloadButton',
    emits: ['export'],
    setup(props, { emit }) {
      function onExport() {
        emit('export')
      }
      return {
        onExport,
      }
    },
  })
</script>
