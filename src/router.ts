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
      },
      {
        path: '06-mesh-properties',
        component: ()=>import('@/views/chapter2/06-mesh-properties.vue')
      },
      {
        path: '07-both-cameras',
        component: ()=>import('@/views/chapter2/07-both-cameras.vue')
      },
      {
        path: '08-cameras-lookat',
        component: ()=>import('@/views/chapter2/08-cameras-lookat.vue')
      }
    ]
  },
  {
    path: '/chapter3/',
    children: [
      {
        path: '01-ambient-light',
        component: ()=>import('@/views/chapter3/01-ambient-light.vue')
      },
      {
        path: '02-point-light',
        component: ()=>import('@/views/chapter3/02-point-light.vue')
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
