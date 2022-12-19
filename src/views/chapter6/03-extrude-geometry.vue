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
import { createMesh, setCenter } from '@/utils'

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
let mesh: THREE.Group

onMounted(() => {
  initTHREE()
  initMesh()
  initGUI()
  render()
})

const extrudeGeometryOptions = {
  curveSegments: 12,
  steps: 1,
  depth: 12,
  bevelEnabled: true,
  bevelThickness: 0.02,
  bevelSize: 0.1,
  bevelOffset: 0,
  bevelSegments: 3,
}
const params = {
  rotateSpeed: 0.01,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(params, 'rotateSpeed', 0, 0.04, 0.01)
  gui
    .add(extrudeGeometryOptions, 'curveSegments', 1, 24, 1)
    .onChange(() => updateMesh())
  gui
    .add(extrudeGeometryOptions, 'steps', 1, 24, 1)
    .onChange(() => updateMesh())
  gui
    .add(extrudeGeometryOptions, 'depth', 1, 24, 1)
    .onChange(() => updateMesh())
  gui.add(extrudeGeometryOptions, 'bevelEnabled').onChange(() => updateMesh())
  gui
    .add(extrudeGeometryOptions, 'bevelThickness', 0, 10, 0.01)
    .onChange(() => updateMesh())
  gui
    .add(extrudeGeometryOptions, 'bevelSize', 0, 10, 0.1)
    .onChange(() => updateMesh())
  gui
    .add(extrudeGeometryOptions, 'bevelOffset', 0, 10, 0.1)
    .onChange(() => updateMesh())
  gui
    .add(extrudeGeometryOptions, 'bevelSegments', 0, 10, 1)
    .onChange(() => updateMesh())
}

let step = 0
function animate() {
  mesh.rotation.x = step += params.rotateSpeed
  mesh.rotation.y = step += params.rotateSpeed
}
function initGeo() {
  let geo = new THREE.ExtrudeGeometry(getShape()(), extrudeGeometryOptions)
  geo.applyMatrix4(new THREE.Matrix4().makeTranslation(-40, -40, 0))
  return geo
}
function initMesh() {
  let geo = initGeo()
  mesh = createMesh(geo)
  mesh.scale.multiplyScalar(0.4)
  // setCenter(mesh)
  scene.add(mesh)
}
function updateMesh() {
  ;(mesh.children[0] as THREE.Mesh).geometry.dispose()
  ;(mesh.children[1] as THREE.Mesh).geometry.dispose()
  ;(mesh.children[0] as THREE.Mesh).geometry = initGeo()
  ;(mesh.children[1] as THREE.Mesh).geometry = initGeo()
}
function getShape() {
  let shape: THREE.Shape
  return function () {
    if (shape) return shape
    const smileyShape = new THREE.Shape()
      .moveTo(80, 40)
      .absarc(40, 40, 40, 0, Math.PI * 2, false)

    const smileyEye1Path = new THREE.Path()
      .moveTo(35, 20)
      .absellipse(25, 20, 10, 10, 0, Math.PI * 2, true, 0)

    const smileyEye2Path = new THREE.Path()
      .moveTo(65, 20)
      .absarc(55, 20, 10, 0, Math.PI * 2, true)

    const smileyMouthPath = new THREE.Path()
      .moveTo(20, 40)
      .quadraticCurveTo(40, 60, 60, 40)
      .bezierCurveTo(70, 45, 70, 50, 60, 60)
      .quadraticCurveTo(40, 80, 20, 60)
      .quadraticCurveTo(5, 50, 20, 40)

    smileyShape.holes.push(smileyEye1Path)
    smileyShape.holes.push(smileyEye2Path)
    smileyShape.holes.push(smileyMouthPath)
    shape = smileyShape
    return shape
  }
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
  scene.add(new THREE.AmbientLight(0xffffff))

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
