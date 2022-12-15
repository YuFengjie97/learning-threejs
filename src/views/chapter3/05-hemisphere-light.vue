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

import textureGrassImg from '@/assets/texture/grasslight-big.jpg?url'

const { random, PI, floor, ceil, min, max, sin, cos, sqrt, abs } = Math

const canvasDom = ref<HTMLElement>()
const canvasCon = ref<HTMLElement>()
let width = window.innerWidth
let height = window.innerHeight
let stats: Stats
let orbitControls: OrbitControls
let scene: THREE.Scene
let camera: THREE.PerspectiveCamera
let renderer: THREE.WebGLRenderer

let hemiLight: THREE.HemisphereLight
let hemiLightHelper: THREE.HemisphereLightHelper
let dirLight: THREE.DirectionalLight
let dirLightHelper: THREE.DirectionalLightHelper
let plane: THREE.Mesh
let cube: THREE.Mesh
let sphere: THREE.Mesh

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
  initLight()
  render()
})

const controls = {
  rotationSpeed: 0.02,
  bouncingSpeed: 0.03,
}
// 半球光
const lightParams = {
  showHelper: true,
  skyColor: 0x0984e3,
  groundColor: 0x00b894,
  intensity: 2,
}
// 平行光
const dirLightParams = {
  showHelper: true,
  color: 0xffffff,
  intensity: 2,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  const folder1 = gui.addFolder('HemisphereLight')
  folder1.open()
  folder1.add(lightParams, 'showHelper').onChange((val) => {
    hemiLightHelper.visible = val
  })
  folder1.addColor(lightParams, 'skyColor').onChange((val) => {
    hemiLight.color.setHex(val)
    hemiLightHelper.update()
  })
  folder1.addColor(lightParams, 'groundColor').onChange((val) => {
    hemiLight.groundColor.setHex(val)
    hemiLightHelper.update()
  })
  folder1.add(lightParams, 'intensity', 0, 4, 0.1).onChange((val) => {
    hemiLight.intensity = val
    hemiLightHelper.update()
  })

  const folder2 = gui.addFolder('dirLight')
  folder2.open()
  folder2.add(dirLightParams, 'showHelper').onChange((val) => {
    dirLightHelper.visible = val
  })
  folder2.addColor(dirLightParams, 'color').onChange((val) => {
    dirLight.color.setHex(val)
    dirLightHelper.update()
  })
  folder2.add(dirLightParams, 'intensity', 0, 4, 0.1).onChange((val) => {
    dirLight.intensity = val
    dirLightHelper.update()
  })
}

function initMesh() {
  const textureGrass = new THREE.TextureLoader().load(textureGrassImg)
  textureGrass.wrapS = THREE.RepeatWrapping
  textureGrass.wrapT = THREE.RepeatWrapping
  textureGrass.repeat.set(4, 4)
  const planeGeo = new THREE.PlaneGeometry(1000, 200, 20, 20)
  const planeMat = new THREE.MeshLambertMaterial({ map: textureGrass })
  plane = new THREE.Mesh(planeGeo, planeMat)
  scene.add(plane)
  plane.receiveShadow = true

  plane.rotation.x = -0.5 * PI

  const cubeGeo = new THREE.BoxGeometry(4, 4, 4)
  const cubeMat = new THREE.MeshLambertMaterial({ color: 0xfdcb6e })
  cube = new THREE.Mesh(cubeGeo, cubeMat)
  cube.castShadow = true
  cube.position.set(-10, 4, 0)
  scene.add(cube)

  const sphereGeo = new THREE.SphereGeometry(4)
  const sphereMat = new THREE.MeshLambertMaterial({ color: 0xfdcb6e })
  sphere = new THREE.Mesh(sphereGeo, sphereMat)
  sphere.castShadow = true
  sphere.position.y = 2
  scene.add(sphere)
}

function initLight() {
  hemiLight = new THREE.HemisphereLight(
    lightParams.skyColor,
    lightParams.groundColor,
    lightParams.intensity
  )
  // hemiLight.castShadow = true // 半球光是没有阴影的
  hemiLight.position.set(0, 100, 0)
  hemiLightHelper = new THREE.HemisphereLightHelper(hemiLight, 100)
  scene.add(hemiLight)
  scene.add(hemiLightHelper)

  dirLight = new THREE.DirectionalLight(
    dirLightParams.color,
    dirLightParams.intensity
  )
  dirLight.position.set(0, 40, -100)
  dirLight.castShadow = true
  dirLight.target = plane
  dirLight.shadow.camera.near = 0.1;
  dirLight.shadow.camera.far = 200;
  dirLight.shadow.camera.left = -50;
  dirLight.shadow.camera.right = 50;
  dirLight.shadow.camera.top = 50;
  dirLight.shadow.camera.bottom = -50;
  dirLight.shadow.mapSize.width = 2048;
  dirLight.shadow.mapSize.height = 2048;

  dirLightHelper = new THREE.DirectionalLightHelper(dirLight, 100)
  scene.add(dirLight)
  scene.add(dirLightHelper)
}

let step = 0
function animate() {
  cube.rotation.x += controls.rotationSpeed
  cube.rotation.y += controls.rotationSpeed
  cube.rotation.z += controls.rotationSpeed

  step += controls.bouncingSpeed
  sphere.position.x = 20 + 10 * cos(step)
  sphere.position.y = 2 + 10 * abs(sin(step))
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
  // scene.background = new THREE.Color(0x444444)
  scene.fog = new THREE.Fog(0xaaaaaa, 0.01, 200)

  camera = new THREE.PerspectiveCamera(75, width / height, 1, 1000)
  renderer = new THREE.WebGLRenderer({
    canvas: canvasDom.value,
    antialias: true,
  })
  renderer.setClearColor(new THREE.Color(0xaaaaff))
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
  orbitControls.target = new THREE.Vector3(10, 0, 0)
  orbitControls.object.position.set(-20, 15, 45)
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
