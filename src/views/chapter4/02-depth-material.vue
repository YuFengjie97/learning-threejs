<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * scene.overrideMaterial可以将场景中的所有材质以这个材质来渲染
 * camera的near，far属性修改后，需要调用updateProjectMatrix()来更新
 */
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
// let cubeMat: THREE.MeshDepthMaterial = new THREE.MeshDepthMaterial()
let cubeMat: THREE.MeshLambertMaterial = new THREE.MeshLambertMaterial()
let cubes: Array<THREE.Mesh> = []

onMounted(() => {
  initTHREE()
  initGUI()

  render()
})

const cubeParams = {
  rotateSpeed: 0.01,
  addCube: function () {
    const s = random() * 3 + 3
    const cubeGeo = new THREE.BoxGeometry(s, s, s)
    const cube = new THREE.Mesh(cubeGeo, cubeMat)
    cube.position.x = 20 - random() * 40
    cube.position.y = 20 - random() * 40
    cube.position.z = 100 - random() * 100
    cubes.push(cube)
    scene.add(cube)
  },
  removeCube: function () {
    const lastCube = cubes.pop()
    if (lastCube) scene.remove(lastCube)
  },
}
const cameraParams = {
  near: 1,
  far: 130,
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value!.appendChild(gui.domElement)

  for (let i = 0; i < 10; i++) {
    cubeParams.addCube()
  }

  gui.add(cubeParams, 'rotateSpeed', 0, 1, 0.01)
  gui.add(cubeParams, 'addCube').listen()
  gui.add(cubeParams, 'removeCube').listen()

  gui.add(cameraParams, 'near', 0, 50, 1).onChange((val) => {
    // ;(orbitControls.object as THREE.PerspectiveCamera).near = val
    camera.near = val
    camera.updateProjectionMatrix()
    // orbitControls.update()
  })
  gui.add(cameraParams, 'far', 50, 200, 1).onChange((val) => {
    // ;(orbitControls.object as THREE.PerspectiveCamera).far = val
    camera.far = val
    camera.updateProjectionMatrix()
    // orbitControls.update()
  })
}

function animate() {
  cubes.forEach((c) => {
    c.rotation.x += cubeParams.rotateSpeed
    c.rotation.y += cubeParams.rotateSpeed
    c.rotation.z += cubeParams.rotateSpeed
  })
}
// three初始化
function initTHREE() {
  scene = new THREE.Scene()
  // scene.background = new THREE.Color(0x444444)
  camera = new THREE.PerspectiveCamera(75, width / height, 10, 130)
  scene.overrideMaterial = new THREE.MeshDepthMaterial() //强制使用场景中的所有材质以这个材质来渲染
  renderer = new THREE.WebGLRenderer({
    canvas: canvasDom.value,
    antialias: true,
  })
  renderer.setClearColor(new THREE.Color(0x000000))
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
  orbitControls.object.position.set(0, 3, 100)
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
