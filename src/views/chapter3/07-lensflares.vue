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
import {
  Lensflare,
  LensflareElement
} from 'three/examples/jsm/objects/Lensflare'

import textureGrassImg from '@/assets/texture/grasslight-big.jpg?url'
import textureFlare0 from '@/assets/texture/lensflare/lensflare0.png?url'
import textureFlare3 from '@/assets/texture/lensflare/lensflare3.png?url'

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

let plane: THREE.Mesh
let cube: THREE.Mesh
let sphere: THREE.Mesh
let lensFlareLight: LensFlareLight
let textureLoader: THREE.TextureLoader

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

const lightParams= {
  color: 0xffffff,
  intensity: 1,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.addColor(lightParams, 'color').onChange(val => {
    lensFlareLight.spotLight.color.setHex(val)
    lensFlareLight.spotLightHelper.update()
    ;(lensFlareLight.lensflare.material as THREE.MeshBasicMaterial).color.setHex(val)
  })
  gui.add(lightParams, 'intensity',0, 2*lightParams.intensity,0.1).onChange(val=>{
    lensFlareLight.spotLight.intensity = val
  })
}

function initMesh() {
  textureLoader = new THREE.TextureLoader()
  const textureGrass = textureLoader.load(textureGrassImg)
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
  const cubeMat = new THREE.MeshLambertMaterial({ color: 0xd63031 })
  cube = new THREE.Mesh(cubeGeo, cubeMat)
  cube.castShadow = true
  cube.position.set(-10, 4, 0)
  scene.add(cube)

  const sphereGeo = new THREE.SphereGeometry(4)
  const sphereMat = new THREE.MeshLambertMaterial({ color: 0x0984e3 })
  sphere = new THREE.Mesh(sphereGeo, sphereMat)
  sphere.castShadow = true
  sphere.position.y = 2
  scene.add(sphere)
}

function initLight() {
  lensFlareLight = new LensFlareLight(-30,30,-80)
}

class LensFlareLight {
  spotLight: THREE.SpotLight
  spotLightHelper: THREE.SpotLightHelper
  lensflare: Lensflare
  constructor (x: number, y: number, z: number) {
    this.spotLight = new THREE.SpotLight(lightParams.color, lightParams.intensity)
    this.spotLight.castShadow = true
    this.spotLight.position.set(x,y,z)
    this.spotLightHelper = new THREE.SpotLightHelper(this.spotLight)

    const texture0 = textureLoader.load(textureFlare0)
    const texture3 = textureLoader.load(textureFlare3)
    this.lensflare = new Lensflare()
    this.lensflare.addElement(new LensflareElement(texture0,700,0,new THREE.Color().setHex(0xffffff)))
    this.lensflare.addElement(new LensflareElement(texture3, 60, 0.6))
    this.lensflare.addElement(new LensflareElement(texture3, 70, 0.7))
    this.lensflare.addElement(new LensflareElement(texture3, 120, 0.9))
    this.lensflare.addElement(new LensflareElement(texture3, 70, 1))
    this.spotLight.add(this.lensflare)

    scene.add(this.spotLight)
    scene.add(this.spotLightHelper)
  }
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
