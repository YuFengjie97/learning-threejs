<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * stl模型数据仅仅是geometry
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import { STLLoader } from 'three/examples/jsm/loaders/STLLoader'
import modelUrl from '@/assets/models/SolidHead_2_lowPoly_42k.stl?url'
import { createMultiMaterialObject } from 'three/examples/jsm/utils/SceneUtils'

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

let model: THREE.Group

onMounted(async() => {
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

const loader = new STLLoader()
function modelLoad(modelUrl: string): Promise<THREE.BufferGeometry>{
  return new Promise((resolve, reject)=>{
    loader.load(modelUrl, resolve)
  })
}

async function initMesh(){
  const modelGeo = await modelLoad(modelUrl)
  modelGeo.applyMatrix4(new THREE.Matrix4().makeScale(0.5,0.5,0.5))
  modelGeo.applyMatrix4(new THREE.Matrix4().makeRotationX(-PI/2))
  modelGeo.applyMatrix4(new THREE.Matrix4().makeTranslation(-30,0,30))
  const mat1 = new THREE.MeshNormalMaterial()
  const mat2 = new THREE.MeshBasicMaterial({wireframe: true})
  model = createMultiMaterialObject(modelGeo, [mat1,mat2])
  scene.add(model)
}
function initLight() {
  const light = new THREE.PointLight(0xff0000, 2)
  light.position.set(50,50,50)
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
  orbitControls.target = new THREE.Vector3(0, 40, 0)
  orbitControls.object.position.set(0, 40, 100)
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
