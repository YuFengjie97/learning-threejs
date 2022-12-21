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
const size = 20
const gap = 10
const vertexes: number[] = []
const colors: number[] = []

onMounted(() => {
  initGUI()
  initTHREE()
  initSprites()
  render()
})

function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
}

function animate() {
  sprites.rotation.x += 0.01
  sprites.rotation.y += 0.01
}
function initSprites() {
  const mat = new THREE.PointsMaterial({
    size: 1,
    vertexColors: true
  })
  for (let x = 0; x < size; x++) {
    for (let y = 0; y < size; y++) {
      for (let z = 0; z < size; z++) {
        vertexes.push(x * gap, y * gap, z * gap)
        const cx = x / size
        const cy = y / size
        const cz = z / size
        colors.push(cx, cy, cz)
      }
    }
  }
  // spritesGeo.setAttribute('position', new THREE.BufferAttribute(vertexes, 3))
  spritesGeo.setAttribute(
    'position',
    new THREE.Float32BufferAttribute(vertexes, 3)
  )

  spritesGeo.setAttribute('color', new THREE.Float32BufferAttribute(colors, 3))
  const offset = -0.5 * size * gap
  spritesGeo.applyMatrix4(
    new THREE.Matrix4().makeTranslation(offset, offset, offset)
  )
  spritesGeo.setDrawRange(0, size ** 3)
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
  orbitControls.object.position.set(1, 1, 1)
  orbitControls.target = new THREE.Vector3(gap, gap, gap)
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
