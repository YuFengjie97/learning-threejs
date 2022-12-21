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
import { ParametricGeometry } from 'three/examples/jsm/geometries/ParametricGeometry' 

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
let mesh: THREE.Group
let light: THREE.DirectionalLight
let lightHelper: THREE.DirectionalLightHelper

onMounted(() => {
  initTHREE()
  initMesh()
  initLight()
  initGUI()
  render()
})

const lightParams = {
  color: 0xffffff
}
const matParams = {
  color: 0x3399ff,
  specular: 0xaaaafff,
  shininess: 40,
}
const params = {
  type: 'radialWave',
  rotateSpeed: 0.01,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
  
  gui.add(params, 'type', ['radialWave', 'klein']).onChange(val=>{
    scene.remove(mesh)
    if(val === 'radialWave'){
      mesh = createMesh(new ParametricGeometry(radialWave, 120, 120));
    }else if(val === 'klein'){
      mesh = createMesh(new ParametricGeometry(klein, 120, 120));
    }
    scene.add(mesh)
  })
  gui.add(params, 'rotateSpeed', 0, 0.02,0.005)
  gui.addColor(lightParams, 'color').name('lightColor').onChange(val => {
    light.color.setHex(val)
    lightHelper.update()
  })
  gui.addColor(matParams, 'color').onChange(val=>{
    ;((mesh.children[0] as THREE.Mesh).material as THREE.MeshPhongMaterial).color.setHex(val)
  })
  gui.addColor(matParams, 'specular').onChange(val=>{
    ;((mesh.children[0] as THREE.Mesh).material as THREE.MeshPhongMaterial).specular.setHex(val)
  })
  gui.add(matParams, 'shininess',1,80,1).onChange(val=>{
    ;((mesh.children[0] as THREE.Mesh).material as THREE.MeshPhongMaterial).shininess = val
  })
}

let step = 0
function animate() {
  mesh.rotation.x = step += params.rotateSpeed
  mesh.rotation.y = step += params.rotateSpeed
}
function initMesh() {
  mesh = createMesh(new ParametricGeometry(radialWave, 120, 120));
  scene.add(mesh);
}
function createMesh(geom: THREE.BufferGeometry) {
  if(params.type === 'radialWave') geom.applyMatrix4(new THREE.Matrix4().makeTranslation(-25, 0, -25))
  const meshMaterial = new THREE.MeshPhongMaterial({
    specular: 0xaaaafff,
    color: 0x3399ff,
    shininess: 40,
    side: THREE.DoubleSide
  })
  const plane = createMultiMaterialObject(geom, [meshMaterial])
  plane.position.set(0,0,0)
  return plane
}
function klein(u: number, v: number,target: THREE.Vector3) {
  u *= Math.PI
  v *= 2 * Math.PI

  u = u * 2
  var x, y, z
  if (u < Math.PI) {
    x =
      3 * Math.cos(u) * (1 + Math.sin(u)) +
      2 * (1 - Math.cos(u) / 2) * Math.cos(u) * Math.cos(v)
    z = -8 * Math.sin(u) - 2 * (1 - Math.cos(u) / 2) * Math.sin(u) * Math.cos(v)
  } else {
    x =
      3 * Math.cos(u) * (1 + Math.sin(u)) +
      2 * (1 - Math.cos(u) / 2) * Math.cos(v + Math.PI)
    z = -8 * Math.sin(u)
  }

  y = -2 * (1 - Math.cos(u) / 2) * Math.sin(v)

  target.set(x,y,z)
}
function radialWave(u: number, v: number, target: THREE.Vector3) {
  const r = 50
  const x = Math.sin(u) * r
  const z = Math.sin(v / 2) * 2 * r
  const y = (Math.sin(u * 4 * Math.PI) + Math.cos(v * 2 * Math.PI)) * 2.8

  target.set(x,y,z)
}
function initLight() {
  light = new THREE.DirectionalLight(0xffffff)
  light.position.set(-30, 30, -30)
  light.intensity = 0.7
  light.target = mesh
  lightHelper = new THREE.DirectionalLightHelper(light, 10)
  scene.add(lightHelper)
  scene.add(light)
}
// three初始化
function initTHREE() {
  scene = new THREE.Scene()
  scene.background = new THREE.Color(0xffffff)
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
  orbitControls.object.position.set(0, 30, 50)
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
