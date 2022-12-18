<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * 原来的例子，每个点是使用SphereGeometry。
 * 现在改为使用bufferGeometry来初始化geo，然后使用Points显示点，因为Points默认是矩形来展示点，需要通过导入图片贴图来替换
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import { createMultiMaterialObject } from 'three/examples/jsm/utils/SceneUtils'
import { ConvexGeometry } from 'three/examples/jsm/geometries/ConvexGeometry'
import disc from '@/assets/texture/disc.png?url'

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

const params = {
  pointNum: 50,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
}

let step = 0
function animate() {
  mesh.rotation.y = step += 0.01
}
function getRandomPosition(min: number, max: number) {
  let pos = new THREE.Vector3()
  pos.x = random() * (max - min) + min
  pos.y = random() * (max - min) + min
  pos.z = random() * (max - min) + min
  return pos
}
function initMesh() {
  mesh = new THREE.Group()
  scene.add(mesh)

  // 点
  let points: Array<THREE.Vector3> = []
  for (let i = 0; i < params.pointNum; i++) {
    points.push(getRandomPosition(-15, 15))
  }
  const pointMat = new THREE.PointsMaterial({
    color: 0xd63031,
    size: 1,
    map: loadTexture()(),
    alphaTest: 0.5,
  })
  const pointGeo = new THREE.BufferGeometry().setFromPoints(points)
  const pointMesh = new THREE.Points(pointGeo, pointMat)
  mesh.add(pointMesh)

  // 凹面体
  const convexGeo = new ConvexGeometry(points)
  const convexMat1 = new THREE.MeshBasicMaterial({
    color: 0x0984e3,
    transparent: true,
    opacity: 0.5,
  })
  const convexMat2 = new THREE.MeshBasicMaterial({ wireframe: true })
  const convex = createMultiMaterialObject(convexGeo, [convexMat1, convexMat2])
  mesh.add(convex)
}
function loadTexture() {
  let texture: THREE.Texture
  const loader = new THREE.TextureLoader()
  return function () {
    if (texture) return texture
    else {
      texture = loader.load(disc)
      return texture
    }
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
