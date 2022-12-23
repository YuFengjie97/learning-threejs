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

const group = new THREE.Group()
let cube: THREE.Group
let sphere: THREE.Group

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
  render()
})

type Params = {
  pos: {
    x: number
    y: number
    z: number
  }
  scale: number
}
const params = {
  grouping: false,
  rotate: false,
}
const cubeParams: Params = {
  pos: {
    x: 10,
    y: 0,
    z: 0,
  },
  scale: 1,
}
const sphereParams: Params = {
  pos: {
    x: -10,
    y: 0,
    z: 0,
  },
  scale: 1,
}
const groupParams: Params = {
  pos: {
    x: -10,
    y: 0,
    z: 0,
  },
  scale: 1,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(params, 'grouping')
  gui.add(params, 'rotate')

  const f1 = gui.addFolder('cube')
  f1.open()
  f1.add(cubeParams.pos, 'x', -20, 20, 1).onChange(
    (val) => (cube.position.x = val)
  )
  f1.add(cubeParams.pos, 'y', -20, 20, 1).onChange(
    (val) => (cube.position.y = val)
  )
  f1.add(cubeParams.pos, 'z', -20, 20, 1).onChange(
    (val) => (cube.position.z = val)
  )
  f1.add(cubeParams, 'scale', 0.1, 4, 0.1).onChange((val) =>
    cube.scale.set(val, val, val)
  )

  const f2 = gui.addFolder('sphere')
  f2.open()
  f2.add(sphereParams.pos, 'x', -20, 20, 1).onChange(
    (val) => (sphere.position.x = val)
  )
  f2.add(sphereParams.pos, 'y', -20, 20, 1).onChange(
    (val) => (sphere.position.y = val)
  )
  f2.add(sphereParams.pos, 'z', -20, 20, 1).onChange(
    (val) => (sphere.position.z = val)
  )
  f2.add(sphereParams, 'scale', 0.1, 4, 0.1).onChange((val) =>
    sphere.scale.set(val, val, val)
  )

  const f3 = gui.addFolder('group')
  f3.open()
  f3.add(groupParams.pos, 'x', -20, 20, 1).onChange(
    (val) => (group.position.x = val)
  )
  f3.add(groupParams.pos, 'y', -20, 20, 1).onChange(
    (val) => (group.position.y = val)
  )
  f3.add(groupParams.pos, 'z', -20, 20, 1).onChange(
    (val) => (group.position.z = val)
  )
  f3.add(groupParams, 'scale', 0.1, 4, 0.1).onChange((val) =>
    group.scale.set(val, val, val)
  )
}
let speed = 0.05
function animate() {
  if (params.rotate) {
    if (params.grouping) {
      group.rotation.y += speed
    } else {
      cube.rotation.y += speed
      sphere.rotation.y += speed
    }
  }
}
function initMesh() {
  scene.add(group)
  const groundGeo = new THREE.PlaneGeometry(500, 500, 128, 128)
  const mat1 = new THREE.MeshBasicMaterial({
    color: 0x00b894,
    transparent: true,
    blending: THREE.AdditiveBlending,
  })
  const mat2 = new THREE.MeshBasicMaterial({ wireframe: true })
  const ground = createMultiMaterialObject(groundGeo, [mat1, mat2])
  ground.rotation.x = -PI / 2
  scene.add(ground)

  const cubeGeo = new THREE.BoxGeometry(10, 10, 10, 10, 10, 10)
  cube = createMesh(cubeGeo)
  cube.position.set(
    ...(Object.values(cubeParams.pos) as [number, number, number])
  )
  const sphereGeo = new THREE.SphereGeometry(5, 10, 10)
  sphere = createMesh(sphereGeo)
  sphere.position.set(
    ...(Object.values(sphereParams.pos) as [number, number, number])
  )
  group.add(cube)
  group.add(sphere)
}
function createMesh(geo: THREE.BufferGeometry) {
  const mat1 = new THREE.MeshNormalMaterial({ side: THREE.DoubleSide })
  const mat2 = new THREE.MeshBasicMaterial({ wireframe: true })
  const mesh = createMultiMaterialObject(geo, [mat1, mat2])
  return mesh
}
// three初始化
function initTHREE() {
  scene = new THREE.Scene()
  scene.background = new THREE.Color(0x444444)
  camera = new THREE.PerspectiveCamera(75, width / height, 1, 1000)
  renderer = new THREE.WebGLRenderer({
    canvas: canvasDom.value,
    antialias: true,
  })
  renderer.setSize(width, height)
  // renderer.setPixelRatio(window.devicePixelRatio) // 不推荐
  window.addEventListener('resize', onWindowResize)

  // 坐标轴
  scene.add(new THREE.AxesHelper(1000))

  // 环境光
  scene.add(new THREE.AmbientLight(0x404040))

  // 帧率状态
  stats = Stats()
  canvasCon.value!.append(stats.dom)

  // 轨道控制器
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(0, 10, 40)
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
