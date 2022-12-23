<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * 抽象过的geo不能想bufferGeo通过直接修改属性，然后调用geo.attributes.key.needsupdate = ture来更新几何体吗？
 * 抽象过的geo不也是以bufferGeo为父类实现的吗
 * 答案是不能，看官网写的例子，是生成新的geo，在原来的mesh.geometry上找到旧的geo，dispose掉，然后在geometry属性上重新定义
 * 根据官网文档这些具体的geo对象在初始化后，就不会改变了
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'

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
let knotMat: THREE.MeshNormalMaterial
let knot: THREE.Mesh
let knotCloud: THREE.Points
let knotPointMat: THREE.PointsMaterial

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
  render()
})

const params = {
  showKnot: true,
  showCloud: false,
}
const knotParams = {
  radius: 50,
  tube: 2,
  tubularSegments: 156,
  radialSegments: 12,
  p: 5,
  q: 4,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
  gui.add(knotParams, 'radius', 10, 100, 1).onChange(() => updateKnot())
  gui.add(knotParams, 'tube', 0.4, 10, 0.1).onChange(() => updateKnot())
  gui
    .add(knotParams, 'tubularSegments', 10, 200, 1)
    .onChange(() => updateKnot())
  gui.add(knotParams, 'radialSegments', 10, 20, 1).onChange(() => updateKnot())
  gui.add(knotParams, 'p', 1, 30, 1).onChange(() => updateKnot())
  gui.add(knotParams, 'q', 1, 30, 1).onChange(() => updateKnot())

  gui.add(params, 'showKnot').onChange((val) => (knot.visible = val))
  gui.add(params, 'showCloud').onChange((val) => (knotCloud.visible = val))
}

function animate() {
  // doSomething
}
function initMesh() {
  // 管道
  const knotGeo = new THREE.TorusKnotGeometry(...Object.values(knotParams))
  knotMat = new THREE.MeshNormalMaterial()
  knot = new THREE.Mesh(knotGeo, knotMat)
  scene.add(knot)

  // 管道顶点形成的点云
  knotPointMat = new THREE.PointsMaterial({
    color: 0xfffff,
    size: 2,
    transparent: true,
    blending: THREE.AdditiveBlending,
    depthTest: true,
    map: generateSprite()
  })
  knotCloud = new THREE.Points(knotGeo, knotPointMat)
  knotCloud.visible = false
  scene.add(knotCloud)
}
function updateKnot() {
  const newGeo = new THREE.TorusKnotGeometry(...Object.values(knotParams))
  knot.geometry.dispose()
  knot.geometry = newGeo

  knotCloud.geometry.dispose()
  knotCloud.geometry = newGeo
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
  orbitControls.object.position.set(100, 100, 100)
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
