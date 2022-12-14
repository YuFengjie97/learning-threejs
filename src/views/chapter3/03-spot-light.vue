<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * target与点光源的位置模拟是使用的伯努利双纽线
 * https://www.zhihu.com/question/40717216
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'

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
let cubeMesh: THREE.Mesh
let sphereMesh: THREE.Mesh
let targetMesh: THREE.Mesh

let ambientLight: THREE.AmbientLight
let spotLight: THREE.SpotLight
let spotLightHelper: THREE.SpotLightHelper

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
  initLight()
  render()
})

const params = {
  ambientLightColor: 0xffffff,
  disableSpotlight: false,
  rotateSpeed: 0.02,
  jumpSpeed: 0.04,
}
const spotLightParams = {
  stopMoving: false,
  showHelper: true,
  castShadow: true,
  target: 'plane',
  color: 0x0984e3,
  intensity: 2,
  distance: 200,
  decay: 1,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.addColor(params, 'ambientLightColor').onChange((val) => {
    ambientLight.color = new THREE.Color(val)
  })
  gui.add(params, 'rotateSpeed', 0, 1, 0.01)
  gui.add(params, 'jumpSpeed', 0, 1, 0.01)

  const folder = gui.addFolder('spotLight')
  folder.open()
  folder.add(spotLightParams, 'stopMoving')
  folder.add(spotLightParams, 'showHelper').onChange(val => {
    spotLightHelper.visible = val
  })
  folder.add(spotLightParams, 'castShadow').onChange(val=>{
    spotLight.castShadow = val
  })
  folder
    .add(spotLightParams, 'target', ['plane', 'cube', 'sphere'])
    .onChange((val) => {
      if (val === 'plane') spotLight.target = planeMesh
      else if (val === 'cube') spotLight.target = cubeMesh
      else if (val === 'sphere') spotLight.target = sphereMesh
    })
  folder.addColor(spotLightParams, 'color').onChange((val) => {
    spotLight.color = new THREE.Color(val)
  })
  folder.add(spotLightParams, 'intensity', 0, 4, 0.1).onChange((val) => {
    spotLight.intensity = val
  })
  folder.add(spotLightParams, 'distance', 0, 400, 1).onChange((val) => {
    spotLight.distance = val
  })
  folder.add(spotLightParams, 'decay', 0, 2, 0.1).onChange((val) => {
    spotLight.decay = val
  })
}

// 伯努利双纽线
function lemniscateMove(r: number = 30) {
  let t = 0 // 0到2PI
  return function () {
    t += params.rotateSpeed
    if (t >= 2 * PI) t = 0
    let s = sin(t)
    let c = cos(t)
    let x = (r * s) / (1 + c * c)
    let y = (r * s * c) / (1 + c * c)
    return { x, y }
  }
}

function initMesh() {
  const planeGeo = new THREE.PlaneGeometry(60, 40, 1, 1)
  const planeMat = new THREE.MeshLambertMaterial({
    color: 0x636e72,
    side: THREE.DoubleSide,
  })
  planeMesh = new THREE.Mesh(planeGeo, planeMat)
  planeMesh.rotation.x = -0.5 * PI
  planeMesh.receiveShadow = true
  scene.add(planeMesh)

  const cubeGeo = new THREE.BoxGeometry(4, 4, 4)
  const cubeMat = new THREE.MeshLambertMaterial({
    color: 0xff0000,
    wireframe: false,
  })
  cubeMesh = new THREE.Mesh(cubeGeo, cubeMat)
  cubeMesh.position.set(-14, 2, 0)
  cubeMesh.castShadow = true
  scene.add(cubeMesh)

  const sphereGeo = new THREE.SphereGeometry(4)
  const sphereMat = new THREE.MeshLambertMaterial({
    color: 0x7777ff,
    wireframe: false,
  })
  sphereMesh = new THREE.Mesh(sphereGeo, sphereMat)
  sphereMesh.position.set(4, 4, 0)
  sphereMesh.castShadow = true
  scene.add(sphereMesh)

  const targetGeo = new THREE.SphereGeometry(1)
  const targetMat = new THREE.MeshLambertMaterial({ color: 0xfdcb6e })
  targetMesh = new THREE.Mesh(targetGeo, targetMat)
  targetMesh.position.set(3, 0, 3)
  scene.add(targetMesh)
}

function initLight() {
  ambientLight = new THREE.AmbientLight(params.ambientLightColor)
  scene.add(ambientLight)

  const { color, intensity, distance, decay } = spotLightParams
  spotLight = new THREE.SpotLight(color, intensity, distance, decay)
  spotLight.castShadow = true
  spotLightHelper = new THREE.SpotLightHelper(spotLight)
  scene.add(spotLight)
  scene.add(spotLightHelper)
}

let step = 0
let getTargetPos = lemniscateMove()
function animate() {
  cubeMesh.rotation.x += params.rotateSpeed
  cubeMesh.rotation.y += params.rotateSpeed
  cubeMesh.rotation.z += params.rotateSpeed

  step += params.jumpSpeed
  sphereMesh.position.x = 5 + 10 * Math.cos(step)
  sphereMesh.position.y = 2 + 6 * Math.abs(Math.sin(step))

  // 小球与点光源位置更新
  if(!spotLightParams.stopMoving){
    let { x, y } = getTargetPos()
    targetMesh.position.set(x, 4, y)
    spotLight.position.copy(targetMesh.position)
    spotLight.castShadow = spotLightParams.castShadow
    spotLightHelper.update()
  }
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
  orbitControls.object.position.set(-30, 20, 20)
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
