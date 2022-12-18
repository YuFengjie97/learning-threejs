<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import { createMultiMaterialObject } from 'three/examples/jsm/utils/SceneUtils'

const { random, PI, floor, ceil, min, max, sin, cos } = Math

const canvasDom = ref<HTMLElement>()
const canvasCon = ref<HTMLElement>()
let width = window.innerWidth
let height = window.innerHeight
let stats: Stats
let orbitControls: OrbitControls
let scene: THREE.Scene
let camera: THREE.PerspectiveCamera
let renderer: THREE.WebGLRenderer
let polyhedron: THREE.Group

onMounted(() => {
  initTHREE()
  createMesh('Icosahedron')
  initGUI()
  render()
})

// prettier-ignore
type polyType = 'Icosahedron'|'Tetrahedron'|'Octahedron'|'Dodecahedron'|'Custom'
const params: {
  type: polyType
  radius: number
  details: number
} = {
  type: 'Icosahedron',
  radius: 14,
  details: 0,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui
    .add(params, 'type', [
      'Icosahedron',
      'Tetrahedron',
      'Octahedron',
      'Dodecahedron',
      'Custom',
    ])
    .onChange((val) => createMesh(val))
  gui
    .add(params, 'radius', 1, 20, 1)
    .onChange((val) => createMesh(params.type))
  gui
    .add(params, 'details', 0, 20, 1)
    .onChange((val) => createMesh(params.type))
}

let step = 0
function animate() {
  polyhedron.rotation.y = step += 0.01
}
function createMesh(type: polyType) {
  if(polyhedron) scene.remove(polyhedron)
  const { radius, details } = params
  let geo
  switch (type) {
    case 'Icosahedron':
      geo = new THREE.IcosahedronGeometry(radius, details)
      break
    case 'Tetrahedron':
      geo = new THREE.TetrahedronGeometry(radius, details)
      break
    case 'Octahedron':
      geo = new THREE.OctahedronGeometry(radius, details)
      break
    case 'Dodecahedron':
      geo = new THREE.DodecahedronGeometry(radius, details)
      break
    case 'Custom':
      const vertices = [1, 1, 1, -1, -1, 1, -1, 1, -1, 1, -1, -1]
      const indices = [2, 1, 0, 0, 3, 2, 1, 3, 0, 2, 3, 1]
      geo = new THREE.PolyhedronGeometry(vertices, indices, radius, details)
      break
  }
  const matNormal = new THREE.MeshNormalMaterial({ side: THREE.DoubleSide })
  const matBasic = new THREE.MeshBasicMaterial({ wireframe: true })
  polyhedron = createMultiMaterialObject(geo, [matNormal, matBasic])
  scene.add(polyhedron)
}

// three初始化
function initTHREE() {
  scene = new THREE.Scene()
  scene.background = new THREE.Color(0xffffff)
  camera = new THREE.PerspectiveCamera(75, width / height, 1, 1000)
  renderer = new THREE.WebGLRenderer({
    canvas: canvasDom.value,
    antialias: true,
  })
  // renderer.shadowMap.enabled = true
  renderer.setSize(width, height)
  // renderer.setPixelRatio(window.devicePixelRatio) // 不推荐
  window.addEventListener('resize', onWindowResize)

  // 坐标轴
  scene.add(new THREE.AxesHelper(1000))

  // 环境光
  // scene.add(new THREE.AmbientLight(0xffffff))

  // 帧率状态
  stats = Stats()
  canvasCon.value!.append(stats.dom)

  // 轨道控制器
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(0, 30, 50)
  orbitControls.update()
}
// 绘制
function render() {
  requestAnimationFrame(render)
  stats.update()
  animate()
  renderer.render(scene, camera)
}
// 自适应
function onWindowResize() {
  width = window.innerWidth
  height = window.innerHeight
  camera.aspect = width / height
  camera.updateProjectionMatrix()
  renderer.setSize(width, height)
}
</script>
