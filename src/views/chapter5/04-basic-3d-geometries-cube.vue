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
let cube: THREE.Group

onMounted(() => {
  initTHREE()
  createMesh()
  initGUI()
  render()
})

const params = {
  width: 10,
  height: 10,
  depth: 10,
  widthSegments: 1,
  heightSegments: 1,
  depthSegments: 1,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
  gui.add(params, 'width', 0, 40).onChange(redraw)
  gui.add(params, 'height', 0, 40).onChange(redraw)
  gui.add(params, 'depth', 0, 40).onChange(redraw)
  gui.add(params, 'widthSegments', 0, 10, 1).onChange(redraw)
  gui.add(params, 'heightSegments', 0, 10, 1).onChange(redraw)
  gui.add(params, 'depthSegments', 0, 10, 1).onChange(redraw)
}

let step = 0
function animate() {
  cube.rotation.y = step += 0.01
}
function createMesh() {
  const { width, height, depth, widthSegments, heightSegments, depthSegments } =
    params
  const cubeGeo = new THREE.BoxGeometry(
    width,
    height,
    depth,
    widthSegments,
    heightSegments,
    depthSegments
  )
  const matNormal = new THREE.MeshNormalMaterial()
  const matBasic = new THREE.MeshBasicMaterial({ wireframe: true })
  cube = createMultiMaterialObject(cubeGeo, [matNormal, matBasic])
  scene.add(cube)
}
function redraw() {
  scene.remove(cube)
  createMesh()
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
  orbitControls.object.position.set(0, 3, 20)
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
