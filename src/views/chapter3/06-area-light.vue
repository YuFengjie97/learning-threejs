<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * 注意：
 * 不支持阴影。
 * 只支持 MeshStandardMaterial 和 MeshPhysicalMaterial 两种材质。
 * 你必须在你的场景中加入 RectAreaLightUniformsLib ，并调用init()。
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import { RectAreaLightUniformsLib } from 'three/examples/jsm/lights/RectAreaLightUniformsLib'
import { RectAreaLightHelper } from 'three/examples/jsm/helpers/RectAreaLightHelper'

const { random, PI, floor, ceil, min, max, sin, cos, sqrt } = Math

const canvasDom = ref<HTMLElement>()
const canvasCon = ref<HTMLElement>()
let width = window.innerWidth
let height = window.innerHeight
let stats: Stats
let orbitControls: OrbitControls
let scene: THREE.Scene
let camera: THREE.PerspectiveCamera
let renderer: THREE.WebGLRenderer

let planeMesh: THREE.Mesh

let lights: Array<AreaLight> = []

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
  initLight()
  render()
})
interface LightParam {
  showHelper: boolean,
  color: number
  intensity: number
  width: number
  height: number
}
const light1Params: LightParam = {
  showHelper: true,
  color: 0xff0000,
  intensity: 4,
  width: 12,
  height: 20,
}
const light2Params: LightParam = {
  showHelper: true,
  color: 0x00ff00,
  intensity: 4,
  width: 12,
  height: 20,
}
const light3Params: LightParam = {
  showHelper: true,
  color: 0x0000ff,
  intensity: 4,
  width: 12,
  height: 20,
}

function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  const folder1 = gui.addFolder('light1')
  folder1.open()
  folder1.add(light1Params, 'showHelper').onChange(val=>{
    lights[0].helper.visible = val
  })
  folder1.addColor(light1Params,'color').onChange(val=>{
    lights[0].light.color.setHex(val)
  })
  folder1.add(light1Params, 'intensity',0,light1Params.intensity*2,0.1).onChange(val=>{
    lights[0].light.intensity = val
  })
  folder1.add(light1Params, 'width',1,light1Params.width*2,1).onChange(val=>{
    lights[0].light.width = val
  })
  folder1.add(light1Params, 'height',1,light1Params.height*2,1).onChange(val=>{
    lights[0].light.height = val
  })

  const folder2 = gui.addFolder('light2')
  folder2.open()
  folder2.add(light2Params, 'showHelper').onChange(val=>{
    lights[1].helper.visible = val
  })
  folder2.addColor(light2Params,'color').onChange(val=>{
    lights[1].light.color.setHex(val)
  })
  folder2.add(light2Params, 'intensity',0,light2Params.intensity*2,0.1).onChange(val=>{
    lights[1].light.intensity = val
  })
  folder2.add(light2Params, 'width',1,light2Params.width*2,1).onChange(val=>{
    lights[1].light.width = val
  })
  folder2.add(light2Params, 'height',1,light2Params.height*2,1).onChange(val=>{
    lights[1].light.height = val
  })

  const folder3 = gui.addFolder('light3')
  folder3.open()
  folder3.add(light3Params, 'showHelper').onChange(val=>{
    lights[2].helper.visible = val
  })
  folder3.addColor(light3Params,'color').onChange(val=>{
    lights[2].light.color.setHex(val)
  })
  folder3.add(light3Params, 'intensity',0,light3Params.intensity*2,0.1).onChange(val=>{
    lights[2].light.intensity = val
  })
  folder3.add(light3Params, 'width',1,light3Params.width*2,1).onChange(val=>{
    lights[2].light.width = val
  })
  folder3.add(light3Params, 'height',1,light3Params.height*2,1).onChange(val=>{
    lights[2].light.height = val
  })
}

function initMesh() {
  const planeGeo = new THREE.PlaneGeometry(60, 40, 1, 1)
  const planeMat = new THREE.MeshStandardMaterial( { color: 0x808080, roughness: 0.1, metalness: 0 } );
  planeMesh = new THREE.Mesh(planeGeo, planeMat)
  planeMesh.rotation.x = -0.5 * PI
  planeMesh.receiveShadow = true
  scene.add(planeMesh)
}

function initLight() {
  RectAreaLightUniformsLib.init()
  const l1 = new AreaLight(light1Params, -12, 8, -20)
  const l2 = new AreaLight(light2Params, 0, 8, -20)
  const l3 = new AreaLight(light3Params, 12, 8, -20)
  lights.push(...[l1, l2, l3])
}

class AreaLight {
  light: THREE.RectAreaLight
  helper: RectAreaLightHelper
  constructor(p: LightParam, x: number, y: number, z: number) {
    this.light = new THREE.RectAreaLight(
      p.color,
      p.intensity,
      p.width,
      p.height
    )
    this.light.position.set(x, y-1, z)
    this.light.rotation.y = PI
    scene.add(this.light)

    this.helper = new RectAreaLightHelper(this.light)
    scene.add(this.helper)
  }
}

function animate() {}
// 绘制
function render() {
  requestAnimationFrame(render)
  stats.update()
  animate()
  renderer.render(scene, camera)
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
  // renderer.setPixelRatio(window.devicePixelRatio) // 不推荐
  window.addEventListener('resize', onWindowResize)

  // 坐标轴
  scene.add(new THREE.AxesHelper(1000))

  // 环境光
  // scene.add(new THREE.AmbientLight(0x404040))

  // 帧率状态
  stats = Stats()
  canvasCon.value!.append(stats.dom)

  // 轨道控制器
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(-10, 10, 20)
  orbitControls.update()
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
