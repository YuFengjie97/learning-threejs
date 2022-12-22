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
import rainPng from '@/assets/texture/raindrop-3.png?url'

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

const rainNum = 10000
const vertexes = new Float32Array(rainNum * 3)
const rainGeo = new THREE.BufferGeometry()
const textureLoader = new THREE.TextureLoader()
const rainTexture = textureLoader.load(rainPng)
const rainMat = new THREE.PointsMaterial({
  size: 20,
  color: 0x74b9ff,
  map: rainTexture,
  transparent: true,
  opacity: 1,
  depthTest: false,
  blending: THREE.AdditiveBlending,
})
let rainCloud: THREE.Points
const rains: Rain[] = []
const range = 2000

onMounted(() => {
  initGUI()
  initTHREE()
  initRain()
  render()
})

const params = {
  g: 0.1,
  stop: false,
  color: 0x74b9ff,
  size: 20,
  opacity: 1,
  rainNum: 10000
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
  gui.add(params, 'g', 0.01, 0.2, 0.001).name('gravity')
  gui.add(params, 'stop')
  gui.add(params, 'rainNum',0,10000,1).onChange(val=>rainGeo.setDrawRange(0,val))
  gui.addColor(params, 'color').onChange((val) => rainMat.color.setHex(val))
  gui.add(params, 'size', 0.1, 30, 0.1).onChange((val) => (rainMat.size = val))
  gui
    .add(params, 'opacity', 0.1, 1, 0.1)
    .onChange((val) => (rainMat.opacity = val))
}
class Rain {
  xInd: number
  yInd: number
  zInd: number
  vel = random()
  constructor(xInd: number, yInd: number, zInd: number) {
    this.xInd = xInd
    this.yInd = yInd
    this.zInd = zInd
  }
  update() {
    this.vel += params.g
    vertexes[this.yInd] -= this.vel
  }
  edge() {
    if (vertexes[this.yInd] < -range / 2) {
      vertexes[this.yInd] = random() * -0.5 * range + range
      this.vel = random()
    }
  }
}

function animate() {
  if (!params.stop) updateRain()
}
function initRain() {
  const randomPos = () => random() * range - range / 2
  for (let i = 0; i < rainNum; i++) {
    vertexes[i * 3] = randomPos()
    vertexes[i * 3 + 1] = randomPos()
    vertexes[i * 3 + 2] = randomPos()
    const rain = new Rain(i * 3, i * 3 + 1, i * 3 + 2)
    rains.push(rain)
  }
  rainGeo.setAttribute('position', new THREE.BufferAttribute(vertexes, 3))
  rainCloud = new THREE.Points(rainGeo, rainMat)
  scene.add(rainCloud)
}
function updateRain() {
  rains.forEach((r) => {
    r.edge()
    r.update()
  })
  rainGeo.attributes.position.needsUpdate = true
  // rains.geometry.attributes.position.needsUpdate = true
}
// three初始化
function initTHREE() {
  scene = new THREE.Scene()
  camera = new THREE.PerspectiveCamera(75, width / height, 1, 1000)
  renderer = new THREE.WebGLRenderer({
    canvas: canvasDom.value,
    antialias: true,
  })
  renderer.setSize(width, height)
  // renderer.setPixelRatio(window.devicePixelRatio) // 不推荐
  window.addEventListener('resize', onWindowResize)
  scene.add(new THREE.AxesHelper(1000))
  scene.add(new THREE.AmbientLight(0x404040))
  stats = Stats()
  canvasCon.value!.append(stats.dom)
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.object.position.set(0, 0, 600)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
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
