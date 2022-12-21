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

type MeshType = 'sphere1' | 'sphere2' | 'cube'
let sphere1: THREE.Group
let sphere2: THREE.Group
let cube: THREE.Group

onMounted(async () => {
  initTHREE()
  initMesh()
  initGUI()
  render()
})

const sphere1Params = {
  pos: { x: -10, y: 0, z: 0 },
  scl: { sx: 1, sy: 1, sz: 1 },
}
const sphere2Params = {
  pos: { x: 10, y: 0, z: 0 },
  scl: { sx: 1, sy: 1, sz: 1 },
}
const cubeParams = {
  pos: { x: 0, y: 0, z: 0 },
  scl: { sx: 1, sy: 1, sz: 1 },
}
const params = {
  showWireframes: true,
}

function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(params, 'showWireframes').onChange(val => {
    sphere1.visible = val
    sphere2.visible = val
    cube.visible = val
  })

  const f1 = gui.addFolder('sphere1')
  const f2 = gui.addFolder('sphere2')
  const f3 = gui.addFolder('cube')
  f1.open()
  f2.open()
  f3.open()
  f1.add(sphere1Params.pos, 'x', -20, 20, 0.1).onChange((val) => {
    sphere1.position.x = val
  })
  f1.add(sphere1Params.pos, 'y', -20, 20, 0.1).onChange((val) => {
    sphere1.position.y = val
  })
  f1.add(sphere1Params.pos, 'z', -20, 20, 0.1).onChange((val) => {
    sphere1.position.z = val
  })
  f1.add(sphere1Params.scl, 'sx', 0.1, 2, 0.1).onChange((val) => {
    sphere1.scale.x = val
  })
  f1.add(sphere1Params.scl, 'sy', 0.1, 2, 0.1).onChange((val) => {
    sphere1.scale.y = val
  })
  f1.add(sphere1Params.scl, 'sz', 0.1, 2, 0.1).onChange((val) => {
    sphere1.scale.z = val
  })

  f2.add(sphere2Params.pos, 'x', -20, 20, 0.1).onChange((val) => {
    sphere2.position.x = val
  })
  f2.add(sphere2Params.pos, 'y', -20, 20, 0.1).onChange((val) => {
    sphere2.position.x = val
  })
  f2.add(sphere2Params.pos, 'z', -20, 20, 0.1).onChange((val) => {
    sphere2.position.x = val
  })
  f2.add(sphere2Params.scl, 'sx', 0.1, 2, 0.1).onChange((val) => {
    sphere1.scale.x = val
  })
  f2.add(sphere2Params.scl, 'sy', 0.1, 2, 0.1).onChange((val) => {
    sphere1.scale.y = val
  })
  f2.add(sphere2Params.scl, 'sz', 0.1, 2, 0.1).onChange((val) => {
    sphere1.scale.z = val
  })

  f3.add(cubeParams.pos, 'x', -20, 20, 0.1).onChange((val) => {
    cube.position.x = val
  })
  f3.add(cubeParams.pos, 'y', -20, 20, 0.1).onChange((val) => {
    cube.position.y = val
  })
  f3.add(cubeParams.pos, 'z', -20, 20, 0.1).onChange((val) => {
    cube.position.z = val
  })
  f3.add(cubeParams.scl, 'sx', 0.1, 2, 0.1).onChange((val) => {
    cube.scale.x = val
  })
  f3.add(cubeParams.scl, 'sy', 0.1, 2, 0.1).onChange((val) => {
    cube.scale.y = val
  })
  f3.add(cubeParams.scl, 'sz', 0.1, 2, 0.1).onChange((val) => {
    cube.scale.z = val
  })
}

function initMesh() {
  sphere1 = createMesh(
    new THREE.SphereGeometry(5),
    sphere1Params.pos,
    sphere1Params.scl
  )
  sphere2 = createMesh(
    new THREE.SphereGeometry(5),
    sphere2Params.pos,
    sphere2Params.scl
  )
  cube = createMesh(
    new THREE.BoxGeometry(10, 10, 10),
    cubeParams.pos,
    cubeParams.scl
  )
}
function createMesh(
  geo: THREE.BufferGeometry,
  pos: { x: number; y: number; z: number },
  scl: { sx: number; sy: number; sz: number }
) {
  const mat1 = new THREE.MeshNormalMaterial()
  const mat2 = new THREE.MeshBasicMaterial({
    wireframe: true,
    transparent: true,
    opacity: 0.6,
  })
  const mesh = createMultiMaterialObject(geo, [mat2])
  mesh.position.set(...(Object.values(pos) as [number, number, number]))
  mesh.scale.set(...(Object.values(scl) as [number, number, number]))
  scene.add(mesh)
  return mesh
}

function initTHREE() {
  scene = new THREE.Scene()
  scene.background = new THREE.Color(0x444444)
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
  // 帧率状态
  stats = Stats()
  canvasCon.value!.append(stats.dom)
  // 轨道控制器
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(0, 10, 25)
  orbitControls.update()
}
// 绘制
function render() {
  requestAnimationFrame(render)
  stats.update()
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
