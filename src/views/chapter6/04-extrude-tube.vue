<template>
  <div class="viewCon">
    <div class="canvasCon" ref="canvasCon">
      <canvas class="canvas" ref="canvasDom" />
    </div>
  </div>
</template>

<script lang="ts" setup>
/**
 * 我感觉动态的修改mesh的geometry很麻烦
 * 还是原来remove掉，重新创建geo，mesh比较方便
 */
import { ref, onMounted } from 'vue'
import { GUI } from 'dat.gui'
import * as THREE from 'three'
import Stats from 'three/examples/jsm/libs/stats.module.js'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js'
import { getRandomVec3, CustomCurve } from '@/utils'
import { createMultiMaterialObject } from 'three/examples/jsm/utils/SceneUtils'
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
let tube: THREE.Group
let spheres: THREE.Group = new THREE.Group()
let points: THREE.Vector3[] = []

onMounted(() => {
  initTHREE()
  initMesh()
  initGUI()
  render()
})

const tubeGeometryOptions = {
  tubularSegments: 64,
  radius: 1,
  radialSegments: 8,
  closed: false,
}
const params = {
  rotateSpeed: 0.005,
  pointsNum: 10,
  redraw: function () {
    updateMesh()
  },
}
function initGUI() {
  let gui = new GUI({
    autoPlace: false,
    width: 300,
  })
  gui.domElement.style.cssText = 'position: fixed; top: 0; right: 0;'
  canvasCon.value?.appendChild(gui.domElement)

  gui.add(params, 'rotateSpeed', 0, 0.02, 0.005)
  gui.add(params, 'pointsNum', 2, 40, 1).onChange(() => updateMesh())
  gui
    .add(tubeGeometryOptions, 'tubularSegments', 1, 128, 1)
    .onChange(() => updateTube())
  gui
    .add(tubeGeometryOptions, 'radius', 1, 10, 0.1)
    .onChange(() => updateTube())
  gui
    .add(tubeGeometryOptions, 'radialSegments', 2, 40, 1)
    .onChange(() => updateTube())
  gui.add(tubeGeometryOptions, 'closed').onChange(() => updateTube())
  gui.add(params, 'redraw').listen()
}

let step = 0
function animate() {
  spheres.rotation.x = step += params.rotateSpeed
  tube.rotation.x = step += params.rotateSpeed
  spheres.rotation.y = step += params.rotateSpeed
  tube.rotation.y = step += params.rotateSpeed
}
// 获取随机点
function setPoints() {
  points.length = 0
  let min = -15
  let max = 15
  for (let i = 0; i < params.pointsNum; i++) {
    const pos = getRandomVec3(min, max)
    points.push(pos)
  }
}
// 使用随机点初始化tubeGeo
function initTubeGeo() {
  const { tubularSegments, radius, radialSegments, closed } =
    tubeGeometryOptions
  const curvePath = new CustomCurve(points)
  const tubeGeo = new THREE.TubeGeometry(
    curvePath,
    tubularSegments,
    radius,
    radialSegments,
    closed
  )
  return tubeGeo
}
// 使用tubeGeo初始化tube
function initTube(geo: THREE.TubeGeometry) {
  const mat1 = new THREE.MeshBasicMaterial({
    color: 0x0984e3,
    transparent: true,
    opacity: 0.5,
    side: THREE.DoubleSide,
  })
  const mat2 = new THREE.MeshBasicMaterial({ wireframe: true })
  tube = createMultiMaterialObject(geo, [mat1, mat2])
  scene.add(tube)
}
// 使用points初始化点
function initSpheres() {
  points.forEach((p) => {
    const sphere = new THREE.Mesh(
      new THREE.SphereGeometry(0.3),
      new THREE.MeshBasicMaterial({ color: 0xff0000 })
    )
    sphere.position.set(p.x, p.y, p.z)
    spheres.add(sphere)
  })
  return spheres
}
// 初始化
function initMesh() {
  scene.add(spheres)
  setPoints()
  initSpheres()
  initTube(initTubeGeo())
}

// 重新获取随机点，更新点与tube
function updateMesh() {
  setPoints()
  
  scene.remove(spheres)
  spheres = new THREE.Group()
  scene.add(spheres)
  
  initSpheres()
  updateTube()
}
// 只更新tube
function updateTube() {
  const tubeGeo = initTubeGeo()
  ;(tube.children[0] as THREE.Mesh).geometry.dispose()
  ;(tube.children[1] as THREE.Mesh).geometry.dispose()
  ;(tube.children[0] as THREE.Mesh).geometry = tubeGeo
  ;(tube.children[1] as THREE.Mesh).geometry = tubeGeo
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

  // 环境光
  scene.add(new THREE.AmbientLight(0xffffff))

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
