<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * MeshFaceMaterial已经被废弃，可以直接传入材质数组，自动分配mesh的所有面
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import {vertex, fragment1, fragment2, fragment3, fragment4, fragment5, fragment6} from  './08-shader'

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

onMounted(() => {
  initGUI()
  initTHREE()
  initMesh()
  initLight()
  render()
})

const params = {
  step: 0.02
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)
  gui.add(params, 'step', 0,1,0.01)
}

function animate() {
  cube.rotation.x += params.step
  cube.rotation.y += params.step
  cube.rotation.z += params.step
  ;(cube.material as Array<THREE.ShaderMaterial>).forEach(function (e) {
    e.uniforms.time.value += 0.01;
  });
}
function initMesh() {
  const cubeGeo = new THREE.BoxGeometry(40,40,40)
  const mat1 = createMaterialByShader(vertex, fragment1)
  const mat2 = createMaterialByShader(vertex, fragment2)
  const mat3 = createMaterialByShader(vertex, fragment3)
  const mat4 = createMaterialByShader(vertex, fragment4)
  const mat5 = createMaterialByShader(vertex, fragment5)
  const mat6 = createMaterialByShader(vertex, fragment6)
  const cubeMat = [mat1, mat2, mat3, mat4, mat5, mat6]
  cube =  new THREE.Mesh(cubeGeo, cubeMat)
  // console.log(cube.material);
  // const cube =  new THREE.Mesh(cubeGeo, new THREE.MeshLambertMaterial({color: 0xff0000}))
  scene.add(cube)
  return cube
}
function createMaterialByShader(vertexShader: string, fragmentShader: string){
  const uniforms = {
    time: { type: 'f', value: 0.2},
    scale: { type: 'f', value: 0.2},
    alpha: {type: 'f', value: 0.6},
    resolution: {type: 'v2', value: new THREE.Vector2()}
  }
  uniforms.resolution.value.x = window.innerWidth
  uniforms.resolution.value.y = window.innerHeight

  const meshMaterial = new THREE.ShaderMaterial({
    uniforms,
    vertexShader,
    fragmentShader,
    transparent: true
  })
  return meshMaterial
}
function initLight() {
  const spotLight = new THREE.SpotLight(0xffffff, 1)
  spotLight.target = scene
  spotLight.position.set(-50, 50, 50)
  const spotLightHelper = new THREE.SpotLightHelper(spotLight)
  scene.add(spotLight)
  scene.add(spotLightHelper)
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
  orbitControls.object.position.set(40, 40, 40)
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
