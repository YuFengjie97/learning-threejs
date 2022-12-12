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

let cubeMesh: THREE.Mesh
let sphereMesh: THREE.Mesh

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
  initLight()
  render()
})

const params = {
  rotateSpeed: 0.02,
  jumpSpeed: 0.04
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(params, 'rotateSpeed', 0, 1,0.01)
  gui.add(params, 'jumpSpeed',0,1,0.01)
}

function initMesh(){
  const planeGeo = new THREE.PlaneGeometry(60, 20, 1,1)
  const planeMat = new THREE.MeshLambertMaterial({ color: 0xcccccc })
  const planeMesh = new THREE.Mesh(planeGeo, planeMat)
  planeMesh.rotation.x = -0.5*PI
  planeMesh.receiveShadow = true;
  scene.add(planeMesh)

  const cubeGeo = new THREE.BoxGeometry(4,4,4)
  const cubeMat = new THREE.MeshLambertMaterial({color: 0xff0000, wireframe: false})
  cubeMesh = new THREE.Mesh(cubeGeo, cubeMat)
  cubeMesh.position.set(-14,2,0)
  cubeMesh.castShadow = true
  scene.add(cubeMesh)

  const sphereGeo = new THREE.SphereGeometry(4)
  const sphereMat = new THREE.MeshLambertMaterial({ color: 0x7777ff, wireframe: false })
  sphereMesh = new THREE.Mesh(sphereGeo, sphereMat)
  sphereMesh.position.set(4, 4, 0)
  sphereMesh.castShadow = true
  scene.add(sphereMesh)
}

function initLight(){
  const spotLight = new THREE.SpotLight(0xffffff)
  spotLight.position.set(-20 ,10 , 0)
  spotLight.castShadow = true
  const helper = new THREE.SpotLightHelper(spotLight)
  scene.add(helper)
  scene.add(spotLight)
}

let step = 0
function animate() {
  cubeMesh.rotation.x += params.rotateSpeed
  cubeMesh.rotation.y += params.rotateSpeed
  cubeMesh.rotation.z += params.rotateSpeed

  step += params.jumpSpeed;
  sphereMesh.position.x = 5 + ( 10 * (Math.cos(step)));
  sphereMesh.position.y = 2 + ( 6 * Math.abs(Math.sin(step)));
}
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
    antialias: true
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
  orbitControls.object.position.set(0, 20, 20)
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
