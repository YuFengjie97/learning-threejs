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
      },
      {
        path: '03-spot-light',
        component: ()=>import('@/views/chapter3/03-spot-light.vue')
      },
      {
        path: '04-directional-light',
        component: ()=>import('@/views/chapter3/04-directional-light.vue')
      },
      {
        path: '05-hemisphere-light',
        component: ()=>import('@/views/chapter3/05-hemisphere-light.vue')
      },
      {
        path: '06-area-light',
        component: ()=>import('@/views/chapter3/06-area-light.vue')
      },
      {
        path: '07-lensflares',
        component: ()=>import('@/views/chapter3/07-lensflares.vue')
      },
    ]
  },
  {
    path: '/chapter4/',
    children: [
      {
        path: '01-basic-mesh-material',
        component: () => import('@/views/chapter4/01-basic-mesh-material.vue')
      },
      {
        path: '02-depth-material',
        component: () => import('@/views/chapter4/02-depth-material.vue')
      },
      {
        path: '03-combined-material',
        component: () => import('@/views/chapter4/03-combined-material.vue')
      },
      {
        path: '04-mesh-normal-material',
        component: () => import('@/views/chapter4/04-mesh-normal-material.vue')
      },
      {
        path: '05-mesh-face-material',
        component: () => import('@/views/chapter4/05-mesh-face-material.vue')
      },
      {
        path: '06-mesh-lambert-material',
        component: () => import('@/views/chapter4/06-mesh-lambert-material.vue')
      },
      {
        path: '07-mesh-phong-material',
        component: () => import('@/views/chapter4/07-mesh-phong-material.vue')
      },
      {
        path: '08-shader-material',
        component: () => import('@/views/chapter4/08-shader-material.vue')
      },
    ]
  },
  {
    path: '/chapter5/',
    children: [
      {
        path: '01-basic-2d-geometries-plane',
        component: () => import('@/views/chapter5/01-basic-2d-geometries-plane.vue')
      },
      {
        path: '02-basic-2d-geometries-circle',
        component: () => import('@/views/chapter5/02-basic-2d-geometries-circle.vue')
      },
      {
        path: '03-basic-2d-geometries-shape',
        component: () => import('@/views/chapter5/03-basic-2d-geometries-shape.vue')
      },
      {
        path: '04-basic-3d-geometries-cube',
        component: () => import('@/views/chapter5/04-basic-3d-geometries-cube.vue')
      },
      {
        path: '05-basic-3d-geometries-sphere',
        component: () => import('@/views/chapter5/05-basic-3d-geometries-sphere.vue')
      },
      {
        path: '06-basic-3d-geometries-cylinder',
        component: () => import('@/views/chapter5/06-basic-3d-geometries-cylinder.vue')
      },
      {
        path: '07-basic-3d-geometries-torus',
        component: () => import('@/views/chapter5/07-basic-3d-geometries-torus.vue')
      },
    ]
  },
  { path: '/:path(.*)', component: NotFound },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes,
})

export default router
