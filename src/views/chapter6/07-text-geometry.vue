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
import { TextGeometry } from 'three/examples/jsm/geometries/TextGeometry'
import helvetiker from 'three/examples/fonts/helvetiker_regular.typeface.json?url'
import optimer from 'three/examples/fonts/optimer_regular.typeface.json?url'
import gentilis from 'three/examples/fonts/gentilis_regular.typeface.json?url'
import { Font, FontLoader } from 'three/examples/jsm/loaders/FontLoader'

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
let light: THREE.DirectionalLight
let lightHelper: THREE.DirectionalLightHelper
let mesh: THREE.Group
let fontHelvetiker: Font
let fontOptimer: Font
let fontGentilis: Font

onMounted(async () => {
  initTHREE()
  fontHelvetiker = await loadFont(helvetiker)
  fontOptimer = await loadFont(optimer)
  fontGentilis = await loadFont(gentilis)
  initMesh()
  initLight(0xffffff,-30, 30, -30)
  initLight(0xffffff,-30, 30, 30)
  initGUI()
  render()
})

const textParams = {
  type: 'gentilis',
  size: 20,
  height: 8,
  curveSegments: 20,
  bevelEnabled: true,
  bevelThickness: 40,
  bevelSize: 11,
  bevelOffset: 1,
  bevelSegments: 2,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
  gui.add(textParams, 'type', ['helvetiker', 'optimer', 'gentilis']).onChange(()=>initMesh())
  gui.add(textParams, 'size',0,20,1).onChange(()=>initMesh())
  gui.add(textParams, 'height',0,20,1).onChange(()=>initMesh())
  gui.add(textParams, 'curveSegments',1,20,1).onChange(()=>initMesh())
  gui.add(textParams, 'bevelEnabled').onChange(()=>initMesh())
  gui.add(textParams, 'bevelThickness',0,40,1).onChange(()=>initMesh())
  gui.add(textParams, 'bevelSize',0,20,1).onChange(()=>initMesh())
  gui.add(textParams, 'bevelOffset',0,20,1).onChange(()=>initMesh())
  gui.add(textParams, 'bevelSegments',0,20,1).onChange(()=>initMesh())
}

function loadFont(jsonUrl: string): Promise<Font> {
  return new Promise((resolve, reject) => {
    const loader = new FontLoader()
    loader.load(jsonUrl, resolve, undefined, reject)
  })
}

let step = 0
function animate() {}
function initMesh() {
  if(mesh) scene.remove(mesh)
  const {
    type,
    size,
    height,
    curveSegments,
    bevelEnabled,
    bevelThickness,
    bevelSize,
    bevelOffset,
    bevelSegments,
  } = textParams
  let textGeo = new TextGeometry('hello,three.js', {
    font:
      type === 'helvetiker'
        ? fontHelvetiker
        : type === 'optimer'
        ? fontOptimer
        : fontGentilis,
    size,
    height,
    curveSegments,
    bevelEnabled,
    bevelThickness,
    bevelSize,
    bevelOffset,
    bevelSegments,
  })
  textGeo.applyMatrix4(new THREE.Matrix4().makeTranslation(-3*size,0,0))
  createMesh(textGeo)
  scene.add(mesh)
}
function createMesh(geom: THREE.BufferGeometry) {
  const meshMaterial = new THREE.MeshPhongMaterial({
    specular: 0xffffff,
    color: 0xeeffff,
    shininess: 100,
  })
  mesh = createMultiMaterialObject(geom, [meshMaterial])
}
function initLight(color: number, x: number,y: number,z: number) {
  light = new THREE.DirectionalLight(color)
  light.position.set(x, y, z)
  light.intensity = 0.7
  light.target = scene
  lightHelper = new THREE.DirectionalLightHelper(light, 10)
  scene.add(lightHelper)
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
  orbitControls.object.position.set(0, 50, 100)
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
