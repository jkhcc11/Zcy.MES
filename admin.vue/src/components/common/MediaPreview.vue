<template>
  <!-- <iframe :src="previewUrl" @load="setReferrerPolicy"></iframe> -->
  <n-spin :show="loading">
    <div id="xgplayer"></div>
  </n-spin>
</template>

<script lang="ts">
  import { defineComponent, onMounted, toRef, ref, onUnmounted, watch } from 'vue'
  import Player from 'xgplayer'
  import HlsPlugin from 'xgplayer-hls'
  import 'xgplayer/dist/index.min.css'
  export default defineComponent({
    name: 'MediaPreview',
    props: {
      previewUrl: {
        type: String,
        default: '',
      },
      loading: Boolean,
    },
    setup(props) {
      const playerRef = ref<any>(null)
      const playUrl = toRef(props, 'previewUrl')
      const poster = ''

      function getPlayer() {
        return playerRef.value
      }

      function destroyPlayer() {
        playerRef.value.destroy() // 销毁播放器
        playerRef.value = null // 将实例引用置空
      }

      watch(
        () => props.previewUrl,
        (newVal: string) => {
          if (newVal) {
            const isTs = newVal.indexOf('.m3u8') != -1
            let plugin = [] as any[]
            if (isTs) {
              plugin = [HlsPlugin]
            }

            playerRef.value = new Player({
              id: 'xgplayer',
              url: playUrl.value,
              poster,
              playbackRate: [0.5, 0.75, 1, 1.5, 2],
              defaultPlaybackRate: 1,
              height: 250,
              plugins: plugin,
              autoplay: false,
            })
            // playerRef.value?.playNext({
            //   url: newVal,
            // })
            // playerRef.value?.reload()
          }
        }
      )

      onMounted(() => {
        const meta = document.createElement('meta')
        meta.name = 'referrer'
        meta.content = 'no-referrer'
        document.getElementsByTagName('head')[0].appendChild(meta)
      })

      return {
        getPlayer,
        destroyPlayer,
      }
    },
  })
</script>
