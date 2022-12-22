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
import spritImg from '@/assets/texture/sprite-sheet.png?url'

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

const textureLoader = new THREE.TextureLoader()
const spriteTexture = textureLoader.load(spritImg)
let sprite: THREE.Sprite
// 初始化精灵材质
const spriteMat = new THREE.SpriteMaterial({
  color: 0xffffff,
  map: spriteTexture,
  transparent: true,
  opacity: 0.5,
  depthTest: false,
  blending: THREE.AdditiveBlending,
})
spriteMat.map!.offset = new THREE.Vector2(0.2 * 0, 0)
spriteMat.map!.repeat = new THREE.Vector2(1 / 5, 1)

let sphere: THREE.Mesh

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
  render()
})

const spriteParams = {
  size: 10,
  type: 0,
  depthTest: false,
  blending: 'AdditiveBlending',
}
type blendingType =
  | 'NoBlending'
  | 'NormalBlending'
  | 'AdditiveBlending'
  | 'SubtractiveBlending'
  | 'MultiplyBlending'
  | 'CustomBlending'
const blendingMap = {
  NoBlending: THREE.NoBlending,
  NormalBlending: THREE.NormalBlending,
  AdditiveBlending: THREE.AdditiveBlending,
  SubtractiveBlending: THREE.SubtractiveBlending,
  MultiplyBlending: THREE.MultiplyBlending,
  CustomBlending: THREE.CustomBlending,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(spriteParams, 'size', 1, 15, 1).onChange((val) => {
    sprite.scale.set(val, val, 1)
  })
  gui.add(spriteParams, 'type', [0, 1, 2, 3, 4]).onChange((val) => {
    spriteMat.map!.offset = new THREE.Vector2(0.2 * val, 0)
  })
  gui.add(spriteParams, 'depthTest').onChange((val) => {
    spriteMat.depthTest = val
  })
  gui
    .add(spriteParams, 'blending', [
      'NoBlending',
      'NormalBlending',
      'AdditiveBlending',
      'SubtractiveBlending',
      'MultiplyBlending',
      'CustomBlending',
    ])
    .onChange((val) => {
      spriteMat.blending = blendingMap[val as blendingType]
    })
}

let dy = 0.2
let dx = 0.5
function animate() {
  sphere.position.y += dy
  let range = 20
  if (sphere.position.y >= range || sphere.position.y <= -range) {
    dy *= -1
  }

  sprite.position.x += dx
  let range2 = 30
  if (sprite.position.x >= range2 || sprite.position.x <= -range2) {
    dx *= -1
  }
}
function initMesh() {
  const sphereGeo = new THREE.SphereGeometry(10)
  const sphereMat = new THREE.MeshNormalMaterial()
  sphere = new THREE.Mesh(sphereGeo, sphereMat)
  scene.add(sphere)

  sprite = new THREE.Sprite(spriteMat)

  sprite.scale.set(spriteParams.size, spriteParams.size, 1)
  scene.add(sprite)
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
