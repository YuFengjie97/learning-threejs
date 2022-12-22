<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * spriteMaterial中的map属性应该单独对应一个材质对象
 * 不然所有材质将共用一个引用对象
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import spritImg from '@/assets/texture/sprite-sheet.png?url'
import { getRandomVec3 } from '@/utils'

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

const range = 500
const spriteNum = 500
const textureLoader = new THREE.TextureLoader()
function getTexture() {
  return textureLoader.load(spritImg)
}
let sprites = new THREE.Group()
let spriteMats: THREE.SpriteMaterial[] = []
for (let i = 0; i < 5; i++) {
  const mat = createSpriteMat(i)
  spriteMats.push(mat)
}
console.log(spriteMats)

function createSpriteMat(ind: number) {
  const spriteMat = new THREE.SpriteMaterial({
    color: 0xffffff,
    map: getTexture(),
    transparent: true,
    opacity: 1,
    depthTest: false,
    blending: THREE.AdditiveBlending,
  })
  spriteMat.map!.offset = new THREE.Vector2(0.2 * ind, 0)
  spriteMat.map!.repeat = new THREE.Vector2(1 / 5, 1)
  return spriteMat
}

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
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
function initMesh() {
  scene.add(sprites)
  for (let i = 0; i < spriteNum; i++) {
    let s = new THREE.Sprite(spriteMats[i % 5])
    s.position.copy(getRandomVec3(-range / 2, range / 2))
    s.scale.multiplyScalar(10)
    sprites.add(s)
  }
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
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(0, 0, 30)
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
