<template>
  <div class="main-container">
    <n-grid x-gap="12" :cols="2">
      <n-gi :span="2">
        关键字：
        <n-input v-model:value="useVodSearch.keyWord" type="text" placeholder="资源关键字" />
      </n-gi>
      <n-gi :span="2">
        <n-card title="录资源" style="margin-bottom: 16px">
          <n-tabs type="line" animated>
            <n-tab-pane name="find" tab="1、找资源">
              <n-space vertical>
                <n-button
                  quaternary
                  tag="a"
                  :href="'https://pan666.net/?q=' + useVodSearch.keyWord"
                  target="_blank"
                  text
                  type="primary"
                  rel="noreferrer"
                >
                  阿里云盘居多 优先级1
                </n-button>
                <n-button
                  quaternary
                  tag="a"
                  :href="'https://www.alipansou.com/search?k=' + useVodSearch.keyWord"
                  target="_blank"
                  text
                  type="primary"
                  rel="noreferrer"
                >
                  混合云盘资源 优先级2
                </n-button>
                <n-button
                  target="_blank"
                  type="primary"
                  rel="noreferrer"
                  text
                  quaternary
                  tag="a"
                  :href="
                    'https://www.rrdynb.com/plus/search.php?q=' +
                    useVodSearch.keyWord +
                    '&pagesize=10'
                  "
                >
                  稍微冷门点的用这个 优先级3
                </n-button>
                <n-button
                  target="_blank"
                  type="primary"
                  rel="noreferrer"
                  text
                  quaternary
                  tag="a"
                  :href="'https://t-rex.tzfile.com/?s=' + useVodSearch.keyWord + '&type=post'"
                >
                  稍微热门一点用这个 优先级4
                </n-button>
                <n-button
                  target="_blank"
                  type="primary"
                  rel="noreferrer"
                  text
                  quaternary
                  tag="a"
                  :href="
                    'https://yyets.click/search?keyword=' + useVodSearch.keyWord + '&type=default'
                  "
                >
                  欧美剧人人资源居多 非网盘 优先级5
                </n-button>
              </n-space>
            </n-tab-pane>
            <n-tab-pane name="comfirm" tab="2、确认资源">
              <n-card class="ml-2 mr-2 tip-wrapper" :content-style="{ padding: 0 }">
                <div class="text-lg text-bold">合格说明</div>
                <abbr style="color: red; font-weight: bold">必须无广告，字幕组广告除外。</abbr
                ><br />
                <n-button
                  target="_blank"
                  type="info"
                  rel="noreferrer"
                  text
                  quaternary
                  tag="a"
                  href="https://docs.qq.com/doc/DQWl0Q3ZxRFhRVmdT"
                >
                  一些例子
                </n-button>
                <br />
                云盘优先、高清优先，有字幕优先，无广告优先（无明显广告赌博信息等）国产剧普通话优先，海外对应国家语言，港剧优先粤语，有国内各大平台的Logo的资源不要。
                <div class="margin-tb">电影</div>
                1、预览时无广告！！！！！ <br />
                2、最好不要有Logo(优爱腾) <br />
                3、资源大小最好低于<abbr style="color: red; font-weight: bold">3G</abbr>
                <div class="margin-tb">电视剧</div>
                1、优先字幕组资源 <br />
                2、预览时无广告！！！！！ <br />
                3、最好不要有Logo(优爱腾)<br />
                4、每集资源大小最好低于<abbr style="color: red; font-weight: bold">1.5G</abbr>
              </n-card>
            </n-tab-pane>
            <n-tab-pane name="createTask" tab="3、创建任务">
              <div class="text-lg text-bold">阿里云盘资源</div>
              暂时人工记录
              <div class="text-lg text-bold">非阿里云盘资源</div>
              <n-image src="/static/images/create-task.png" />
            </n-tab-pane>
            <n-tab-pane name="waitAudit" tab="4、等待审核"> 等待接单做完后，审核 </n-tab-pane>
            <n-tab-pane name="createVod" tab="5、录入系统"> 录入 </n-tab-pane>
          </n-tabs>
        </n-card>
      </n-gi>
    </n-grid>
  </div>
</template>

<script lang="ts">
  import useVodSearchStore from '@/store/modules/vod-search'
  import { defineComponent } from 'vue'

  export default defineComponent({
    name: 'SearchVod',
    setup() {
      const useVodSearch = useVodSearchStore()
      return {
        useVodSearch,
      }
    },
  })
</script>
