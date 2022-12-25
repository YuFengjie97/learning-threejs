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

let cube: THREE.Mesh
let sphere: THREE.Mesh
let bar: THREE.Mesh

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
    width: 300
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
}

let rotateSpeed = 0.01
let bouncingStep = 0
let bouncingSpeed = 0.1
let scalingStep = 0
let scalingSpeed = 0.1
function animate() {
  cube.rotation.x += rotateSpeed
  cube.rotation.y += rotateSpeed

  bouncingStep += bouncingSpeed;
  sphere.position.x = 40 + ( 10 * (Math.cos(bouncingStep)));
  sphere.position.y = 2 + ( 10 * Math.abs(Math.sin(bouncingStep)));

  scalingStep += scalingSpeed;
  const scaleX = Math.abs(Math.sin(scalingStep / 4));
  const scaleY = Math.abs(Math.cos(scalingStep / 5));
  const scaleZ = Math.abs(Math.sin(scalingStep / 7));
  bar.scale.set(scaleX, scaleY, scaleZ);
}
function initMesh(){
  const plane =  new THREE.Mesh(new THREE.PlaneGeometry(100,100), new THREE.MeshLambertMaterial({
    color: 0x444444,
  }))
  plane.rotation.x = -PI/2
  plane.receiveShadow = true
  scene.add(plane)

  cube = new THREE.Mesh(new THREE.BoxGeometry(10,10,10), new THREE.MeshLambertMaterial({
    color: 0xff0000
  }))
  cube.castShadow = true
  cube.position.set(-40, 5,0)
  scene.add(cube)

  sphere = new THREE.Mesh(new THREE.SphereGeometry(5), new THREE.MeshLambertMaterial({
    color: 0x00ff00,
  }))
  sphere.position.set(40,5,0)
  sphere.castShadow = true
  scene.add(sphere)

  bar = new THREE.Mesh(new THREE.CylinderGeometry(2, 2, 20), new THREE.MeshLambertMaterial({
    color: 0x0000ff
  }))
  bar.position.set(0,10,0)
  bar.castShadow = true
  scene.add(bar)
}
function initLight(){
  let light = new THREE.SpotLight(0xffffff,2)
  light.position.set(-40,40,40)
  light.castShadow = true
  light.lookAt(new THREE.Vector3(0,0,0))
  let lightHelper = new THREE.SpotLightHelper(light)

  scene.add(light)
  scene.add(lightHelper)
}
// three初始化
function initTHREE() {
  scene = new THREE.Scene()
  // scene.background = new THREE.Color(0x444444)
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

  // 帧率状态
  stats = Stats()
  canvasCon.value!.append(stats.dom)

  // 轨道控制器
  orbitControls = new OrbitControls(camera, renderer.domElement)
  orbitControls.target = new THREE.Vector3(0, 0, 0)
  orbitControls.object.position.set(50, 50, 50)
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
