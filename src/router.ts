import { createRouter, createWebHashHistory } from 'vue-router'
import { RouteRecordRaw } from 'vue-router'
import NotFound from './views/NotFound.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    component: () => import('@/views/Home.vue'),
  },
  {
    path: '/chapter1/',
    children: [
      {
        path: '01-basic-skeleton',
        component: () => import('@/views/chapter1/01-basic-skeleton.vue'),
      },
      {
        path: '02-first-scene',
        component: ()=>import('@/views/chapter1/02-first-scene.vue')
      },
      {
        path: '03-materials-light',
        component: ()=>import('@/views/chapter1/03-materials-light.vue')
      },
      {
        path: '04-materials-light-animation',
        component: ()=>import('@/views/chapter1/04-materials-light-animation.vue')
      }
    ],
  },
  {
    path: '/chapter2/',
    children: [
      {
        path: '01-basic-scene',
        component: ()=>import('@/views/chapter2/01-basic-scene.vue')
      },
      {
        path: '02-foggy-scene',
        component: ()=>import('@/views/chapter2/02-foggy-scene.vue')
      },
      {
        path: '03-forced-materials',
        component: ()=>import('@/views/chapter2/03-forced-materials.vue')
      },
      {
        path: '05-custom-geometry',
        component: ()=>import('@/views/chapter2/05-custom-geometry.vue')
      }
      
      
    ]
  },
  { path: '/:path(.*)', component: NotFound },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes,
})

export default router
