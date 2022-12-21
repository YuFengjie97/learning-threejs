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
let sprites: THREE.Points
const spritesGeo = new THREE.BufferGeometry()
const cols = 10
const rows = 10
const vertexes: number[] = []
const colors: number[] = []

onMounted(() => {
  initGUI()
  initTHREE()
  initSprites()
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
  // doSomething
}
function initSprites() {
  const mat = new THREE.PointsMaterial({
    size: 6,
    vertexColors: true,//使用geo顶点颜色来确定材质
  })
  for (let x = 0; x < cols; x++) {
    for (let y = 0; y < rows; y++) {
      // vertexes[(x + y * 10) * 3] = x * 10
      // vertexes[(x + y * 10) * 3 + 1] = y * 10
      // vertexes[(x + y * 10) * 3 + 2] = 0
      vertexes.push(x * 10,y * 10,0)
      const cx = random()
      const cy = random()
      const cz = random()
      colors.push(cx,cy,cz)
    }
  }
  // spritesGeo.setAttribute('position', new THREE.BufferAttribute(vertexes, 3))
  spritesGeo.setAttribute(
    'position',
    new THREE.Float32BufferAttribute(vertexes, 3)
  )
  
  spritesGeo.setAttribute(
    'color',
    new THREE.Float32BufferAttribute(colors, 3)
  )
  spritesGeo.setDrawRange(0, cols * rows)
  sprites = new THREE.Points(spritesGeo, mat)
  scene.add(sprites)
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
  scene.add(new THREE.AxesHelper(1000))
  scene.add(new THREE.AmbientLight(0x404040))
  stats = Stats()
  canvasCon.value!.append(stats.dom)
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(50, 50, 0)
  orbitControls.object.position.set(50, 50, 100)
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
