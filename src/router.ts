import { createRouter, createWebHashHistory } from 'vue-router'
import { RouteRecordRaw } from 'vue-router'
import NotFound from './views/NotFound.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    component: () => import('@/views/Home.vue'),
  },
  {
    path: '/shader/',
    children: [
      {
        path: '',
        component: () => import('@/views/shader/index.vue'),
      },
      {
        path: '1',
        component: () => import('@/views/shader1/index.vue'),
      },
      {
        path: '2',
        component: () => import('@/views/shader2/index.vue'),
      },
      {
        path: 'mouse',
        component: () => import('@/views/shaderMouse/index.vue'),
      },
      {
        path: 'color',
        component: () => import('@/views/shaderColor/index.vue'),
      }
    ],
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
        component: () => import('@/views/chapter1/02-first-scene.vue'),
      },
      {
        path: '03-materials-light',
        component: () => import('@/views/chapter1/03-materials-light.vue'),
      },
      {
        path: '04-materials-light-animation',
        component: () =>
          import('@/views/chapter1/04-materials-light-animation.vue'),
      },
    ],
  },
  {
    path: '/chapter2/',
    children: [
      {
        path: '01-basic-scene',
        component: () => import('@/views/chapter2/01-basic-scene.vue'),
      },
      {
        path: '02-foggy-scene',
        component: () => import('@/views/chapter2/02-foggy-scene.vue'),
      },
      {
        path: '03-forced-materials',
        component: () => import('@/views/chapter2/03-forced-materials.vue'),
      },
      {
        path: '05-custom-geometry',
        component: () => import('@/views/chapter2/05-custom-geometry.vue'),
      },
      {
        path: '06-mesh-properties',
        component: () => import('@/views/chapter2/06-mesh-properties.vue'),
      },
      {
        path: '07-both-cameras',
        component: () => import('@/views/chapter2/07-both-cameras.vue'),
      },
      {
        path: '08-cameras-lookat',
        component: () => import('@/views/chapter2/08-cameras-lookat.vue'),
      },
    ],
  },
  {
    path: '/chapter3/',
    children: [
      {
        path: '01-ambient-light',
        component: () => import('@/views/chapter3/01-ambient-light.vue'),
      },
      {
        path: '02-point-light',
        component: () => import('@/views/chapter3/02-point-light.vue'),
      },
      {
        path: '03-spot-light',
        component: () => import('@/views/chapter3/03-spot-light.vue'),
      },
      {
        path: '04-directional-light',
        component: () => import('@/views/chapter3/04-directional-light.vue'),
      },
      {
        path: '05-hemisphere-light',
        component: () => import('@/views/chapter3/05-hemisphere-light.vue'),
      },
      {
        path: '06-area-light',
        component: () => import('@/views/chapter3/06-area-light.vue'),
      },
      {
        path: '07-lensflares',
        component: () => import('@/views/chapter3/07-lensflares.vue'),
      },
    ],
  },
  {
    path: '/chapter4/',
    children: [
      {
        path: '01-basic-mesh-material',
        component: () => import('@/views/chapter4/01-basic-mesh-material.vue'),
      },
      {
        path: '02-depth-material',
        component: () => import('@/views/chapter4/02-depth-material.vue'),
      },
      {
        path: '03-combined-material',
        component: () => import('@/views/chapter4/03-combined-material.vue'),
      },
      {
        path: '04-mesh-normal-material',
        component: () => import('@/views/chapter4/04-mesh-normal-material.vue'),
      },
      {
        path: '05-mesh-face-material',
        component: () => import('@/views/chapter4/05-mesh-face-material.vue'),
      },
      {
        path: '06-mesh-lambert-material',
        component: () =>
          import('@/views/chapter4/06-mesh-lambert-material.vue'),
      },
      {
        path: '07-mesh-phong-material',
        component: () => import('@/views/chapter4/07-mesh-phong-material.vue'),
      },
      {
        path: '08-shader-material',
        component: () => import('@/views/chapter4/08-shader-material.vue'),
      },
    ],
  },
  {
    path: '/chapter5/',
    children: [
      {
        path: '01-basic-2d-geometries-plane',
        component: () =>
          import('@/views/chapter5/01-basic-2d-geometries-plane.vue'),
      },
      {
        path: '02-basic-2d-geometries-circle',
        component: () =>
          import('@/views/chapter5/02-basic-2d-geometries-circle.vue'),
      },
      {
        path: '03-basic-2d-geometries-shape',
        component: () =>
          import('@/views/chapter5/03-basic-2d-geometries-shape.vue'),
      },
      {
        path: '04-basic-3d-geometries-cube',
        component: () =>
          import('@/views/chapter5/04-basic-3d-geometries-cube.vue'),
      },
      {
        path: '05-basic-3d-geometries-sphere',
        component: () =>
          import('@/views/chapter5/05-basic-3d-geometries-sphere.vue'),
      },
      {
        path: '06-basic-3d-geometries-cylinder',
        component: () =>
          import('@/views/chapter5/06-basic-3d-geometries-cylinder.vue'),
      },
      {
        path: '07-basic-3d-geometries-torus',
        component: () =>
          import('@/views/chapter5/07-basic-3d-geometries-torus.vue'),
      },
      {
        path: '08-basic-3d-geometries-ring',
        component: () =>
          import('@/views/chapter5/08-basic-3d-geometries-ring.vue'),
      },
      {
        path: '09-basic-3d-geometries-torus-knot',
        component: () =>
          import('@/views/chapter5/09-basic-3d-geometries-torus-knot.vue'),
      },
      {
        path: '010-basic-3d-geometries-polyhedron',
        component: () =>
          import('@/views/chapter5/010-basic-3d-geometries-polyhedron.vue'),
      },
    ],
  },
  {
    path: '/chapter6/',
    children: [
      {
        path: '01-advanced-3d-geometries-convex',
        component: () =>
          import('@/views/chapter6/01-advanced-3d-geometries-convex.vue'),
      },
      {
        path: '02-advanced-3d-geometries-lathe',
        component: () =>
          import('@/views/chapter6/02-advanced-3d-geometries-lathe.vue'),
      },
      {
        path: '02-advanced-3d-geometries-lathe-2',
        component: () =>
          import('@/views/chapter6/02-advanced-3d-geometries-lathe-2.vue'),
      },
      {
        path: '03-extrude-geometry',
        component: () => import('@/views/chapter6/03-extrude-geometry.vue'),
      },
      {
        path: '04-extrude-tube',
        component: () => import('@/views/chapter6/04-extrude-tube.vue'),
      },
      {
        path: '05-extrude-svg',
        component: () => import('@/views/chapter6/05-extrude-svg.vue'),
      },
      {
        path: '06-parametric-geometries',
        component: () =>
          import('@/views/chapter6/06-parametric-geometries.vue'),
      },
      {
        path: '07-text-geometry',
        component: () => import('@/views/chapter6/07-text-geometry.vue'),
      },
      {
        path: '08-binary-operations',
        component: () => import('@/views/chapter6/08-binary-operations.vue'),
      },
    ],
  },
  {
    path: '/chapter7/',
    children: [
      {
        path: '01-particles',
        component: () => import('@/views/chapter7/01-particles.vue'),
      },
      {
        path: '02-particles-webgl',
        component: () => import('@/views/chapter7/02-particles-webgl.vue'),
      },
      {
        path: '03-basic-point-cloud',
        component: () => import('@/views/chapter7/03-basic-point-cloud.vue'),
      },
      {
        path: '05a-program-based-point-cloud-webgl',
        component: () =>
          import('@/views/chapter7/05a-program-based-point-cloud-webgl.vue'),
      },
      {
        path: '05b-program-based-sprites-webgl',
        component: () =>
          import('@/views/chapter7/05b-program-based-sprites-webgl.vue'),
      },
      {
        path: '06-rainy-scene',
        component: () => import('@/views/chapter7/06-rainy-scene.vue'),
      },
      {
        path: '07-snowy-scene',
        component: () => import('@/views/chapter7/07-snowy-scene.vue'),
      },
      {
        path: '08-sprites',
        component: () => import('@/views/chapter7/08-sprites.vue'),
      },
      {
        path: '09-sprites-3D',
        component: () => import('@/views/chapter7/09-sprites-3D.vue'),
      },
      {
        path: '10-create-particle-system-from-model',
        component: () =>
          import('@/views/chapter7/10-create-particle-system-from-model.vue'),
      },
    ],
  },
  {
    path: '/chapter8/',
    children: [
      {
        path: '01-grouping',
        component: () => import('@/views/chapter8/01-grouping.vue'),
      },
      {
        path: '03-load-save-json-object',
        component: () =>
          import('@/views/chapter8/03-load-save-json-object.vue'),
      },
      {
        path: '04-load-save-json-scene',
        component: () => import('@/views/chapter8/04-load-save-json-scene.vue'),
      },
      {
        path: '05-blender-from-json',
        component: () => import('@/views/chapter8/05-blender-from-json.vue'),
      },
      {
        path: '06-load-obj',
        component: () => import('@/views/chapter8/06-load-obj.vue'),
      },
      {
        path: '07-load-obj-mtl',
        component: () => import('@/views/chapter8/07-load-obj-mtl.vue'),
      },
      {
        path: '08-load-collada',
        component: () => import('@/views/chapter8/08-load-collada.vue'),
      },
      {
        path: '09-load-stl',
        component: () => import('@/views/chapter8/09-load-stl.vue'),
      },
      {
        path: '11-load-vtk',
        component: () => import('@/views/chapter8/11-load-vtk.vue'),
      },
      {
        path: '12-load-pdb',
        component: () => import('@/views/chapter8/12-load-pdb.vue'),
      },
      {
        path: '13-load-ply',
        component: () => import('@/views/chapter8/13-load-ply.vue'),
      },
      {
        path: '16-load-vrml',
        component: () => import('@/views/chapter8/16-load-vrml.vue'),
      },
    ],
  },
  {
    path: '/chapter9/',
    children: [
      {
        path: '01-basic-animation',
        component: () => import('@/views/chapter9/01-basic-animation.vue'),
      },
      {
        path: '02-selecting-objects',
        component: () => import('@/views/chapter9/02-selecting-objects.vue'),
      },
    ],
  },
  { path: '/:path(.*)', component: NotFound },
]

const router = createRouter({
  history: createWebHashHistory(),
  routes,
})

export default router
