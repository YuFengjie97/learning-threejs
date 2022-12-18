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
let cylinder: THREE.Group

onMounted(() => {
  initTHREE()
  createMesh()
  initGUI()
  render()
})

const params = {
  radiusTop:20,
  radiusBottom:20,
  height:20,
  radiusSegments:8,
  heightSegments:8,
  openEnded:false,
  thetaStart:0,
  thetaLength: PI*2,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(params,'radiusTop',0,40,1).onChange(()=>redraw())
  gui.add(params,'radiusBottom',0,40,1).onChange(()=>redraw())
  gui.add(params,'height',0,40,1).onChange(()=>redraw())
  gui.add(params,'radiusSegments',0,40,1).onChange(()=>redraw())
  gui.add(params,'heightSegments',0,40,1).onChange(()=>redraw())
  gui.add(params,'openEnded').onChange(()=>redraw())
  gui.add(params,'thetaStart',0,PI*2,0.01).onChange(()=>redraw())
  gui.add(params,'thetaLength',0,PI*2,0.01).onChange(()=>redraw())
}

let step = 0
function animate() {
  cylinder.rotation.y = step += 0.01
}
function createMesh() {
  const {
    radiusTop,
    radiusBottom,
    height,
    radiusSegments,
    heightSegments,
    openEnded,
    thetaStart,
    thetaLength,
  } = params
  const geo = new THREE.CylinderGeometry(
    radiusTop,
    radiusBottom,
    height,
    radiusSegments,
    heightSegments,
    openEnded,
    thetaStart,
    thetaLength,
  )
  const matNormal = new THREE.MeshNormalMaterial()
  const matBasic = new THREE.MeshBasicMaterial({ wireframe: true })
  cylinder = createMultiMaterialObject(geo, [matNormal, matBasic])
  scene.add(cylinder)
}
function redraw() {
  scene.remove(cylinder)
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
