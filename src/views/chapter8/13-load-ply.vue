<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<style lang="less" scoped>
.viewCon {
  .canvasCon {
    position: relative;
  }
}
</style>

<script lang="ts" setup>
/**
 * 有些模型导入的geo不支持直接设置normalMaterial材质，不知道为什么
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import { createMultiMaterialObject } from 'three/examples/jsm/utils/SceneUtils'

import { PLYLoader } from 'three/examples/jsm/loaders/PLYLoader'
import modelUrl from '@/assets/models/car.ply?url'

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

// let model: THREE.Group = new THREE.Group()
let model: THREE.Points

onMounted(async () => {
  initGUI()
  initTHREE()
  initLight()
  await initMesh()
  render()
})

const params = {}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
}

function animate() {
  model.rotation.y += 0.01
}

const loader = new PLYLoader()
function modelLoad(modelUrl: string): Promise<THREE.BufferGeometry> {
  return new Promise((resolve, reject) => {
    loader.load(modelUrl, resolve)
  })
}

async function initMesh() {
  const geo = await modelLoad(modelUrl)

  const mat = new THREE.PointsMaterial({
    color: 0xffffff,
    size: 6,
    depthTest: false,
    transparent: true,
    blending: THREE.AdditiveBlending,
    map: generateSprite(),
  })
  model = new THREE.Points(geo, mat)
  model.scale.multiplyScalar(30)
  scene.add(model)
}
function generateSprite() {
  const canvas = document.createElement('canvas')
  canvas.width = 16
  canvas.height = 16

  const context = canvas.getContext('2d')!
  const gradient = context.createRadialGradient(
    canvas.width / 2,
    canvas.height / 2,
    0,
    canvas.width / 2,
    canvas.height / 2,
    canvas.width / 2
  )
  gradient.addColorStop(0, 'rgba(255,255,255,1)')
  gradient.addColorStop(0.2, 'rgba(0,255,255,1)')
  gradient.addColorStop(0.4, 'rgba(0,0,64,1)')
  gradient.addColorStop(1, 'rgba(0,0,0,1)')

  context.fillStyle = gradient
  context.fillRect(0, 0, canvas.width, canvas.height)

  const texture = new THREE.Texture(canvas)
  texture.needsUpdate = true
  return texture
}
function initLight() {
  const hemiLight = new THREE.HemisphereLight(0xffffff, 0x000000, 1)
  scene.add(hemiLight)
  const light = new THREE.PointLight(0xff0000, 2)
  light.position.set(50, 50, 50)
  scene.add(light)
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
  renderer.shadowMap.enabled = true
  renderer.setPixelRatio(window.devicePixelRatio)

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
  orbitControls.object.position.set(0, 0, 400)
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
window.addEventListener('resize', onWindowResize)
function onWindowResize() {
  width = window.innerWidth
  height = window.innerHeight
  camera.aspect = width / height
  camera.updateProjectionMatrix()
  renderer.setSize(width, height)
}
</script>
