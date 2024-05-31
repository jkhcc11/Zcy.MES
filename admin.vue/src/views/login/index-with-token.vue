<template>
  <div class="main-container"> 加载中，请稍等 </div>
</template>

<script lang="ts">
  import useUserStore from '@/store/modules/user'
  import { UserState } from '@/store/types'
  import { defineComponent } from 'vue'
  import { useRoute, useRouter } from 'vue-router'
  export default defineComponent({
    name: 'LoginWithToken',
    setup() {
      const route = useRoute()
      const router = useRouter()
      const userStore = useUserStore()
      //route.query.token
      userStore
        .saveToken({
          accessToken: route.query.token,
          refreshToken: '',
        } as UserState)
        .then(() => {
          //保存用户信息
          //userStore.saveUser()

          router.replace({
            path: route.query.redirect ? (route.query.redirect as string) : '/',
          })
        })

      console.log(route.query.token, route.params)
      return {}
    },
  })
</script>
