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
let torusKnot: THREE.Group

onMounted(() => {
  initTHREE()
  createMesh()
  initGUI()
  render()
})

const params = {
  radius: 15,
  tube:5,
  tubularSegments:27,
  radialSegments:2,
  p:7,
  q:9,
  scaleX: 1,
  scaleY: 1,
  scaleZ: 2.8,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(params, 'radius',1,40,1).onChange(()=>redraw())
  gui.add(params, 'tube',0.01,40,0.01).onChange(()=>redraw())
  gui.add(params, 'tubularSegments',1,40,1).onChange(()=>redraw())
  gui.add(params, 'radialSegments',1,40,1).onChange(()=>redraw())
  gui.add(params, 'p',1,10,1).onChange(()=>redraw())
  gui.add(params, 'q',1,10,1).onChange(()=>redraw())
  gui.add(params,'scaleX',0.1, 3,0.1).onChange(()=>updateScale())
  gui.add(params,'scaleY',0.1, 3,0.1).onChange(()=>updateScale())
  gui.add(params,'scaleZ',0.1, 3,0.1).onChange(()=>updateScale())
}

let step = 0
function animate() {
  torusKnot.rotation.y = step += 0.01
}
function createMesh() {
  const {
    radius,
    tube,
    tubularSegments,
    radialSegments,
    p,
    q,
  } = params
  const geo = new THREE.TorusKnotGeometry(
    radius,
    tube,
    tubularSegments,
    radialSegments,
    p,
    q,
  )
  const matNormal = new THREE.MeshNormalMaterial({side: THREE.DoubleSide})
  const matBasic = new THREE.MeshBasicMaterial({ wireframe: true })
  torusKnot = createMultiMaterialObject(geo, [matNormal, matBasic])
  scene.add(torusKnot)
}
function redraw() {
  scene.remove(torusKnot)
  createMesh()
}
function updateScale(){
  const {scaleX, scaleY, scaleZ} = params
  torusKnot.scale.set(scaleX, scaleY, scaleZ)
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
