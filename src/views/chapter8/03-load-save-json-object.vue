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

let knot: THREE.Mesh
let knotCopy: THREE.Mesh

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
  render()
})

const objLoader = new THREE.ObjectLoader()
const knotParams = {
  radius: 50,
  tube: 6,
  tubularSegments: 156,
  radialSegments: 12,
  p: 5,
  q: 4,
}
localStorage.clear()
const params = {
  save: function(){
    const knotJson = knot.toJSON()
    localStorage.setItem('knotJson', JSON.stringify(knotJson))
  },
  load: function(){
    if(knotCopy) scene.remove(knotCopy)
    const knotJson = localStorage.getItem('knotJson')
    
    if(knotJson){
      const knotObj = JSON.parse(knotJson)
      knotCopy = objLoader.parse(knotObj)
      knotCopy.position.x = -100
      scene.add(knotCopy)
    }
  }
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

  gui.add(params, 'save').listen()
  gui.add(params, 'load').listen()
}

function animate() {
  knot.rotation.y += 0.01
}
function initMesh() {
  // 管道
  const knotGeo = new THREE.TorusKnotGeometry(...Object.values(knotParams))
  const knotMat = new THREE.MeshBasicMaterial({wireframe: true})
  knot = new THREE.Mesh(knotGeo, knotMat)
  knot.position.x = 100
  scene.add(knot)
}
function updateKnot() {
  const newGeo = new THREE.TorusKnotGeometry(...Object.values(knotParams))
  knot.geometry.dispose()
  knot.geometry = newGeo
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
  orbitControls.object.position.set(0, 10, 200)
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
