import { LAYOUT } from '@/store/keys'

export const asyncRoutes = [
  {
    path: '/index',
    component: LAYOUT,
    name: 'Index',
    meta: {
      title: 'Dashboard',
      iconPrefix: 'iconfont',
      icon: 'dashboard',
    },
    children: [
      {
        path: 'home',
        name: 'Home',
        component: () => import('@/views/index/main.vue'),
        meta: {
          title: '主控台',
          affix: true,
          cacheable: true,
          iconPrefix: 'iconfont',
          icon: 'menu',
        },
      },
      {
        path: 'work-place',
        name: 'WorkPlace',
        component: () => import('@/views/index/work-place.vue'),
        meta: {
          title: '工作台',
          affix: true,
          iconPrefix: 'iconfont',
          icon: 'menu',
        },
      },
      {
        path: 'work-place-vod',
        name: 'WorkPlaceVod',
        component: () => import('@/views/index/work-place-vod.vue'),
        meta: {
          title: '工作台',
          affix: true,
          iconPrefix: 'iconfont',
          icon: 'menu',
        },
      },
      {
        path: 'reg-index',
        name: 'RegIndex',
        component: () => import('@/views/index/reg-index.vue'),
        meta: {
          title: '提交注册',
          affix: true,
          iconPrefix: 'iconfont',
          icon: 'menu',
        },
      },
    ],
  },
]
