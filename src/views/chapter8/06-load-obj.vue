<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * 读取obj模型，会返回一个THREE.Group
 * 在这个例子中，我获取了obj的geo，然后通过createMultiMaterialObject为其添加了两种材质，
 * 原有的mesh好像不能直接修改material属性为材质数组，不知道有没有这种方法来修改mesh材质
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import modelUrl from '@/assets/models/FinalBaseMesh.obj?url'
import { createMultiMaterialObject } from 'three/examples/jsm/utils/SceneUtils'

import { OBJLoader } from 'three/examples/jsm/loaders/OBJLoader'

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

const loader = new OBJLoader()
let model: THREE.Object3D

onMounted(() => {
  initGUI()
  initTHREE()
  initLight()
  initMesh()
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

function animate() {}
function initMesh() {
  loader.load(modelUrl,function(mesh){
    const mat1 = new THREE.MeshNormalMaterial()
    const mat2 = new THREE.MeshBasicMaterial({wireframe: true})
    model = createMultiMaterialObject((mesh.children[0] as THREE.Mesh).geometry, [mat1, mat2])
    model.scale.set(4,4,4)
    scene.add(model)

    // (mesh.children[0] as THREE.Mesh).material = [mat1, mat2]
    // scene.add(mesh)
  })
}
function initLight() {
  const light = new THREE.DirectionalLight(0xaabbff, 0.3)
  light.position.x = 0
  light.position.y = 0
  light.position.z = 100
  light.target = scene
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
  orbitControls.object.position.set(0, 40, 60)
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
